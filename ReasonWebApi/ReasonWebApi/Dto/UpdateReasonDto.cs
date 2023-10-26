namespace ReasonWebApi.Dto
{
    public class UpdateReasonDto
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

        public string UpdatedBy { get; set; }
    }
}
