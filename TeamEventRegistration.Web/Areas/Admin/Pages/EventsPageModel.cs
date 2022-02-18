using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages
{
    public class EventsPageModel : AdminAreaPageModel
    {
        public readonly IAuthorizationService _authorizationService;
        public EventsPageModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
            : base(context, userManager)
        {
            _authorizationService = authorizationService;
        }
    }
}
