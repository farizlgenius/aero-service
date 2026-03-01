using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Action : BaseDomain
{
    public short DeviceId { get; set; }
    public short ActionType { get; set; }
    public string ActionTypeDetail { get; set; } = string.Empty;
    public short Arg1 { get; set; }
    public short Arg2 { get; set; }
    public short Arg3 { get; set; }
    public short Arg4 { get; set; }
    public short Arg5 { get; set; }
    public short Arg6 { get; set; }
    public short Arg7 { get; set; }
    public string StrArg { get; set; } = string.Empty;
    public short DelayTime { get; set; }
    public short ProcedureId { get; set; }

    public Action() { }

    public Action(short deviceId, short actionType, string actionTypeDetail, short arg1, short arg2, short arg3, short arg4, short arg5, short arg6, short arg7, string strArg, short delayTime, short procedureId)
    {
        DeviceId = deviceId;
        ActionType = actionType;
        ActionTypeDetail = ValidateRequiredString(actionTypeDetail, nameof(actionTypeDetail));
        Arg1 = arg1;
        Arg2 = arg2;
        Arg3 = arg3;
        Arg4 = arg4;
        Arg5 = arg5;
        Arg6 = arg6;
        Arg7 = arg7;
        StrArg = ValidateRequiredString(strArg, nameof(strArg));
        DelayTime = delayTime;
        ProcedureId = procedureId;
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
