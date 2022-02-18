using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace TeamEventRegistration.Core.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
        [Display(Name = "Team Members")]
        public ICollection<Registration> Registrations { get; set; }
    }
}
