using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class UserAttendanceSessionDTO
    {  
            public long SessionID { get; set; }  
            public string SessionName { get; set; } 
            public int Count { get; set; }
    }
}