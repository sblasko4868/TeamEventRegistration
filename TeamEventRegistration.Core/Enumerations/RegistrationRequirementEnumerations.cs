using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.Enumerations
{
    public static class RegistrationRequirementEnumerations
    {
        public enum RegistrationRequirementNames
        {
            BeerOlympicsRegistrationSignature
        }

        public enum Statuses
        {
            Initializing,
            InProgress,
            Complete,
            Cancelled
        }
    }
}
