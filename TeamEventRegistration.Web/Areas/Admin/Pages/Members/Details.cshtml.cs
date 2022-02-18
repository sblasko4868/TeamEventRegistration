using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Members
{
    public class DetailsModel : AdminAreaPageModel
    {
        public DetailsModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager) : base(context, userManager) { }

        public Member Member { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);

            if (Member == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
