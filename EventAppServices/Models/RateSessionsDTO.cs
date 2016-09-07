using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class RateSessionsDTO
    { 
        public long UserId { get; set; }
        public long SessionId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    
    }
}