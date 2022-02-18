using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Authorization;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Register
{
    public class TeamModel : RegistrationAreaPageModel
    {
        public TeamModel(TeamEventRegistrationDbContext context,
            UserManager<Member> userManager,
            IAuthorizationService authorizationService)
            : base(context, userManager, authorizationService) { }

        public Core.Models.Registration Registration { get; set; }
        public List<Team> Teams { get; set; }

        [BindProperty]
        public int? SelectedTeam { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int RegistrationId)
        {
            InitializeModel(RegistrationId);

            if (Registration == null)
            {
                return NotFound();
            }

            if (_authorizationService.AuthorizeAsync(User, Registration.Team, AuthorizationOperations.Join).Result.Succeeded
                || Registration.TeamId == null)
            {
                return Page();
            }
            else
            {
                return Forbid();
            }
        }

        public async Task<IActionResult> OnPostAsync(int RegistrationId)
        {
            if (SelectedTeam == null)
            {
                ModelState.AddModelError(string.Empty, "Team must be selected");
            }

            if (!ModelState.IsValid)
            {
                InitializeModel(RegistrationId);
                return Page();
            }

            var registrationToUpdate = _context.Registrations.FirstOrDefault(r => r.Id == RegistrationId);
            var selectedTeamObject = _context.Teams.Include(t => t.Registrations).FirstOrDefault(t => t.Id == SelectedTeam);
            if (registrationToUpdate == null || selectedTeamObject == null)
            {
                return NotFound();
            }

            if (_authorizationService.AuthorizeAsync(User, selectedTeamObject, AuthorizationOperations.Join).Result.Succeeded)
            {
                // Make captain if no other members of team
                registrationToUpdate.IsCaptain = !selectedTeamObject.Registrations.Any();
                registrationToUpdate.TeamId = SelectedTeam;
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            else
            {
                return Forbid();
            }
            
        }

        public void InitializeModel(int RegistrationId)
        {
            Registration = _context.Registrations.FirstOrDefault(r => r.Id == RegistrationId && r.MemberId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (Registration.TeamId != null)
            {
                SelectedTeam = Registration.TeamId;
            } 
            else
            {
                SelectedTeam = null;
            }
            Teams = _context.Teams
                .Include(t => t.Registrations)
                    .ThenInclude(r => r.Member)
                .Where(t => t.EventId == Registration.EventId)
                .ToList();
        }
    }
}
