namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class Action : BaseEntity
    {
        public short scp_id { get; set; }
        public short action_type { get; set; }
        public string action_type_desc { get; set; } = string.Empty;
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
    }
}
