namespace HIDAeroService.Models
{
    public sealed class IDReport
    {
        public short DeviceID { get; set; }
        public short DeviceVer { get; set; }
        public short SoftwareRevMajor { get; set; }
        public short SoftwareRevMinor { get; set; }
        public int SerialNumber { get; set; }
        public int RamSize { get; set; }
        public int RamFree { get; set; }
        public DateTimeOffset ESec { get; set; }
        public int DatabaseMax { get; set; }
        public int DatabaseActive { get; set; }
        public byte DipSwitchPowerUp { get; set; }
        public byte DipSwitchCurrent { get; set; }
        public short ScpID { get; set; }
        public short FirmWareAdvisory { get; set; }
        public short ScpIn1 { get; set; }
        public short ScpIn2 { get; set; }
        public short NOemCode { get; set; }
        public byte ConfigFlag { get; set; }
        public string MacAddress { get; set; }
        public byte TlsStatus { get; set; }
        public byte OperMode { get; set; }
        public short ScpIn3 { get; set; }
        public int CumulativeBldCnt { get; set; }
        public int Ip { get; set; }
        public int Port { get; set; }

    }
}
