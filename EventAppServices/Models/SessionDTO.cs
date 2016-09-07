using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class SessionDTO
    {
        public long SessionId { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; } 
        public string EndTime { get; set; }
        public string Venue { get; set; }
        public Nullable<long> SpeakerId { get; set; } 
        public String Title { get; set; }

        public List<String> PillarTag { get; set; }
    }
}