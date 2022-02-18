using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Roles
{
    public class EditModel : AdminAreaPageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager)
        {
            _roleManager = roleManager;
        }

        public MultiSelectList SelectedMembersSelectList { get; set; }

        public MultiSelectList AvailableMembersSelectList { get; set; }
        [BindProperty]
        public string[] SelectedMembers { get; set; }
        [BindProperty]
        public string[] AvailableMembers { get; set; }

        public IdentityRole Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Role = _roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();
            if (Role == null)
            {
                return NotFound();
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync(Role.Name);
            var tempSelectedMembers = usersInRole.Select(u => u.Id).ToArray();
            SelectedMembersSelectList = new MultiSelectList(usersInRole, nameof(Core.Models.Member.Id), nameof(Core.Models.Member.FullName));
            AvailableMembersSelectList = new MultiSelectList(_userManager.Users.Where(u => !tempSelectedMembers.Contains(u.Id)), nameof(Core.Models.Member.Id), nameof(Core.Models.Member.FullName));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Role = _roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();
            if (Role == null)
            {
                return NotFound();
            }
            var usersInRole = _userManager.GetUsersInRoleAsync(Role.Name).Result.Select(u => u.Id).ToArray();

            var usersToAdd = SelectedMembers.Except(usersInRole);
            foreach (var userToAdd in usersToAdd)
            {
                await _userManager.AddToRoleAsync(_userManager.Users.Where(u => u.Id == userToAdd).FirstOrDefault(), Role.Name);
            }

            var usersToRemove = usersInRole.Except(SelectedMembers);
            foreach (var userToRemove in usersToRemove)
            {
                await _userManager.RemoveFromRoleAsync(_userManager.Users.Where(u => u.Id == userToRemove).FirstOrDefault(), Role.Name);
            }

            return RedirectToPage("./Index");
        }

    }
}
