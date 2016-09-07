using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class Slots
    {
        public string Time { get; set; }
        public List<EventSlot> Events {get;set;}
    }
}