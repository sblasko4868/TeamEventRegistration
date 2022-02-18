using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Core.Authorization
{
    public class TeamOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Team>
    {
        private readonly TeamEventRegistrationDbContext _dbContext;
        private readonly UserManager<Member> _userManager;

        public TeamOperationHandler(TeamEventRegistrationDbContext context, UserManager<Member> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext authContext, 
            OperationAuthorizationRequirement requirement, 
            Team resource)
        {
            var user = _userManager.GetUserAsync(authContext.User).Result;
            
            var team = _dbContext.Teams
                .Include(t => t.Event)
                    .ThenInclude(e => e.EventManagers)
                .FirstOrDefault(t => t.Id == resource.Id);
            var userEventRegistrations = _dbContext.Registrations.Where(r => r.MemberId == user.Id && team.EventId == r.EventId);

            if (_userManager.IsInRoleAsync(user, AuthorizationEnumerations.Roles.EventAdministrator.ToString()).Result || 
                _userManager.IsInRoleAsync(user, AuthorizationEnumerations.Roles.SystemAdministrator.ToString()).Result)
            {
                // If in Event Administrator or System Administrator role
                authContext.Succeed(requirement);
            }

            if (team.Event.EventManagers.Where(em => em.MemberId == user.Id).Any())
            {
                // If the user is an event manager for the team's event
                authContext.Succeed(requirement);
            }

            if (requirement.Name == AuthorizationEnumerations.Permissions.Create.ToString())
            {
                // Success if user registered for same event as team
                if (userEventRegistrations.Any())
                    authContext.Succeed(requirement);
            }
            if (requirement.Name == AuthorizationEnumerations.Permissions.Read.ToString())
            {
                // Success if user registered for same event as team
                if (userEventRegistrations.Any())
                    authContext.Succeed(requirement);
            }
            if (requirement.Name == AuthorizationEnumerations.Permissions.Join.ToString())
            {
                // Success if user reigstered on team
                if (userEventRegistrations.Where(r => r.EventId == team.EventId).Any())
                    authContext.Succeed(requirement);
            }
            if (requirement.Name == AuthorizationEnumerations.Permissions.Update.ToString())
            {
                // Success if user is captain of team or only member of team
                if (userEventRegistrations.Where(r => r.TeamId == team.Id && r.IsCaptain).Any() ||
                        userEventRegistrations.Where(r => r.TeamId == team.Id).Count() == 1)
                    authContext.Succeed(requirement);
            }
            if (requirement.Name == AuthorizationEnumerations.Permissions.Delete.ToString())
            {
                // Success if user is the only member of the team
                if (userEventRegistrations.Where(r => r.TeamId == team.Id).Count() == 1)
                    authContext.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
