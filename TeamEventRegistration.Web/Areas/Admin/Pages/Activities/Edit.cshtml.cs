using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Activities
{
    public class EditModel : AdminAreaPageModel
    {
        private IHtmlSanitizer _htmlSanitizer;

        public EditModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IHtmlSanitizer htmlSanitizer)
            :base(context, userManager)
        {
            _htmlSanitizer = htmlSanitizer;
        }

        [BindProperty]
        public Activity Activity { get; set; }
        public MultiSelectList SelectedEventsSelectList { get; set; }

        public MultiSelectList AvailableEventsSelectList { get; set; }

        [BindProperty]
        public int[] SelectedEvents { get; set; }

        public int[] AvailableEvents { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Activity = await _context.Activities
                .Include(a => a.EventActivities)
                .ThenInclude(ea => ea.Event)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Activity == null)
            {
                return NotFound();
            }

            var selectedEvents = Activity.EventActivities.Select(ea => ea.Event);
            var tempSelectedEventIds = selectedEvents.Select(e => e.Id).ToArray();
            SelectedEventsSelectList = new MultiSelectList(selectedEvents, nameof(Core.Models.Event.Id), nameof(Core.Models.Event.EventName));
            AvailableEventsSelectList = new MultiSelectList(_context.Events.Where(e => !tempSelectedEventIds.Contains(e.Id)), nameof(Core.Models.Event.Id), nameof(Core.Models.Event.EventName));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Activity = await _context.Activities
                .Include(a => a.EventActivities)
                .FirstOrDefaultAsync(m => m.Id == id);
            Activity.ActivityDescription = _htmlSanitizer.Sanitize(WebUtility.HtmlDecode(Activity.ActivityDescription));
            Activity.ActivityRules = _htmlSanitizer.Sanitize(WebUtility.HtmlDecode(Activity.ActivityRules));
            Activity.EventActivities = new List<EventActivity>();
            foreach (var selectedEvent in SelectedEvents)
            {
                Activity.EventActivities.Add(new EventActivity() { EventId = selectedEvent, ActivityId = Activity.Id }); 
            }
            _context.Attach(Activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(Activity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
