using HIDAeroService.Entity;

namespace HIDAeroService.DTO.Hardware
{
    public sealed class CreateHardwareDto : BaseDto
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }

    }
}
