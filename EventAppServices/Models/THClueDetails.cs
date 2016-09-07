namespace EventAppServices.Models
{
    public class THClueDetails
    {
       // THTeamID,THTeamDescription,  THClueID, THClueDescription, IsValid, LocationDescription,UserName
        public long THTeamID { get; set; }
        public string THTeamDescription { get; set; }
        public long THClueID { get; set; }
        public string THClueDescription { get; set; }
        public bool IsValid { get; set; }
        public string UserName { get; set; }
        public string LocationDescription { get; set; }
        public string Answer { get; set; }
        public string THRiddle { get; set; }

        public string MembersName { get; set; }
    }
}