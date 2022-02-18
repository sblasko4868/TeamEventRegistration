using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Event.Pages
{
    public abstract class EventAreaPageModel : PageModel
    {
        protected readonly TeamEventRegistrationDbContext _context;
        protected readonly UserManager<Member> _userManager;
        protected readonly IAuthorizationService _authorizationService;

        protected EventAreaPageModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
            : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }
    }
}
