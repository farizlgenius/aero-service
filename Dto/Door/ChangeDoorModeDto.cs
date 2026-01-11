namespace AeroService.DTO.Acr
{
    public sealed class ChangeDoorModeDto
    {
        public string MacAddress { get; set; }
        public short ComponentId { get; set; }
        public short Mode { get; set; }
    }
}
