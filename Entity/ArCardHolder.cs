namespace HIDAeroService.Entity
{
    public sealed class ArCardHolder : ArBaseEntity
    {
        public string CardHolderId { get; set; }
        public string CardHolderRefNo { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? HolderStatus { get; set; }
        public int IssueCodeRunningNo { get; set; }
    }
}
