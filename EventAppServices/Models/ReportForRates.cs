using System;

namespace EventAppServices.Models
{
    public class ReportForRates
    {
        public long UserId { get; set; }
        public long SessionId { get; set; }
        public int Rating { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}