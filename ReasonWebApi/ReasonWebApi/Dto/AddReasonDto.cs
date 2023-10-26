namespace ReasonWebApi.Dto
{
    public class AddReasonDto
    {
       
        public bool IsPublished { get; set; }
        public bool PrimaryReason { get; set; }
        public string ReasonName { get; set; }
        public int ReasonCode { get; set; }
        public string ReasonType { get; set; }
        public int ThirdPartyNumber { get; set; }
        public string Description { get; set; }
        public string PublishedBy { get; set; }

        public string CreatedBy { get; set; }
    }
}
