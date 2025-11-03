using HIDAeroService.Entity;

namespace HIDAeroService.DTO.Hardware
{
    public sealed class CreateHardwareDto : BaseEntity
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }

    }
}
