using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class EventSlot
    {
        public long Id { get; set; }
        public string EndTime { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public Nullable<long> SpeakerId { get; set; } 
        public List<string > Tags { get; set; }
       

    }
}