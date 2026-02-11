using System;

namespace Aero.Domain.Entities;

public sealed class Module : BaseEntity
{
    public short SioId { get; set; }
      public short Model { get; set; }
      public string ModelDescription { get; set; } = string.Empty;
      public string Revision { get; set; } = string.Empty;
      public string SerialNumber { get; set; } = string.Empty;
      public int nHardwareId { get; set; }
      public string nHardwareIdDescription { get; set; } = string.Empty;
      public int nHardwareRev { get; set; }
      public int nProductId { get; set; }
      public int nProductVer { get; set; }
      public short nEncConfig { get; set; }
      public string nEncConfigDescription { get; set; } = string.Empty;
      public short nEncKeyStatus { get; set; }
      public string nEncKeyStatusDescription { get; set; } = string.Empty;
      // HardwareComponent 
      public List<Reader>? Readers { get; set; }
      public List<Sensor>? Sensors { get; set; }
      public List<Strike>? Strikes { get; set; }
      public List<RequestExit>? RequestExits { get; set; }
      public List<MonitorPoint>? MonitorPoints { get; set; }
      public List<ControlPoint>? ControlPoints { get; set; }
      // End
      public short Address { get; set; }
      public string AddressDescription { get; set; } = string.Empty;
      public short Port { get; set; }
      public short nInput { get; set; }
      public short nOutput { get; set; }
      public short nReader { get; set; }
      public short Msp1No { get; set; }
      public short BaudRate { get; set; }
      public short nProtocol { get; set; }
      public short nDialect { get; set; }
}
