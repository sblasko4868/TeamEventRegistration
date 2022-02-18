using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.DTOs
{
    public class RegistrationRequirementMetResponse
    {
        public bool IsMet { get; set; }
        public string Message { get; set; }
    }
}
