using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TeamEventRegistration.Core.Models
{
    public class EventRegistrationRequirement
    {
        public int RegistrationRequirementId { get; set; }
        public int EventId { get; set; }
        public RegistrationRequirement RegistrationRequirement { get; set; }
        public Event Event { get; set; }
    }
}
