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

namespace TeamEventRegistration.Web.Areas.Event.Pages
{
    public class EventActivitiesModel : EventAreaPageModel
    {
        public IList<Activity> Activities { get; set; }

        public EventActivitiesModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
        :base(context, userManager, authorizationService)
        {

        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var eventObj = await _context.Events
                .Include(e => e.EventActivities)
                .ThenInclude(ea => ea.Activity)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (eventObj == null)
            {
                return NotFound();
            }

            if (!_authorizationService.AuthorizeAsync(User, eventObj, AuthorizationOperations.Read).Result.Succeeded)
            {
                return Forbid();
            }

            if (eventObj.DisplayEventActivities) 
            {
                Activities = eventObj.EventActivities.Select(ea => ea.Activity).ToList();
            }
            else
            {
                Activities = new List<Activity>();
            }
            return Page();
        }
    }
}
