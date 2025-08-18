namespace HIDAeroService.Dto
{
    public class AddACRDto
    {
        public string Name { get; set; }
        public string ScpIp { get; set; }
        public short AccessConfig { get; set; }
        public short PairACRNo { get; set; }
        // Reader setting for Reader In
        public short ReaderSioNumber { get; set; }
        public short ReaderNumber { get; set; }
        // Reader setting for Reader In
        public short ReaderDataFormat { get; set; } = 0x01;
        public short KeyPadMode { get; set; } = 2;
        public byte OsdpBaudRate { get; set; } = 0x00;
        public byte OsdpNoDiscover { get; set; } = 0x00;
        public byte OsdpTracing { get; set; } = 0x00;
        public byte OsdpAddress { get; set; } = 0x00;
        public byte OsdpSecureChannel { get; set; } = 0x00;
        public bool IsReaderOsdp { get; set; } = false;

        
        // Output setting for strike
        public short StrikeSioNumber { get; set; }
        public short StrikeNumber { get; set; }
        public short StrikeMinActiveTime { get; set; }
        public short StrikeMaxActiveTime { get;set; }
        public short StrikeMode { get; set; } = 0;
        public short StrikeRelayDriveMode { get; set; }
        public short StrikeRelayOfflineMode { get; set; }
        public short RelayMode { get; set; }

        //Input setting for sensor
        public short SensorSioNumber { get; set; }
        public short SensorNumber { get; set; }
        public short HeldOpenDelay { get; set; }
        public short SensorMode { get; set; }
        public short SensorDebounce { get; set; } = 6;
        public short SensorHoldTime { get; set; } = 0;

        //Input setting for rex0
        public bool IsREX0Used { get; set; } = false;
        public short REX0SioNumber { get; set; }
        public short REX0Number { get; set; }
        public short REX0TimeZone { get; set; } = 0;
        public short REX0SensorMode { get; set; }
        public short REX0SensorDebounce { get; set; } = 2;
        public short REX0SensorHoldTime { get; set; } = 0;

        //Input setting for rex1
        public bool IsREX1Used { get; set; } = false;
        public short REX1SioNumber { get; set; }
        public short REX1Number { get; set; }
        public short REX1TimeZone { get;set; }
        public short REX1SensorMode { get; set; }
        public short REX1SensorDebounce { get; set; } = 2;
        public short REX1SensorHoldTime { get; set; } = 0;
        //Reader setting for reader out
        public bool IsAlternateReaderUsed { get; set; } = false;
        public short AlternateReaderSioNumber { get; set; }
        public short AlternateReaderNumber { get;set; }
        public short AlternateReaderConfig { get; set; }
        public bool IsAlternateReaderOsdp { get; set; } = false;

        // Reader setting for Reader out
        public short AlternateReaderDataFormat { get; set; } = 0x01;
        public short AlternateKeyPadMode { get; set; } = 2;
        public byte AlternateOsdpBaudRate { get; set; } = 0x00;
        public byte AlternateOsdpNoDiscover { get; set; } = 0x00;
        public byte AlternateOsdpTracing { get; set; } = 0x00;
        public byte AlternateOsdpAddress { get; set; } = 0x00;
        public byte AlternateOsdpSecureChannel { get; set; } = 0x00;

        public short CardFormat { get; set; }
        public short AntiPassbackMode { get; set; }
        public short AntiPassBackIn { get; set; }
        public short AntiPassBackOut { get; set; }
        public short SpareTags { get; set; }
        public short AccessControlFlags { get; set; }
        public short OfflineMode { get; set; }
        public short DefaultMode { get; set; }
        public short DefaultLEDMode { get; set; }
        public short PreAlarm { get; set; }
        public short AntiPassbackDelay { get; set; }

        // Advance Feature


    }
}
