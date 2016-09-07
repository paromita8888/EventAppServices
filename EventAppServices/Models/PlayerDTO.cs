using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAppServices.Models
{
    public class PlayerDTO
    {
        public string EmpId { get; set; }
        public string PlayerName { get; set; }

        public string Quiz1Score { get; set; }
        public string   Quiz2Score {get;set;}
        public string Quiz3Score { get; set; }
        public string Quiz4Score { get; set; }
        public string CommulativeScore { get; set; }
        public string LeadershipPosition { get; set; }
        public string Location { get; set; }  
    }
}