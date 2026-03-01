using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class CommandAudit
{
    public int TagNo { get; set; }
    public int ScpId { get; set; }
    public string? Mac { get; set; } = string.Empty;
    public string? Command { get; set; } = string.Empty;
    public bool IsPending { get; set; }
    public bool IsSuccess { get; set; }
    public string? NakReason { get; set; }
    public int NakDescCode { get; set; }
    public short LoationId { get; set; }

    public CommandAudit() { }

    public CommandAudit(int tagNo, int scpId, string? mac, string? command, bool isPending, bool isSuccess, string? nakReason, int nakDescCode, short loationId)
    {
        TagNo = tagNo;
        ScpId = scpId;
        Mac = ValidateOptionalString(mac, nameof(mac));
        Command = ValidateOptionalString(command, nameof(command));
        IsPending = isPending;
        IsSuccess = isSuccess;
        NakReason = ValidateOptionalString(nakReason, nameof(nakReason));
        NakDescCode = nakDescCode;
        LoationId = loationId;
    }

    private static string? ValidateOptionalString(string? value, string field)
    {
        if (value is null) return null;

        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty).Replace("@", string.Empty);
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
