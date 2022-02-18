using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Members
{
    public class EditModel : AdminAreaPageModel
    {
        private readonly SignInManager<Member> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(TeamEventRegistrationDbContext context, 
            UserManager<Member> userManager,
            SignInManager<Member> signInManager,
            RoleManager<IdentityRole> roleManager)
            : base (context, userManager)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public Member Member { get; set; }
        public MultiSelectList SelectedRolesSelectList { get; set; }

        public MultiSelectList AvailableRolesSelectList { get; set; }
        [BindProperty]
        public string[] SelectedRoles { get; set; }
        [BindProperty]
        public string[] AvailableRoles { get; set; }

        private void LoadAsync(Member user)
        {
            Member = new Member
            {
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName
            };
        }

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

            var userRoles = await _userManager.GetRolesAsync(Member);
            SelectedRolesSelectList = new MultiSelectList(userRoles);
            AvailableRolesSelectList = new MultiSelectList(_roleManager.Roles.Where(r => !userRoles.Contains(r.Name)));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                LoadAsync(user);
                return Page();
            }

            user.UserName = Member.Email;
            user.Email = Member.Email;
            user.FirstName = Member.FirstName;
            user.PhoneNumber = Member.PhoneNumber;
            user.LastName = Member.LastName;
            user.NickName = Member.NickName;
            user.StreetAddress = Member.StreetAddress;
            user.City = Member.City;
            user.State = Member.State;
            user.ZipCode = Member.ZipCode;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                //StatusMessage = "Unexpected error when trying to update user.";
                return RedirectToPage();
            }

            var userRoles = await _userManager.GetRolesAsync(Member);

            var rolesToAdd = SelectedRoles.Except(userRoles);
            var addRoleResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

            var rolesToRemove = userRoles.Except(SelectedRoles);
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            
            if (User.Identity.Name == user.UserName)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            //StatusMessage = "Your profile has been updated";
            return RedirectToPage("./Index");
        }
    }
}
