using System;
using System.Collections.Generic;
using System.Text;

namespace TeamEventRegistration.Core.Models
{
    public class EventActivity
    {
        public int EventId { get; set; }
        public int ActivityId { get; set; }
        public Event Event { get; set; }
        public Activity Activity { get; set; }
    }
}
