using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace EventAppServices.Models
{
    public class StallDTO
    {
        public long StallId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long BeaconId { get; set; }
    }
}