namespace HIDAeroService.Entity
{
    public sealed class TypeUserCmnd : BaseTransactionType
    {
        public short nKeys {  get; set; }
        public string keys { get; set; } = string.Empty;
    }
}
