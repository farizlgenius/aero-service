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

    public Action(short scpid,short actiontype,string actiontypedetail,short arg1,short arg2,short arg3,short arg4,short arg5,short arg6,short arg7,short strarg,short delay)
    {
        this.DeviceId = scpid;
        this.ActionType = actiontype;
    }


    

}
