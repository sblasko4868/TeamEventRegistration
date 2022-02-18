using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration
{
    public abstract class RegistrationAreaPageModel : PageModel
    {
        protected readonly TeamEventRegistrationDbContext _context;
        protected readonly UserManager<Member> _userManager;
        protected readonly IAuthorizationService _authorizationService;

        protected RegistrationAreaPageModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService)
            : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }
    }
}
