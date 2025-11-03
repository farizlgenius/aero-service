using HIDAeroService.Model;

namespace HIDAeroService.DTO.MonitorGroup
{
    public sealed class MonitorGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public MonitorGroupDetail Details { get; set; }
    }
}
