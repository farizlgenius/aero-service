namespace HIDAeroService.Dto.AccessLevel
{
    public sealed class CreateAccessLevelDoor
    {
        public string ScpMac { get; set; }
        public short AcrNo { get; set; }
        public string AcrName { get; set; }
        public short TzNo { get; set; }
        public string TzName { get; set; }
    }
}
