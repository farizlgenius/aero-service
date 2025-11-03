namespace HIDAeroService.Entity
{
    public sealed class AccessLevelCredential
    {
        public short AccessLevelId { get; set; }
        public short CredentialId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Credential Credential { get; set; }
    }
}
