using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;
using TeamEventRegistration.Core.Authorization;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Events
{
    public class IndexModel : EventsPageModel
    {
        public IndexModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService) : base(context, userManager, authorizationService) { }

        public IList<Core.Models.Event> Event { get;set; }

        public async Task OnGetAsync()
        {
            Event = await _context.Events.ToListAsync();
        }
    }
}
