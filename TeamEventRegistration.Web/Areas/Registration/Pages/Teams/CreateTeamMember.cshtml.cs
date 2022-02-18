using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Teams
{
    public class CreateTeamMember : RegistrationAreaPageModel
    {
        private Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> _registrationServiceDelegate;

        public CreateTeamMember(TeamEventRegistrationDbContext context, UserManager<Member> userManager, 
            IAuthorizationService authorizationService, 
            Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> registrationServiceDelegate)
            : base(context, userManager, authorizationService) 
        { 
            _registrationServiceDelegate = registrationServiceDelegate;
        }

        public IActionResult OnGet(int TeamId)
        {
            this.TeamId = TeamId;
            var team = _context.Teams
                            .Include(t => t.Event)
                            .Include(t => t.Registrations)
                            .FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
                return NotFound($"Team not found with ID: {TeamId}");
            }
            if (team.Registrations.Count >= team.Event.MaxTeamMembers)
            {
                ModelState.AddModelError(string.Empty, $"Cannot add member to team. Max number of members has been reached.");
            }
            return Page();
        }

        [BindProperty]
        public Member Member { get; set; }
        public int TeamId { get; set; }

        public async Task<IActionResult> OnPostAsync(int TeamId)
        {
            var team = _context.Teams
                            .Include(t => t.Event)
                            .Include(t => t.Registrations)
                            .FirstOrDefault(t => t.Id == TeamId);
            if (team.Registrations.Count >= team.Event.MaxTeamMembers)
            {
                ModelState.AddModelError(string.Empty, $"Cannot add member to team. Max number of members has been reached.");
            }
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
                    IRegistrationService _registrationService = _registrationServiceDelegate(RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature);
                    await _registrationService.CreateAsync(user.Id, team.EventId, team.Id, false);
                    return RedirectToPage("./Edit", new { Id = TeamId });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
    }
}
