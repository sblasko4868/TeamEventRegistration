using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Activities
{
    public class IndexModel : AdminAreaPageModel
    {
        public IndexModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager)
            :base(context, userManager)
        {
        }

        public IList<Activity> Activities { get;set; }

        public async Task OnGetAsync()
        {
            Activities = await _context.Activities.ToListAsync();
        }
    }
}
