namespace AeroService.DTO.Hardware
{
    public sealed class SetTranDto
    {
        public string MacAddress { get; set; } = string.Empty;
        public short Param { get; set; }
    }
}
