using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Module : BaseDomain
{
    public int DeviceId { get; set; }
    public short DriverId { get; set; }
    public short Model { get; set; }
    public string ModelDetail { get; set; } = string.Empty;
    public string Revision { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public int nHardwareId { get; set; }
    public string nHardwareIdDetail { get; set; } = string.Empty;
    public int nHardwareRev { get; set; }
    public int nProductId { get; set; }
    public int nProductVer { get; set; }
    public short nEncConfig { get; set; }
    public string nEncConfigDetail { get; set; } = string.Empty;
    public short nEncKeyStatus { get; set; }
    public string nEncKeyStatusDetail { get; set; } = string.Empty;
    public List<Reader>? Readers { get; set; }
    public List<Sensor>? Sensors { get; set; }
    public List<Strike>? Strikes { get; set; }
    public List<RequestExit>? RequestExits { get; set; }
    public List<MonitorPoint>? MonitorPoints { get; set; }
    public List<ControlPoint>? ControlPoints { get; set; }
    public short Address { get; set; }
    public string AddressDetail { get; set; } = string.Empty;
    public short Port { get; set; }
    public short nInput { get; set; }
    public short nOutput { get; set; }
    public short nReader { get; set; }
    public short Msp1No { get; set; }
    public short BaudRate { get; set; }
    public short nProtocol { get; set; }
    public short nDialect { get; set; }

    public Module() { }

    public Module(int deviceId, short driverId, short model, string modelDetail, string revision, string serialNumber, int nHardwareId, string nHardwareIdDetail, int nHardwareRev, int nProductId, int nProductVer, short nEncConfig, string nEncConfigDetail, short nEncKeyStatus, string nEncKeyStatusDetail, List<Reader>? readers, List<Sensor>? sensors, List<Strike>? strikes, List<RequestExit>? requestExits, List<MonitorPoint>? monitorPoints, List<ControlPoint>? controlPoints, short address, string addressDetail, short port, short nInput, short nOutput, short nReader, short msp1No, short baudRate, short nProtocol, short nDialect)
    {
        DeviceId = deviceId;
        DriverId = driverId;
        Model = model;
        ModelDetail = ValidateRequiredString(modelDetail, nameof(modelDetail));
        Revision = ValidateRequiredString(revision, nameof(revision));
        SerialNumber = ValidateRequiredString(serialNumber, nameof(serialNumber));
        this.nHardwareId = nHardwareId;
        this.nHardwareIdDetail = ValidateRequiredString(nHardwareIdDetail, nameof(nHardwareIdDetail));
        this.nHardwareRev = nHardwareRev;
        this.nProductId = nProductId;
        this.nProductVer = nProductVer;
        this.nEncConfig = nEncConfig;
        this.nEncConfigDetail = ValidateRequiredString(nEncConfigDetail, nameof(nEncConfigDetail));
        this.nEncKeyStatus = nEncKeyStatus;
        this.nEncKeyStatusDetail = ValidateRequiredString(nEncKeyStatusDetail, nameof(nEncKeyStatusDetail));
        Readers = readers;
        Sensors = sensors;
        Strikes = strikes;
        RequestExits = requestExits;
        MonitorPoints = monitorPoints;
        ControlPoints = controlPoints;
        Address = address;
        AddressDetail = ValidateRequiredString(addressDetail, nameof(addressDetail));
        Port = port;
        this.nInput = nInput;
        this.nOutput = nOutput;
        this.nReader = nReader;
        Msp1No = msp1No;
        BaudRate = baudRate;
        this.nProtocol = nProtocol;
        this.nDialect = nDialect;
    }

    private static string ValidateRequiredString(string value, string field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty);
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
