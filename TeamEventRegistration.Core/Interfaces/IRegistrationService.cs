using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamEventRegistration.Core.DTOs;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Core.Interfaces
{
    public interface IRegistrationService
    {
        public abstract Task<RegistrationRequirementMetResponse> CheckRequirementMet(int registrationId);
        public abstract Task InvokeRequirementAsync(int registrationId);
        public abstract Task CreateAsync(string memberId, int eventId);
        public abstract Task CreateAsync(string memberId, int eventId, int teamId, bool setCaptain);
    }
}
