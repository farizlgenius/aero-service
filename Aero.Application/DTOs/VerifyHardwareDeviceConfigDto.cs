namespace Aero.Application.DTOs
{
    public sealed class VerifyHardwareDeviceConfigDto 
    {
        public string ComponentName { get; set; } = string.Empty;
        public int nMismatchRecord { get; set; }
        public bool IsUpload { get; set; }
    }
}
