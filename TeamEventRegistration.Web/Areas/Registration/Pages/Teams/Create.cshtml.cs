using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Teams
{
    public class CreateModel : RegistrationAreaPageModel
    {
        private Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> _registrationServiceDelegate;
        public CreateModel(TeamEventRegistrationDbContext context, UserManager<Member> userManager, IAuthorizationService authorizationService,
            Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> registrationServiceDelegate)
            : base(context, userManager, authorizationService)
        {
            _registrationServiceDelegate = registrationServiceDelegate;
        }
        public string EventName { get; set; }

        [BindProperty]
        public string SelectedCaptainId { get; set; }
        public SelectList AvailableCaptainsSelectList { get; set; }
        public enum RegistrationCondition
        {
            EventManager,
            UserDoesNotHaveRegistration,
            UserIsOnlyMemberOfTeam,
            UserIsTeamCaptain,
            UserIsRegistered
        }

        [BindProperty]
        [Required]
        [Display(Name = "Name")]
        public string NewTeamName { get; set; }

        public string CurrentTeamName { get; set; }

        [BindProperty]
        public int? CurrentUserTeamRegistrationId { get; set; }

        [BindProperty]
        public RegistrationCondition Condition { get; set; }

        public string FormMessage { get; set; }

        public async Task<IActionResult> OnGet(int eventId)
        {
            var teamEvent = _context.Events
                .Include(e => e.EventManagers)
                .FirstOrDefault(e => e.Id == eventId);
            
            if (teamEvent == null)
            {
                return NotFound();
            }
            var userEventRegistration = _context.Registrations
                .Where(r => r.MemberId == _userManager.GetUserId(User) && r.EventId == teamEvent.Id)
                .FirstOrDefault();

            FormMessage = "";

            if (_authorizationService.AuthorizeAsync(User, AuthorizationEnumerations.Policies.RequireEventAdministrator.ToString()).Result.Succeeded || 
                teamEvent.EventManagers.Where(em => em.MemberId == _userManager.GetUserId(User)).Any())
            {
                Condition = RegistrationCondition.EventManager;
            }
            else
            {
                CurrentUserTeamRegistrationId = userEventRegistration.Id;
                if (userEventRegistration == null)
                {
                    Condition = RegistrationCondition.UserDoesNotHaveRegistration;
                    FormMessage = $"You will be added to this team and registered for {teamEvent.EventName} after the team is created.";
                }
                else if (userEventRegistration != null && userEventRegistration.TeamId == null)
                {
                    Condition = RegistrationCondition.UserIsRegistered;
                    FormMessage = $"You will be added to this team after it is created.";
                }
                else
                {
                    var currentTeam = _context.Teams
                        .Include(t => t.Registrations)
                            .ThenInclude(r => r.Member)
                        .Where(t => t.Id == userEventRegistration.TeamId)
                        .FirstOrDefault();
                    CurrentTeamName = currentTeam.TeamName;
                    var otherTeamMemberRegistrations = currentTeam.Registrations.Where(r => r.MemberId != _userManager.GetUserId(User));
                    if (otherTeamMemberRegistrations.Any())
                    {
                        if (userEventRegistration.IsCaptain)
                        {
                            Condition = RegistrationCondition.UserIsTeamCaptain;
                            AvailableCaptainsSelectList = new SelectList(otherTeamMemberRegistrations.Select(r => new { r.Member.Id, r.Member.FullName }),
                                nameof(Core.Models.Member.Id),
                                nameof(Core.Models.Member.FullName));
                            FormMessage = $"Select a new captain for the {CurrentTeamName} team.";
                        }
                        else
                        {
                            Condition = RegistrationCondition.UserIsRegistered;
                            FormMessage = $"You will be removed from {CurrentTeamName}.";
                        }
                    }
                    else
                    {
                        Condition = RegistrationCondition.UserIsOnlyMemberOfTeam;
                        FormMessage = $"{CurrentTeamName} will be deleted.";
                    }
                }
            }

            EventName = teamEvent.EventName;
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync(int eventId)
        {
            if (await ValidatePostAsync(eventId, CurrentUserTeamRegistrationId))
            {
                if (_context.Teams.Where(t => t.EventId == eventId && t.TeamName == NewTeamName).Any())
                {
                    ModelState.AddModelError(string.Empty, "Team Name is not unique");
                    return Page();
                }
                var newTeam = await CreateTeam(eventId);
                return RedirectToPage("/Index");
            }
            else
            {
                return NotFound();
            }

        }
        public async Task<IActionResult> OnPostSaveAndAddMembersAsync(int eventId)
        {
            if (await ValidatePostAsync(eventId, CurrentUserTeamRegistrationId))
            {
                if (_context.Teams.Where(t => t.EventId == eventId && t.TeamName == NewTeamName).Any())
                {
                    ModelState.AddModelError(string.Empty, "Team Name is not unique");
                    return Page();
                }
                var newTeam = await CreateTeam(eventId);
                return RedirectToPage("./Edit", new { id = newTeam.Entity.Id });
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<bool> ValidatePostAsync(int eventId, int? registrationId)
        {
            var checkEvent = await _context.Events.FindAsync(eventId);
            if (checkEvent == null) return false;
            if (registrationId.HasValue)
            {
                var registration = await _context.Registrations.FindAsync(registrationId);
                if (registration == null) return false;
            }
            return true;
        }

        public async Task<EntityEntry<Team>> CreateTeam(int eventId)
        {
            var newTeam = new Team()
            {
                TeamName = NewTeamName,
                EventId = eventId
            };
            var result = _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            switch (Condition)
            {
                case RegistrationCondition.EventManager:
                    // Create team without assigning user as captain
                    // Nothing to do because we created team previously
                    // Test Event Manager: test5 (Manager) and test8 (Admin) (DONE)
                    break;
                case RegistrationCondition.UserIsTeamCaptain:
                    {
                        // Change captain of old team to selected user
                        // Add user as captain of new team
                        // Test Team Captain: test4 (DONE)
                        var currentUserTeamRegistration = _context.Registrations
                              .Include(r => r.Team)
                              .Where(r => r.Id == CurrentUserTeamRegistrationId)
                              .FirstOrDefault();
                        var newTeamCaptainRegistration = _context.Registrations
                            .Where(r => r.MemberId == SelectedCaptainId && 
                                r.EventId == currentUserTeamRegistration.EventId && 
                                r.TeamId == currentUserTeamRegistration.TeamId)
                            .FirstOrDefault();
                        newTeamCaptainRegistration.IsCaptain = true;
                        currentUserTeamRegistration.TeamId = result.Entity.Id;
                        currentUserTeamRegistration.IsCaptain = true;
                        break;
                    }
                case RegistrationCondition.UserIsOnlyMemberOfTeam:
                    // Delete team and warn user
                    // Test only member of team: test1 (DONE)
                    {
                        var currentUserTeamRegistration = _context.Registrations
                          .Include(r => r.Team)
                          .Where(r => r.Id == CurrentUserTeamRegistrationId)
                          .FirstOrDefault();
                        currentUserTeamRegistration.TeamId = result.Entity.Id;
                        currentUserTeamRegistration.IsCaptain = true;
                        _context.Teams.Remove(currentUserTeamRegistration.Team);
                        break;
                    }
                case RegistrationCondition.UserIsRegistered:
                    // Remove user from team and warn user
                    // Test if user is registered on another team: test6 (DONE)
                    // Test if user is registered without a team: test7 (DONE)
                    {
                        var currentUserTeamRegistration = _context.Registrations
                          .Where(r => r.Id == CurrentUserTeamRegistrationId)
                          .FirstOrDefault();
                        currentUserTeamRegistration.TeamId = result.Entity.Id;
                        currentUserTeamRegistration.IsCaptain = true;
                        break;
                    }
                case RegistrationCondition.UserDoesNotHaveRegistration:
                    // Create team and assign user as captain and initiate registration process
                    // Test user not registered: test3 (Can't be reached without registration)
                    IRegistrationService _registrationService = _registrationServiceDelegate(RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature);
                    await _registrationService.CreateAsync(_userManager.GetUserId(User), result.Entity.EventId, result.Entity.Id, true);
                    break;
            }
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
