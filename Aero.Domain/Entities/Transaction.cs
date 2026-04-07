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
        Actor = actor;
        Source = source;
        SourceDesc = sourceDesc;
        Origin = origin;
        SourceModule = sourceModule;
        Type = type;
        TypeDesc = typeDesc;
        TranCode = tranCode;
        Image = image;
        TranCodeDesc = tranCodeDesc;
        ExtendDesc = extendDesc;
        Remark = remark;
        TransactionFlags = transactionFlags;
        Mac = mac;
        HardwareName = hardwareName;
    }


}
