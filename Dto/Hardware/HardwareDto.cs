using HIDAeroService.DTO.Module;

namespace HIDAeroService.DTO.Scp
{
    public sealed class HardwareDto : BaseDto
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string IpAddress { get; set; }
        public List<ModuleDto> Modules { get; set; }
        public string SerialNumber { get; set; }
        public bool IsUpload { get; set; } = false;
        public bool IsReset { get; set; } = false;
        public DateTime LastSync { get; set; }
    }
}
