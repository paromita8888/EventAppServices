namespace EventAppServices.Models
{
    public class THClueDetailsSubmitDTO
    {
       // THTeamID,THTeamDescription,  THClueID, THClueDescription, IsValid, LocationDescription,UserName
        public long THTeamID { get; set; }
        public long THClueCategoryID { get; set; }
        public long THClueID { get; set; }
        public string Answer { get; set; }
    }
}