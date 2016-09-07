using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class UserAttendanceDTO
    { 
            public string EmpID { get; set; }
            public long  SessionID { get; set; } 
            public long  StallID { get; set; }
            public bool IsAttendee { get; set; } 
            public long UserID { get; set; }
            public string SessionName { get; set; } 
            public string StallName { get; set; }
    }
}