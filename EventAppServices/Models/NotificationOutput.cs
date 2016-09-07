using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class NotificationOutput
    {
        public string title { get; set; }
        public string content { get; set; }
        public string sentAt { get; set; }
    }
}