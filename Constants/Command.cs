namespace HIDAeroService.Constants
{
    public sealed class Command
    {
        public static string C2116 = "command 2116: Access Level Configuration Extended";
        public static string C1104 = "command 1104: Holiday Configuration";
        public static string C3103 = "command 3103: Extended time Zone Act Specification";
        public static string C1102 = "command 1102: Card Formatter Configuration";
        public static string C208 = "command 208: Detach SCP from Channel";
        public static string C301 = "command 301: Reset SCP";
        public static string C111 = "command 111: Strike Point Specification";
        public static string C114 = "command 114: Control Point Configuration";
        public static string C406 = "command 406: enCcCpSrq";
        public static string C113 = "command 113: Monitor Point Configuration";
        public static string C110 = "command 110: sensor Point Specification";
        public static string C405 = "command 405: enCcMpSrq";
        public static string C307 = "command 307: Control Point command";
        public static string C306 = "command 306: Monitor Point MaskAsync";
        public static string C404 = "command 404: enCcSioSrq";
        public static string C311 = "command 311: Momentary Unlock";
        public static string C112 = "command 112: Reader Specification";
        public static string C115 = "command 115: Access Control Reader Configuration";
        public static string C407 = "command 407: enCcAcrSrq";
        public static string C308 = "command 308: ACR mode";
        public static string C401 = "command 401: enCcIDRequest";
        public static string C8304 = "command 8304: Access Database Card Records";
        public static string C3305 = "command 3305: Card DeleteAsync";
        public static string C1853 = "command 1853: SCP Structure status Read";
        public static string C1121 = "command 1121: Configure Access Area";
        public static string C303 = "command 303: Set the transaction Log Index";
        public static string C120 = "command 120: Configure Monitor Point Group";
        public static string C321 = "command 321: Monitor Point Group Arm/Disarm command";
        public static string C118 = "command 118: action Specification";
        public static string C117 = "command 117: trigger Specification";
        public static string C1117 = "command 1117: trigger Specification (Expanded Code Map)";
        public static string C108 = "command 108: Driver Configuration";
        public static string C109 = "command 109: SIO Panel Configuration";
        public static string C402 = "command 402: enCcTranSrq";
        public static string C314 = "command 314: time Zone Control";
        public static string C1107 = "command 1107: SCP Device Specification";
        public static string C1105 = "command 1105: Access Database Specification";
        public static string C015 = "command 015: Delete SCP";
        public static string C412 = "command 412: enCcAreaSrq";
        public static string C302 = "command 302: time Set";
        public enum CommandFlags
        {
            enCcMpSrq = 405, // Request sensor status
        }

    }
}
