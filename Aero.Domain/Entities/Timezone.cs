using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class TimeZone : BaseDomain
{
    public int Id { get; private set; }
    public short DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
    public short Mode { get; set; }
    public string ActiveTime { get; set; } = string.Empty;
    public string DeactiveTime { get; set; } = string.Empty;
    public List<Interval> Intervals { get; set; } = new List<Interval>();

    public TimeZone() { }

    public TimeZone(short driverId, string name, short mode, string activeTime, string deactiveTime, List<Interval> intervals, int locationId, bool status) : base(locationId, status)
    {
        DriverId = driverId;
        Name = ValidateRequiredString(name, nameof(name));
        Mode = mode;
        ActiveTime = ValidateRequiredString(activeTime, nameof(activeTime));
        DeactiveTime = ValidateRequiredString(deactiveTime, nameof(deactiveTime));
        Intervals = intervals ?? throw new ArgumentNullException(nameof(intervals));
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
