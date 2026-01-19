namespace Aero.Application.DTOs
{
    public sealed class MemoryAllocateDto
    {
        public short nStrType { get; set; }
        public string StrType { get; set; } = string.Empty;
        public int nRecord { get; set; }
        public int nRecSize { get; set; }
        public int nActive { get; set; }
        public int nSwAlloc { get; set; }
        public int nSwRecord { get; set; }
        public bool IsSync { get; set; }
    }
}
