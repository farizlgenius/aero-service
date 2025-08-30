namespace HIDAeroService.Entity
{
    public sealed class ArReader : ArBaseEntity
    {
        public string Name { get; set; }
        public string ScpMac { get; set; }
        public short SioNo { get; set; }
        public short ReaderNo { get; set; }
        public short LedDriveMode { get; set; }
        public short OsdpFlag { get; set; }
    }
}
