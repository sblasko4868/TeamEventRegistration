using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Core.Authorization
{
    public class EventOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Event>
    {
        private readonly TeamEventRegistrationDbContext _dbContext;
        private readonly UserManager<Member> _userManager;

        public EventOperationHandler(TeamEventRegistrationDbContext context, UserManager<Member> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext authContext, OperationAuthorizationRequirement requirement, Event resource)
        {
            var user = _userManager.GetUserAsync(authContext.User).Result;

            // Admins have full rights on events (including delete)
            if (_userManager.IsInRoleAsync(user, AuthorizationEnumerations.Roles.EventAdministrator.ToString()).Result ||
                _userManager.IsInRoleAsync(user, AuthorizationEnumerations.Roles.SystemAdministrator.ToString()).Result)
            {
                // If in Event Administrator or System Administrator role
                authContext.Succeed(requirement);
            }

            // Succeed if user is event manager and operation is Create or Update
            var userIsEventManager = _dbContext.EventManagers.Where(em => em.EventId == resource.Id && em.MemberId == user.Id).Any();
            var ElevatedOperations = new List<string>() 
            { 
                AuthorizationOperations.Create.Name,
                AuthorizationOperations.Update.Name
            };
            if (ElevatedOperations.Contains(requirement.Name) && userIsEventManager)
            {
                authContext.Succeed(requirement);
            }

            // Succeed if user is registered for event or is event manager and operation is read
            var userIsRegisteredForEvent = _dbContext.Registrations.Where(r => r.MemberId == user.Id && r.EventId == resource.Id).Any();
            if (requirement.Name == AuthorizationOperations.Read.Name && (userIsRegisteredForEvent || userIsEventManager))
            {
                authContext.Succeed(requirement);
            }

            // Join is not a valid operation at this time
            if (requirement.Name == AuthorizationOperations.Join.Name)
            {
                throw new NotSupportedException($"Operation {AuthorizationOperations.Join.Name} is not supported for object type {resource.GetType().FullName}");
            }

            return Task.CompletedTask;
        }
    }
}
