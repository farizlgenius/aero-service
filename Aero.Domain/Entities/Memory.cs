using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Memory
{
    public short nStrType { get; set; }
    public string StrType { get; set; } = string.Empty;
    public int nRecord { get; set; }
    public int nRecSize { get; set; }
    public int nActive { get; set; }
    public int nSwAlloc { get; set; }
    public int nSwRecord { get; set; }
    public bool IsSync { get; set; }

    public Memory() { }

    public Memory(short nStrType, string strType, int nRecord, int nRecSize, int nActive, int nSwAlloc, int nSwRecord, bool isSync)
    {
        this.nStrType = nStrType;
        StrType = ValidateRequiredString(strType, nameof(strType));
        this.nRecord = nRecord;
        this.nRecSize = nRecSize;
        this.nActive = nActive;
        this.nSwAlloc = nSwAlloc;
        this.nSwRecord = nSwRecord;
        IsSync = isSync;
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
