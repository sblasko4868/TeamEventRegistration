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
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Activities
{
    public class CreateModel : AdminAreaPageModel
    {
        private IHtmlSanitizer _htmlSanitizer;

        public CreateModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IHtmlSanitizer htmlSanitizer)
            :base(context, userManager)
        {
            _htmlSanitizer = htmlSanitizer;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Activity Activity { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Activity.ActivityDescription = _htmlSanitizer.Sanitize(WebUtility.HtmlDecode(Activity.ActivityDescription));
            Activity.ActivityRules = _htmlSanitizer.Sanitize(WebUtility.HtmlDecode(Activity.ActivityRules));
            _context.Activities.Add(Activity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
