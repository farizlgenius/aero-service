namespace HIDAeroService.Entity
{
    public sealed class TypeWebActivity : BaseTransactionType
    {
        public short iType {  get; set; }
        public short iCurUserId { get; set; }
        public short iObjectUserId { get; set; }
        public string szObjectUser { get; set; } = string.Empty;
        public string ipAddress { get; set; } = string.Empty;
    }
}
