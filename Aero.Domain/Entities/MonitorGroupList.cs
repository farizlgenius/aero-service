using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class MonitorGroupList
{
    public int MonitorGroupId { get; set; } 
      public short PointType { get; set; }
        public string PointTypeDetail { get; set; } = string.Empty;
        public short PointNumber { get; set; }

    public MonitorGroupList() { }

    public MonitorGroupList(int monitorGroupId, short pointType, string pointTypeDetail, short pointNumber)
    {
        MonitorGroupId = monitorGroupId;
        PointType = pointType;
        PointTypeDetail = ValidateRequiredString(pointTypeDetail, nameof(pointTypeDetail));
        PointNumber = pointNumber;
    }

    private static string ValidateRequiredString(string value, string field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(trimmed.Replace("-", string.Empty).Replace("_", string.Empty)))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
