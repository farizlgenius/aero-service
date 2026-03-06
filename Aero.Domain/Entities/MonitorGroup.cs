using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroup : BaseDomain
{
    public int Id {get; set;}
    public short DeviceId { get; set; }
    public short DriverId { get; set; }
      public string Name { get; set; } = string.Empty;
        public short nMpCount { get; set; }
        public List<MonitorGroupList> nMpList { get; set; } = new List<MonitorGroupList>();
    public MonitorGroup()
    {
        
    }

    public MonitorGroup(short deviceId, short driverId, string name, short nMpCount, List<MonitorGroupList> nMpList,int location,bool status) : base(location,status)
    {
        DeviceId = deviceId;
        DriverId = driverId;
        Name = ValidateRequiredString(name, nameof(name));
        this.nMpCount = nMpCount;
        this.nMpList = nMpList ?? throw new ArgumentNullException(nameof(nMpList));
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
