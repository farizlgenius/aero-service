namespace AeroService.DTO.License
{
    public sealed class MachineFingerPrintDto
    {
        // CPU + Motherboard + Disk
        public string FingerPrint { get; set; } =string.Empty;
    }
}
