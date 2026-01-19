namespace Aero.Application.DTOs
{
    public sealed class TransactionCodeDto
    {
        public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
