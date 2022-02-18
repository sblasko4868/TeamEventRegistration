using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Events
{
    public class DetailsModel : EventsPageModel
    {
        // TODO: Redirect to event bracket
        public DetailsModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService) : base(context, userManager, authorizationService) { }

        public Core.Models.Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Events
                .Include(e => e.Teams)
                .FirstOrDefaultAsync(m => m.Id == id);
            

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
