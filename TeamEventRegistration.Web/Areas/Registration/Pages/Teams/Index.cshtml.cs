using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Teams
{
    public class IndexModel : RegistrationAreaPageModel
    {
        public IndexModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
            : base(context, userManager, authorizationService) { }

        //public IList<Team> Teams { get;set; }
        public List<Core.Models.Event> Events { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (_authorizationService.AuthorizeAsync(User, AuthorizationEnumerations.Policies.RequireEventAdministrator.ToString()).Result.Succeeded)
            {
                Events = await _context.Events
                    .Include(e => e.Registrations)
                        .ThenInclude(r => r.Member)
                    .Include(e => e.Teams)
                        .ThenInclude(t => t.Registrations)
                            .ThenInclude(r => r.Member)
                    .Where(e => e.Active)
                    .ToListAsync();
            }
            else
            {
                var userEvents = _context.Registrations
                    .Where(r => r.MemberId == user.Id)
                    .Select(r => r.EventId)
                    .ToList();
                userEvents.AddRange(_context.EventManagers
                                    .Where(em => em.MemberId == user.Id)
                                    .Select(em => em.EventId));
                Events = await _context.Events
                    .Include(e => e.Registrations)
                        .ThenInclude(r => r.Member)
                    .Include(e => e.Teams)
                        .ThenInclude(t => t.Registrations)
                            .ThenInclude(r => r.Member)
                    .Where(e => userEvents.Contains(e.Id))
                    .ToListAsync();
            }
        }
    }
}
