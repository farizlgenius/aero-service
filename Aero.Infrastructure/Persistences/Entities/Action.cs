namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Action : BaseEntity
    {
        public short scp_id { get; set; }
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

        public Action(short scp_id,short action_type, string action_type_detail,short arg1,short arg2,short arg3,short arg4,short arg5,short arg6,short arg7,string str_arg,short delay_time,short procedure_id,int location_id) : base(location_id)
        {
            this.scp_id = scp_id;
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

        public void Update()
        {

        }

    }
}
