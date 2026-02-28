namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Action : BaseEntity
    {
        public int device_id { get; set; }
        public Device device { get; set; }
        public short action_type { get; set; }
        public string action_type_detail { get; set; } = string.Empty;
        public short arg1 { get; set; } 
        public short arg2 { get; set; }
        public short arg3 { get; set; } 
        public short arg4 { get; set; } 
        public short arg5 { get; set; }
        public short arg6 { get; set; }
        public short arg7 { get; set; }
        public string str_arg {  get; set; } = string.Empty;
        public short delay_time { get; set; }   
        public short procedure_id { get; set; }
        public Procedure procedure { get; set; }

        public Action(short device_id,short action_type, string action_type_detail,short arg1,short arg2,short arg3,short arg4,short arg5,short arg6,short arg7,string str_arg,short delay_time,short procedure_id,int location_id) : base(location_id)
        {
            this.device_id = device_id;
            this.action_type = action_type;
            this.action_type_detail = action_type_detail;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.str_arg = str_arg;
            this.delay_time = delay_time;
            this.procedure_id = procedure_id;

        }

        public Action(Aero.Domain.Entities.Action data) : base(data.LocationId)
        {
            this.device_id = data.DeviceId;
            this.action_type = data.ActionType;
            this.action_type_detail = data.ActionTypeDetail;
            this.arg1 = data.Arg1;
            this.arg2 = data.Arg2;
            this.arg3 = data.Arg3;
            this.arg4 = data.Arg4;
            this.arg5 = data.Arg5;
            this.arg6 = data.Arg6;
            this.arg7 = data.Arg7;
            this.str_arg = data.StrArg;
            this.delay_time = data.DelayTime;
            this.procedure_id = data.ProcedureId;
            this.updated_date = DateTime.UtcNow;
        }

        public void Update(Aero.Domain.Entities.Action data)
        {
            this.device_id = data.DeviceId;
            this.action_type = data.ActionType;
            this.action_type_detail = data.ActionTypeDetail;
            this.arg1 = data.Arg1;
            this.arg2 = data.Arg2;
            this.arg3 = data.Arg3;
            this.arg4 = data.Arg4;
            this.arg5 = data.Arg5;
            this.arg6 = data.Arg6;
            this.arg7 = data.Arg7;
            this.str_arg = data.StrArg;
            this.delay_time = data.DelayTime;
            this.procedure_id = data.ProcedureId;
            this.updated_date = DateTime.UtcNow;
        }

       

    }
}
