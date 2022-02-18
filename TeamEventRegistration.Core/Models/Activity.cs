using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamEventRegistration.Core.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string ActivityName { get; set; }
        
        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }
        
        [Display(Name = "Rules")]
        public string ActivityRules { get; set; }

        public ICollection<EventActivity> EventActivities { get; set; }
    }
}
