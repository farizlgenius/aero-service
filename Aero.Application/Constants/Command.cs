namespace Aero.Api.Constants
{
    public sealed class Command
    {
        public static string ALVL_CONFIG = "command 2116: Access Level Configuration Extended";
        public static string HOL_CONFIG = "command 1104: Holiday Configuration";
        public static string TIMEZONE_SPEC = "command 3103: Extended Time Zone Act Specification";
        public static string CARDFORMAT_CONFIG = "command 1102: Card Formatter Configuration";
        public static string C208 = "command 208: Detach SCP from Channel";
        public static string RESET_SCP = "command 301: Reset SCP";
        public static string OUTPUT_SPEC = "command 111: Output Point Specification";
        public static string CONTROL_CONFIG = "command 114: Control Point Configuration";
        public static string CP_STATUS = "command 406: enCcCpSrq";
        public static string MONITOR_CONFIG = "command 113: Monitor Point Configuration";
        public static string INPUT_SPEC = "command 110: Input Point Specification";
        public static string MP_STATUS = "command 405: enCcMpSrq";
        public static string CP_COMMAND = "command 307: Control Point Command";
        public static string SET_MASK = "command 306: Monitor Point MaskAsync";
        public static string MODULE_STATUS = "command 404: Get Module Status";
        public static string MOMENT_UNLOCK = "command 311: Momentary Unlock";
        public static string READER_SPEC = "command 112: Reader Specification";
        public static string ACR_CONFIG = "command 115: Access Control Reader Configuration";
        public static string C407 = "command 407: enCcAcrSrq";
        public static string ACR_MODE = "command 308: ACR mode";
        public static string C401 = "command 401: enCcIDRequest";
        public static string CARD_RECORD = "command 8304: Access Database Card Records";
        public static string DELETE_CARD = "command 3305: Card Delete";
        public static string SCP_STRUCTURE_STATUS = "command 1853: SCP Structure status Read";
        public static string AREA_CONFIG = "command 1121: Configure Access Area";
        public static string C303 = "command 303: Set the transaction Log Index";
        public static string CONFIG_MPG = "command 120: Configure Monitor Point Group";
        public static string MPG_ARM_DISARM = "command 321: Monitor Point Group Arm/Disarm command";
        public static string C118 = "command 118: action Specification";
        public static string C117 = "command 117: trigger Specification";
        public static string C1117 = "command 1117: trigger Specification (Expanded Code Map)";
        public static string SIO_DRIVER = "command 108: Driver Configuration";
        public static string SIO_PANEL_CONFIG = "command 109: SIO Panel Configuration";
        public static string C402 = "command 402: enCcTranSrq";
        public static string TZ_CONTROL = "command 314: Time zone Control";
        public static string C1107 = "command 1107: SCP Device Specification";
        public static string C1105 = "command 1105: Access Database Specification";
        public static string DELETE_SCP = "command 015: Delete SCP";
        public static string C412 = "command 412: enCcAreaSrq";
        public static string C302 = "command 302: time Set";
        public enum CommandFlags
        {
            enCcMpSrq = 405, // Request sensor status
        }

    }
}
