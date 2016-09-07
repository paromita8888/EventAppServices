using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class SessionModel
    {
        public string Date { get; set; }

        public List<Slots> Slots { get; set; }

    }
}