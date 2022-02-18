using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Events
{
    public class EditModel : EventsPageModel
    {
        public EditModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService) : base(context, userManager, authorizationService) { }

        [BindProperty]
        public Core.Models.Event Event { get; set; }
        [BindProperty]
        public string[] SelectedEventManagerIds { get; set; }
        public MultiSelectList AvailableEventManagersList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = _context.Events
                .Include(e => e.EventManagers)
                    .ThenInclude(em => em.Member)
                .FirstOrDefault(e => e.Id == id);

            if (Event == null)
            {
                return NotFound();
            }

            SelectedEventManagerIds = Event.EventManagers.Select(em => em.Member.Id).ToArray();
            AvailableEventManagersList = new MultiSelectList(
                _context.Members.Select(m => new { m.Id, m.FullName }),
                nameof(Core.Models.Member.Id),
                nameof(Core.Models.Member.FullName));

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.Id))
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

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
