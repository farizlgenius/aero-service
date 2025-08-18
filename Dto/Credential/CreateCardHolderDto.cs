namespace HIDAeroService.Dto.Credential
{
    public sealed class CreateCardHolderDto
    {
        public string CardHolderId { get; set; }
        public Guid CardHolderReferenceNumber { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public List<CreateCredentialDto> Cards { get; set; }
    }
}
