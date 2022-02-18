using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Admin.Pages.Roles
{
    public class IndexModel : AdminAreaPageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(TeamEventRegistrationDbContext context, 
            UserManager<Member> userManager,
            RoleManager<IdentityRole> roleManager)
            : base (context, userManager)
        {
            _roleManager = roleManager;
        }
        public IList<RoleData> Roles { get; set; }

        public class RoleData
        {
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public int MemberCount { get; set; }
        }
        public async void OnGet()
        {
            Roles = new List<RoleData>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                Roles.Add(new RoleData
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    MemberCount = _userManager.GetUsersInRoleAsync(role.Name).Result.Count
                });
            }
        }
    }
}
