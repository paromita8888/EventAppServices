using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppServices.Models
{
   public  class SessionInterest
    {
        public long SessionId { get; set; }
        public string EmpId { get; set; }
        public Boolean Isinterested { get; set; }

        public Boolean IsRegistered { get; set; }
    }
}
