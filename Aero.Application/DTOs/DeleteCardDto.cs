namespace Aero.Application.DTOs
{
    public sealed class DeleteCardDto
    {
        public string Mac {  get; set; } = string.Empty;
        public long CardNo { get; set; }
    }
}
