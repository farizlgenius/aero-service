using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Device : BaseDomain
{
    public short DriverId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int HardwareType { get; private set; }
    public string HardwareTypeDetail { get; private set; } = string.Empty;
    public string Mac { get; private set; } = string.Empty;
    public string Ip { get; private set; } = string.Empty;
    public string Firmware { get; private set; } = string.Empty;
    public string Port { get; private set; } = string.Empty;
    public List<Module> Modules { get; private set; } = new List<Module>();
    public string SerialNumber { get; private set; } = string.Empty;
    public bool IsUpload { get; private set; }
    public bool IsReset { get; private set; }
    public bool PortOne { get; private set; }
    public short ProtocolOne { get; private set; }
    public string ProtocolOneDetail { get; private set; } = string.Empty;
    public short BaudRateOne { get; private set; }
    public bool PortTwo { get; private set; }
    public short ProtocolTwo { get; private set; }
    public string ProtocolTwoDetail { get; private set; } = string.Empty;
    public short BaudRateTwo { get; private set; }
    public DateTime LastSync { get; private set; }

    public Device() { }

    public Device(
        short driver,
        string name,
        int type,
        string hwtypedetail,
        string mac,
        string ip,
        string firmware,
        string port,
        List<Module> modules,
        string serialnumber,
        bool isUpload,
        bool isReset,
        bool portone,
        short protocolone,
        string protocolonedetail,
        short baudrateone,
        bool porttwo,
        short protocoltwo,
        string protocoltwodetail,
        short baudratetwo,
        DateTime lastsync)
    {
        SetDriverId(driver);
        SetName(name);
        SetHardwareType(type);
        SetHardwareTypeDetail(hwtypedetail);
        SetMac(mac);
        SetIp(ip);
        SetFirmware(firmware);
        SetPort(port);
        SetModules(modules);
        SetSerialNumber(serialnumber);
        SetIsUpload(isUpload);
        SetIsReset(isReset);
        SetPortOne(portone);
        SetProtocolOne(protocolone);
        SetProtocolOneDetail(protocolonedetail);
        SetBaudRateOne(baudrateone);
        SetPortTwo(porttwo);
        SetProtocolTwo(protocoltwo);
        SetProtocolTwoDetail(protocoltwodetail);
        SetBaudRateTwo(baudratetwo);
        SetLastSync(lastsync);
    }

    private void SetDriverId(short driverId)
    {
        if (driverId <= 0) throw new ArgumentException("Driver Id invalid.");
        DriverId = driverId;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name.Trim())) throw new ArgumentException("Name invalid.");
        Name = name;
    }

    private void SetHardwareType(int hardwareType)
    {
        if (hardwareType <= 0) throw new ArgumentException("Hardware type invalid.");
        HardwareType = hardwareType;
    }

    private void SetHardwareTypeDetail(string hardwareTypeDetail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hardwareTypeDetail);
        if (!RegexHelper.IsValidName(hardwareTypeDetail.Trim())) throw new ArgumentException("Hardware type detail invalid.");
        HardwareTypeDetail = hardwareTypeDetail;
    }

    private void SetMac(string mac)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mac);
        var sanitized = mac.Replace(":", string.Empty).Replace("-", string.Empty).Trim();
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Mac invalid.");
        Mac = mac;
    }

    private void SetIp(string ip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ip);
        var sanitized = ip.Replace(".", string.Empty).Replace(":", string.Empty).Trim();
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Ip invalid.");
        Ip = ip;
    }

    public void SetFirmware(string firmware)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firmware);
        var sanitized = firmware.Replace(".", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty).Trim();
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Firmware invalid.");
        Firmware = firmware;
    }

    private void SetPort(string port)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(port);
        var sanitized = port.Replace("/", string.Empty).Trim();
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Port invalid.");
        Port = port;
    }

    private void SetModules(List<Module> modules)
    {
        Modules = modules ?? throw new ArgumentNullException(nameof(modules));
    }

    private void SetSerialNumber(string serialNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        var sanitized = serialNumber.Replace("-", string.Empty).Replace("_", string.Empty).Trim();
        if (!RegexHelper.IsValidOnlyCharAndDigit(sanitized)) throw new ArgumentException("Serial number invalid.");
        SerialNumber = serialNumber;
    }

    public void SetIsUpload(bool isUpload) => IsUpload = isUpload;

    public void SetIsReset(bool isReset) => IsReset = isReset;

    private void SetPortOne(bool portOne) => PortOne = portOne;

    private void SetProtocolOne(short protocolOne)
    {
        if (protocolOne < 0) throw new ArgumentException("Protocol one invalid.");
        ProtocolOne = protocolOne;
    }

    private void SetProtocolOneDetail(string protocolOneDetail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(protocolOneDetail);
        if (!RegexHelper.IsValidName(protocolOneDetail.Trim())) throw new ArgumentException("Protocol one detail invalid.");
        ProtocolOneDetail = protocolOneDetail;
    }

    private void SetBaudRateOne(short baudRateOne)
    {
        if (baudRateOne < 0) throw new ArgumentException("Baud rate one invalid.");
        BaudRateOne = baudRateOne;
    }

    private void SetPortTwo(bool portTwo) => PortTwo = portTwo;

    private void SetProtocolTwo(short protocolTwo)
    {
        if (protocolTwo < 0) throw new ArgumentException("Protocol two invalid.");
        ProtocolTwo = protocolTwo;
    }

    private void SetProtocolTwoDetail(string protocolTwoDetail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(protocolTwoDetail);
        if (!RegexHelper.IsValidName(protocolTwoDetail.Trim())) throw new ArgumentException("Protocol two detail invalid.");
        ProtocolTwoDetail = protocolTwoDetail;
    }

    private void SetBaudRateTwo(short baudRateTwo)
    {
        if (baudRateTwo < 0) throw new ArgumentException("Baud rate two invalid.");
        BaudRateTwo = baudRateTwo;
    }

    private void SetLastSync(DateTime lastSync)
    {
        if (lastSync == default) throw new ArgumentException("Last sync invalid.");
        LastSync = lastSync;
    }
}
