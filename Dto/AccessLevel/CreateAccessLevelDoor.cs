namespace HIDAeroService.Dto.AccessLevel
{
    public sealed class CreateAccessLevelDoor
    {
        public string ScpIp { get; set; }
        public short AcrNumber { get; set; }
        public string AcrName { get; set; }
        public short TzNumber { get; set; }
        public string TzName { get; set; }
    }
}
