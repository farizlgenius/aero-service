using HIDAeroService.DTO.Reader;
using HIDAeroService.DTO.Output;
using HIDAeroService.Entity;
using HIDAeroService.DTO.Sensor;
using HIDAeroService.DTO.Strike;
using HIDAeroService.DTO.RequestExit;
using HIDAeroService.DTO.MonitorPoint;
using HIDAeroService.DTO.ControlPoint;

namespace HIDAeroService.DTO.Module
{
    public sealed class ModuleDto : BaseDto
    {
        public string Model { get; set; } = string.Empty;
        public short ModelNo { get; set; }
        // Component 
        public List<ReaderDto>? Readers { get; set; }
        public List<SensorDto>? Sensors { get; set; }
        public List<StrikeDto>? Strikes { get; set; }
        public List<RequestExitDto>? RequestExits { get; set; }
        public List<MonitorPointDto>? MonitorPoints { get; set; }
        public List<ControlPointDto>? ControlPoints { get; set; }
        // End
        public short Address { get; set; }
        public short Port { get; set; }
        public short nInput { get; set; }
        public short nOutput { get; set; }
        public short nReader { get; set; }
        public short Msp1No { get; set; }
        public short BaudRate { get; set; }
        public short nProtocol { get; set; }
        public short nDialect { get; set; }

    }
}
