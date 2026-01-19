namespace Aero.Application.DTOs
{
    public sealed class TranStatusDto
    {
        public string MacAddress { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int Oldest { get; set; }
        public int LastReport { get; set; }
        public int LastLog { get; set; }
        public int Disabled { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
