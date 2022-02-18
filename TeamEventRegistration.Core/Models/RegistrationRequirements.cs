using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TeamEventRegistration.Core.Models
{
    public class RegistrationRequirement
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
       
        [Required]
        public string Description { get; set; }
    }
}