using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class UserSessionDTO
    {
        public long UserID { get; set; }
        public long SessionID { get; set; }
        public Nullable<bool> Isinterested { get; set; }

        public string EmpID { get; set; }
    //    public Nullable<bool> IsRegistered { get; set; }
    }
}