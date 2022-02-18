using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamEventRegistration.Core.DTOs;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Register
{
    public class CheckModel : RegistrationAreaPageModel
    {
        private Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> _registrationServiceDelegate;

        public CheckModel(TeamEventRegistrationDbContext context, 
            UserManager<Member> userManager, 
            IAuthorizationService authorizationService, 
            Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> registrationServiceDelegate)
            : base(context, userManager, authorizationService) 
        {
            _registrationServiceDelegate = registrationServiceDelegate;
        }

        public RegistrationRequirementMetResponse Status { get; set; }

        public async Task OnGet(int id)
        {
            // TODO: Link Event Registration Requirements to registration service delegate
            IRegistrationService _registrationService = _registrationServiceDelegate(RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature);

            Status = await _registrationService.CheckRequirementMet(id);
        }
    }
}
