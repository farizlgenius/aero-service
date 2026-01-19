namespace Aero.Application.DTOs
{
    public sealed class SetTranDto
    {
        public string MacAddress { get; set; } = string.Empty;
        public short Param { get; set; }
    }
}
