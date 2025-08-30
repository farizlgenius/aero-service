using System.Net;

namespace HIDAeroService.Constants
{
    public class ConstantsHelper
    {
        // Hub Endpoint
        public static string EVENT_HUB = "/eventHub";
        public static string SCP_HUB = "/scpHub";
        public static string SIO_HUB = "/sioHub";
        public static string CP_HUB = "/cpHub";
        public static string MP_HUB = "/mpHub";
        public static string ACR_HUB = "/acrHub";
        public static string CREDENTIAL_HUB = "/credentialHub";
        public static string CMND_HUB = "/cmndHub";


        public static short SUCCESS_CODE = 200;
        public static short NOT_FOUND_CODE = 404;
        public static short CREATED_CODE = 201;
        public static short INTERNAL_ERROR_CODE = 500;

        // HTTP Return Message
        public static string SUCCESS = "Success";
        public static string REMOVE_SUCCESS = "Removed";
        public static string NOT_FOUND = "Not Found";
        public static string CREATED = "Created";
        public static string INTERNAL_ERROR = "Internal Error";

        // System
        public static string NO_SYSTEM_CONFIG_IN_DB = "No System Configuration In Database.";
        public static string NO_SCP_COMPONENT_IN_DB = "No Scp Component In Database";
        public static string SYSTEM_LEVEL_SPEC_COMMAND_UNSUCCESS = "System Level Specification : Unsuccess";
        public static string SYSTEM_LEVEL_SPEC_COMMAND_SUCCESS = "System Level Specification : Success";
        public static string CREATE_CHANNEL_COMMAND_UNSUCCESS = "Create Channel : Unsuccess";
        public static string CREATE_CHANNEL_COMMAND_SUCCESS = "Create Channel : Success";
        public static string INITIAL_DRIVER_SUCCESS = "Initial Driver : Success";

        // Card Formatter
        public static string CARD_FORMATTER_COMMAND_UNSUCCESS = "Card Formatter Configuration Command : Unsuccess";

        // Time Zone
        public static string EXTEND_TIME_ZONE_SPECIFICATION_UNSUCCESS = "Extended Time Zone Act Specification Command : Unsuccess";

        // Access Level
        public static string ACCESS_LEVEL_CONFIGURATION_UNSUCCESS = "Access Level Configuration Extended : Unsuccess";

        // SCP
        public static string DELETE_SCP_FAIL = "Delete Scp unsuccess.";
        public static string DETACH_SCP_FAIL = "Detach Scp from channel unsuccess.";
        public static string NOT_FOUND_RECORD = "Record Not Found";
        public static string COMMAND_UNSUCCESS = "Send Command To Drivers Unsuccess";
        public static string COMMAND_SUCCESS = "Send Command To Driver Success";
        public static string SCP_DEVICE_SPECIFICATION_UNSUCCESS = "SCP Device Specification Command : Unsuccess";
        public static string ACCESS_DATABASE_SPECIFICATION_UNSUCCESS = "Access Database Specification Command : Unsuccess";
        public static string TIME_SET_UNSUCCESS = "Time Set Command : Unsuccess";

        // SIO
        public static string SIO_DRIVER_CONFIGURATION_UNSUCCESS = "SIO Driver Configuration Command : Unsuccess";
        public static string SIO_PANEL_CONFIGURATION_UNSUCCESS = "SIO Panel Configuration Command : Unsuccess";

        // Input
        public static string INPUT_SPECIFICATION_UNSUCCESS = "Input Specification Command : Unsuccess";

        // Upload Message
        public static string UPLOAD_COMPARE_SCP_DATA = "Compare Controller Data...";
        public static string UPLOAD_CONTROL_POINT = "Upload Control Point...";
        public static string UPLOAD_SUCCESS = "Upload Successful";
    }
}
