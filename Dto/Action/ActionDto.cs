namespace HIDAeroService.DTO.Action
{
    public sealed class ActionDto : BaseDto
    {
        public short ScpId { get; set; }
        public short ActionType { get; set; }
        public string ActionTypeDesc { get; set; } = string.Empty;
        public short Arg1 { get; set; } 
        public short Arg2 { get; set; } 
        public short Arg3 { get; set; } 
        public short Arg4 { get; set; } 
        public short Arg5 { get; set; }
        public short Arg6 { get; set; }
        public short Arg7 { get; set; }
        public string StrArg { get; set; } = string.Empty;
        public short DelayTime { get; set; }
    }
}
