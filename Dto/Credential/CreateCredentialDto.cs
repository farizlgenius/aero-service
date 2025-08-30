namespace HIDAeroService.Dto.Credential
{
    public sealed class CreateCredentialDto
    {
        public int Id {  get; set; }
        public string CardHolderReferenceNumber { get; set; }
        public int Bits { get; set; }
        public int IssueCode { get; set; }
        public int FacilityCode { get; set; }
        public long CardNumber { get; set; }
        public string? Pin { get; set; }
        public string ActiveDate { get; set; }
        public string DeactiveDate { get; set; }
        public short AccessLevel {get; set;}
        public string? Image { get; set; }
    }
}
