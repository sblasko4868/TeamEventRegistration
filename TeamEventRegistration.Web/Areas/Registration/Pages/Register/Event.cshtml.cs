using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TeamEventRegistration.Core.Enumerations;
using TeamEventRegistration.Core.Interfaces;
using TeamEventRegistration.Core.Models;
using TeamEventRegistration.Core.Services;

namespace TeamEventRegistration.Web.Areas.Registration.Pages.Register
{
    public class EventModel : RegistrationAreaPageModel
    {
        private Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> _registrationServiceDelegate;

        public EventModel(TeamEventRegistrationDbContext context, 
            UserManager<Member> userManager, 
            IAuthorizationService authorizationService, 
            Func<RegistrationRequirementEnumerations.RegistrationRequirementNames, IRegistrationService> registrationServiceDelegate)
            : base(context, userManager, authorizationService)
        {
            _registrationServiceDelegate = registrationServiceDelegate;
        }
        public Core.Models.Event Event { get; set; }
        public Member CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if already registered
            if (id == null)
            {
                return NotFound();
            }

            CurrentUser = await _userManager.GetUserAsync(User);

            Event = _context.Events
                .Include(e => e.Registrations)
                .Include(e => e.EventRegistrationRequirements)
                    .ThenInclude(err => err.RegistrationRequirement)
                .Where(e => e.Id == id).FirstOrDefault();

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = _context.Events.Where(e => e.Id == id).FirstOrDefault();
            if (Event == null)
            {
                return NotFound();
            }

            CurrentUser = await _userManager.GetUserAsync(User);

            // TODO: Link Event Registration Requirements to registration service delegate
            IRegistrationService _registrationService = _registrationServiceDelegate(RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature);
            await _registrationService.CreateAsync(CurrentUser.Id, Event.Id);

            return RedirectToPage("/Index");
        }
    }
}
