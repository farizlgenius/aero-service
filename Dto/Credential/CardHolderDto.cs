namespace HIDAeroService.Dto.Credential
{
    public sealed class CardHolderDto
    {
        public string CardHolderId { get; set; }
        public Guid CardHolderReferenceNumber { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? HolderStatus { get; set; }
        public int IssueCodeRunningNumber { get; set; }
    }
}
