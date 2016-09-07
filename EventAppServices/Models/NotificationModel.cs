using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime? sentAt { get; set; }
    }

}