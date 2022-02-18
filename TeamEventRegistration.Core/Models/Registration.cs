using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace TeamEventRegistration.Core.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MemberId { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        public Member Member { get; set; }
        
        public Event Event { get; set; }       
        
        public int? TeamId { get; set; }
        
        public Team Team { get; set; }
        
        public bool RegistrationComplete { get; set; }
        
        public string RegistrationMessage { get; set; }
        public string RegistrationData { get; set; }
        public string RegistrationExternalId { get; set; }
        
        public bool IsCaptain { get; set; }
    }
}
