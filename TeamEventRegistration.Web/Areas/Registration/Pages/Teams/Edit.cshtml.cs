using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Authorization;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Teams
{
    public class EditModel : RegistrationAreaPageModel
    {
        private Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> _registrationServiceDelegate;

        public EditModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService,
            Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> registrationServiceDelegate)
            : base(context, userManager, authorizationService) 
        {
            _registrationServiceDelegate = registrationServiceDelegate;
        }

        [BindProperty]
        public Team Team { get; set; }
        [BindProperty]
        public string[] SelectedTeamMemberIds { get; set; }
        public string SaveAndAddTeamMember => "Save and Create New Team Member";
        [BindProperty]
        public string CaptainId { get; set; }
        public SelectList SelectedTeamMembersList { get; set; }
        public MultiSelectList AvailableTeamMembersList { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(t => t.Event)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (Team == null)
            {
                return NotFound();
            }

            if (_authorizationService.AuthorizeAsync(User, Team, AuthorizationOperations.Update).Result.Succeeded)
            {
                SelectedTeamMemberIds = Team.Registrations.Select(r => r.MemberId).ToArray();
                AvailableTeamMembersList = new MultiSelectList(
                    _context.Members
                        .Include(m => m.Registrations)
                        .Where(m => m.Registrations.Where(r => r.TeamId == null || r.TeamId == Team.Id).Any() ||
                                m.Registrations.Count == 0)
                         .Select(m => new {
                             m.Id,
                             m.FullName,
                             RegistrationComplete = m.Registrations
                            .Where(r => r.EventId == Team.EventId && r.RegistrationComplete).Any() ? "Registration Complete" : "Registration Not Complete"
                         }),
                    nameof(Core.Models.Member.Id),
                    nameof(Core.Models.Member.FullName),
                    null,
                    "RegistrationComplete"
                );
                CaptainId = Team.Registrations.Where(r => r.IsCaptain).Select(r => r.MemberId).FirstOrDefault();
                SelectedTeamMembersList = new SelectList(
                    Team.Registrations.Select(r => new { r.Member.Id, r.Member.FullName }),
                    nameof(Core.Models.Member.Id),
                    nameof(Core.Models.Member.FullName)
                );
                return Page();
            }
            else
            {
                return Forbid();
            }
            
        }

        public async Task<IActionResult> OnPostAsync(int? id, string submit)
        {
            var teamToUpdate = await _context.Teams
                .Include(t => t.Event)
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teamToUpdate.Event.MaxTeamMembers != null && SelectedTeamMemberIds.Length > teamToUpdate.Event.MaxTeamMembers)
            {
                ModelState.AddModelError(string.Empty, $"{SelectedTeamMemberIds.Length} Members Selected. Maximum allowed: {teamToUpdate.Event.MaxTeamMembers}");
            }

            if (!ModelState.IsValid || id == null)
            {
                return Page();
            }

            if (await TryUpdateModelAsync<Team>(teamToUpdate, "Team", t => t.TeamName))
            {
                // If no team members are selected, set registrations to empty
                if (SelectedTeamMemberIds.Length == 0)
                {
                    teamToUpdate.Registrations = new List<Core.Models.Registration>();
                }
                else
                {
                    var registrations = _context.Registrations;
                    
                    var teamToUpdateRegistrations = teamToUpdate.Registrations.ToList();
                    
                    // Clear current captain assignments
                    teamToUpdateRegistrations.Select(r => { r.IsCaptain = false; return r; }).ToList();
                    
                    // Handle member removed from a team
                    teamToUpdateRegistrations.RemoveAll(r => !SelectedTeamMemberIds.Contains(r.MemberId));

                    // Handle registered members joining a team
                    teamToUpdateRegistrations.AddRange(registrations
                        .Where(r => r.EventId == teamToUpdate.EventId &&
                            SelectedTeamMemberIds.Contains(r.MemberId) && 
                            r.TeamId == null)
                        .ToList());

                    bool captainIsNewRegistration = false;
                    foreach (var selectedTeamMember in SelectedTeamMemberIds.Where(s => !teamToUpdateRegistrations.Select(t => t.MemberId).Contains(s)))
                    {
                        // Handle unregistered members joining a team
                        // TODO: Potential bug. Not adding to the team.
                        if (selectedTeamMember == CaptainId) { captainIsNewRegistration = true; }
                        IRegistrationService _registrationService = _registrationServiceDelegate(RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature);
                        await _registrationService.CreateAsync(selectedTeamMember, teamToUpdate.EventId, teamToUpdate.Id, captainIsNewRegistration);                        
                    }

                    // Set team captain
                    if (!string.IsNullOrEmpty(CaptainId) && !captainIsNewRegistration) {
                        teamToUpdateRegistrations.Where(r => r.MemberId == CaptainId).FirstOrDefault().IsCaptain = true;
                    }

                    teamToUpdate.Registrations = teamToUpdateRegistrations;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(Team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            if (submit == SaveAndAddTeamMember)
            {
                return RedirectToPage("./CreateTeamMember", new { TeamId = Team.Id });
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
