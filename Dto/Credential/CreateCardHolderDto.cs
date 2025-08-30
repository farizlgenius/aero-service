namespace HIDAeroService.Dto.Credential
{
    public sealed class CreateCardHolderDto
    {
        public string CardHolderId { get; set; }
        public string CardHolderReferenceNumber { get; set; } = string.Empty;
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public List<CreateCredentialDto> Cards { get; set; }
    }
}
