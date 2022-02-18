using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamEventRegistration.Core.Models
{
    public class EventManager
    {
        public string MemberId { get; set; }
        public int EventId { get; set; }
        public Member Member { get; set; }
        public Event Event { get; set; }
    }
}
