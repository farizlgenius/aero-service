using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroup : BaseDomain
{
    public int DeviceId { get; set; }
    public short DriverId { get; set; }
      public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupList> nMpList { get; set; } = new List<MonitorGroupList>();
    public string Mac { get; set; } = string.Empty;
    public MonitorGroup()
    {
        
    }

    public MonitorGroup(int deviceId, short driverId, string name, short nMpCount, List<MonitorGroupList> nMpList, string mac)
    {
        DeviceId = deviceId;
        DriverId = driverId;
        Name = ValidateRequiredString(name, nameof(name));
        this.nMpCount = nMpCount;
        this.nMpList = nMpList ?? throw new ArgumentNullException(nameof(nMpList));
        Mac = ValidateRequiredString(mac, nameof(mac));
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
