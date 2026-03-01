using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Transaction : BaseDomain
{
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public int SerialNumber { get; set; }
    public string Actor { get; set; } = string.Empty;
    public double Source { get; set; }
    public string SourceDesc { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string SourceModule { get; set; } = string.Empty;
    public double Type { get; set; }
    public string TypeDesc { get; set; } = string.Empty;
    public double TranCode { get; set; }
    public string Image { get; set; } = string.Empty;
    public string TranCodeDesc { get; set; } = string.Empty;
    public string ExtendDesc { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public List<TransactionFlag> TransactionFlags { get; set; } = new List<TransactionFlag>();
    public string Mac { get; set; } = string.Empty;
    public string HardwareName { get; set; } = string.Empty;

    public Transaction() { }

    public Transaction(DateTime dateTime, int serialNumber, string actor, double source, string sourceDesc, string origin, string sourceModule, double type, string typeDesc, double tranCode, string image, string tranCodeDesc, string extendDesc, string remark, List<TransactionFlag> transactionFlags, string mac, string hardwareName)
    {
        DateTime = dateTime;
        SerialNumber = serialNumber;
        Actor = ValidateRequiredString(actor, nameof(actor));
        Source = source;
        SourceDesc = ValidateRequiredString(sourceDesc, nameof(sourceDesc));
        Origin = ValidateRequiredString(origin, nameof(origin));
        SourceModule = ValidateRequiredString(sourceModule, nameof(sourceModule));
        Type = type;
        TypeDesc = ValidateRequiredString(typeDesc, nameof(typeDesc));
        TranCode = tranCode;
        Image = ValidateRequiredString(image, nameof(image));
        TranCodeDesc = ValidateRequiredString(tranCodeDesc, nameof(tranCodeDesc));
        ExtendDesc = ValidateRequiredString(extendDesc, nameof(extendDesc));
        Remark = ValidateRequiredString(remark, nameof(remark));
        TransactionFlags = transactionFlags ?? throw new ArgumentNullException(nameof(transactionFlags));
        Mac = ValidateRequiredString(mac, nameof(mac));
        HardwareName = ValidateRequiredString(hardwareName, nameof(hardwareName));
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
