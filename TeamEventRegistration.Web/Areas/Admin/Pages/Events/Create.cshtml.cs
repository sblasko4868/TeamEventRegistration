using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Events
{
    public class CreateModel : EventsPageModel
    {
        public CreateModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService) : base(context, userManager, authorizationService) { }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Core.Models.Event Event { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
