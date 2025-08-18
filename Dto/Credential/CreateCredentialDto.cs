namespace HIDAeroService.Dto.Credential
{
    public sealed class CreateCredentialDto
    {
        public int id {  get; set; }
        public Guid CardHolderReferenceNumber { get; set; }
        public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNumber { get; set; }
        public string? Pin { get; set; }
        public int ActiveDate { get; set; }
        public int DeactiveDate { get; set; }
        public short AccessLevel {get; set;}
        public string? Image { get; set; }
    }
}
