using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class ValidateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string EmpId { get; set; }
        public string EmailId{ get; set; }
        public Boolean ClaimGoody { get; set; }
    }

    public class ValidateUserDTOLogin
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ReferralEmail { get; set; }        
    }
}