using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Members
{
    public class IndexModel : AdminAreaPageModel
    {
        public IndexModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager) : base(context, userManager) { }

        public IList<Member> Member { get;set; }

        public async Task OnGetAsync()
        {
            Member = await _context.Members.ToListAsync();
        }
    }
}
