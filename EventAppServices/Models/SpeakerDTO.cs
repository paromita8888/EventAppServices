using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class SpeakerDTO
    {
        public long SpeakerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Intro { get; set; }
        public string Description { get; set; }
        //public Nullable<long> CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }
        //public Nullable<long> UpdatedBy { get; set; }
        //public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Image { get; set; } 

       
    }
}