using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamEventRegistration.Core.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Event Name")]
        public string EventName { get; set; }

        [Display(Name = "Registration Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:M/d/yyyy h:mm tt}")]
        public DateTime RegistrationStart { get; set; }

        [Display(Name = "Registration End Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:M/d/yyyy h:mm tt}")]
        public DateTime RegistrationEnd { get; set; }

        [Display(Name = "Event Date")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:M/d/yyyy h:mm tt}")]
        public DateTime EventDisplayDateTime { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Event Password")]
        public string EventPassword { get; set; }

        public ICollection<Team> Teams { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxTeamMembers { get; set; }

        public ICollection<EventRegistrationRequirement> EventRegistrationRequirements { get; set; }

        public ICollection<Registration> Registrations { get; set; }
        
        [Display(Name = "Embed HTML")]
        [DataType(DataType.MultilineText)]
        public string EmbedHTML { get; set; }

        public ICollection<EventManager> EventManagers { get; set; }
        public ICollection<EventActivity> EventActivities { get; set; }

        [Display(Name = "Display Event Activities", Description = "When set to false, participants will not see the activities that have been added to the event.")]
        public bool DisplayEventActivities { get; set; }
    }
}
