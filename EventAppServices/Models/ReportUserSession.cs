using System;

namespace EventAppServices.Models
{
    public class ReportUserSession
    {
        public long SessionID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> IsInterested { get; set; }
    }
}