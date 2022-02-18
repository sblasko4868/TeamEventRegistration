using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin
{
    public abstract class AdminAreaPageModel : PageModel
    {
        protected readonly TeamEventRegistrationDbContext _context;
        protected readonly UserManager<Member> _userManager;

        protected AdminAreaPageModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager)
            : base()
        {
            _context = context;
            _userManager = userManager;
        }
    }
}
