namespace ReasonWebApi.Models
{
    public class Reason
    {

        public int ReasonId { get; set; }
        public bool IsPublished { get; set; }
        public bool PrimaryReason { get; set; }
        public string ReasonName { get; set; }
        public int ReasonCode { get; set; }
        public string ReasonType { get; set; }
        public int ThirdPartyNumber { get; set; }
        public string Description { get; set; }
        public string PublishedBy { get; set; }
        public DateTime DatePublished { get; set; }
        public bool DisplayOnWeb { get; set; }
        public int SortOrder { get; set; }
        public string Tag { get; set; }
        public string Comments { get; set; }
        public string IPAddress { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }
    }
}
