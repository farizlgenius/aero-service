namespace HIDAeroService.Constants
{
    public sealed class Command
    {
        public static string C2116 = "Command 2116: Access Level Configuration Extended";
        public static string C1104 = "Command 1104: Holiday Configuration";
        public static string C3103 = "Command 3103: Extended Time Zone Act Specification";
        public static string C1102 = "Command 1102: Card Formatter Configuration";
        public static string C208 = "Command 208: Detach SCP from Channel";
        public static string C015 = "Command 015: Delete SCP";
        public static string C301 = "Command 301: Reset SCP";
        public static string C111 = "Command 111: Strike Point Specification";
        public static string C114 = "Command 114: Control Point Configuration";
        public static string C406 = "Command 406: enCcCpSrq";
        public static string C113 = "Command 113: Monitor Point Configuration";
        public static string C110 = "Command 110: Sensor Point Specification";
        public static string C405 = "Command 405: enCcMpSrq";
        public static string C307 = "Command 307: Control Point Command";
        public static string C306 = "Command 306: Monitor Point MaskAsync";
        public static string C404 = "Command 404: enCcSioSrq";
        public static string C311 = "Command 311: Momentary Unlock";
        public static string C112 = "Command 112: Reader Specification";
        public static string C115 = "Command 115: Access Control Reader Configuration";
        public static string C407 = "Command 407: enCcAcrSrq";
        public static string C308 = "Command 308: ACR Mode";
        public static string C401 = "Command 401: enCcIDRequest";
        public static string C8304 = "Command 8304: Access Database Card Records";
        public static string C3305 = "Command 3305: Card Delete";
        public static string C1853 = "Command 1853: SCP Structure Status Read";
        public static string C1121 = "Command 1121: Configure Access Area";
        public static string C303 = "Command 303: Set the Transaction Log Index";

        public enum CommandFlags
        {
            enCcMpSrq = 405, // Request Sensor Status
        }

    }
}
