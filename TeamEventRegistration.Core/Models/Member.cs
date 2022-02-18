using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TeamEventRegistration.Core.Models
{
    public class Member : IdentityUser
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public override string Email { get; set; }

        [PersonalData]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        [PersonalData]
        [Display(Name = "Nick Name")]
        public string NickName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        public bool SelfCreation { get; set; }
        public ICollection<Registration> Registrations { get; set; }      
        
        public ICollection<EventManager> ManagedEvents { get; set; }
    }
}
