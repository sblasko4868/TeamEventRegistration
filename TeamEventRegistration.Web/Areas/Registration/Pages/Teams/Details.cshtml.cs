using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Authorization;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Teams
{
    public class DetailsModel : RegistrationAreaPageModel
    {
        public DetailsModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
            : base(context, userManager, authorizationService) { }

        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(t => t.Event)
                .Include(t => t.Registrations)
                        .ThenInclude(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }

            if (_authorizationService.AuthorizeAsync(User, Team, AuthorizationOperations.Read).Result.Succeeded)
            {
                return Page();
            }
            else
            {
                return Forbid();
            }
        }
    }
}
