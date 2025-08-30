namespace HIDAeroService.Dto.Credential
{
    public sealed class CardHolderDto
    {
        public int No { get; set; }
        public string CardHolderId { get; set; }
        public string CardHolderReferenceNumber { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? HolderStatus { get; set; }
        public int IssueCodeRunningNumber { get; set; }
    }
}
