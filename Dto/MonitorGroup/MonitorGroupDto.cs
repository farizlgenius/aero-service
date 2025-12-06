using HIDAeroService.Model;

namespace HIDAeroService.DTO.MonitorGroup
{
    public sealed class MonitorGroupDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupListDto> nMpList { get; set; }
    }
}
