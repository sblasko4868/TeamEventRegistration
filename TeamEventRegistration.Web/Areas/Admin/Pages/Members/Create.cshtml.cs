using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Members
{
    public class CreateModel : AdminAreaPageModel
    {
        public CreateModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager) : base(context, userManager) { }

        public IActionResult OnGet()
        {
            return Page();
        }
        public string CreateAndManageRoles => "Create and Manage Roles";

        [BindProperty]
        public Member Member { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string submit)
        {

            if (ModelState.IsValid)
            {
                var user = new Member
                {
                    UserName = Member.Email,
                    Email = Member.Email,
                    FirstName = Member.FirstName,
                    LastName = Member.LastName,
                    NickName = Member.NickName,
                    PhoneNumber = Member.PhoneNumber,
                    StreetAddress = Member.StreetAddress,
                    City = Member.City,
                    State = Member.State,
                    ZipCode = Member.ZipCode,
                    SelfCreation = false
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    if (submit == CreateAndManageRoles)
                    {
                        return RedirectToPage("./Edit", new { id = user.Id });
                    }
                    else
                    {
                        return RedirectToPage("./Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
