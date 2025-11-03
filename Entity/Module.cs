using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class Module : BaseEntity
    {
        public string Model { get; set; } = string.Empty;
        public Hardware Hardware { get; set; }
        // Component 
        public ICollection<Reader>? Readers { get; set; }
        public ICollection<Sensor>? Sensors { get; set; }
        public ICollection<Strike>? Strikes { get; set; }
        public ICollection<RequestExit>? RequestExits { get; set; }
        public ICollection<MonitorPoint>? MonitorPoints { get; set; }
        public ICollection<ControlPoint>? ControlPoints { get; set; }
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
