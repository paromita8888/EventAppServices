using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class OutputResult
    {
        public string IsValid { get; set; }
        public string Message { get; set; }
    }


    public class StreamResult
    {
        public string url { get; set; }
        public string startsAt { get; set; }
    }
}