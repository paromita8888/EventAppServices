using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class TagDTO
    {

        public long TagId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long SessionId { get; set; }
        public long SessionCount { get; set;}

       
    }
}