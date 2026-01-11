using System.Net;

namespace AeroService.Constant
{
    public class ResponseMessage
    {

        public static string COMMAND_UNSUCCESS = "Send command to Controller failed.";
        public static string UNSUCCESS = "Unsuccess";

        // ResponseDto message
        public static string SCP_NOT_FOUND = "NO Main Controller In System";
        public static string DOOR_NOT_FOUND = "accesslevel_door_timezone not found in the System.";
        public static string COMPONENT_EXCEED_LIMIT = "HardwareComponent Exceed The Limit,Please Remove Some Before CreateAsync New HardwareComponent";
        public static string SCP_ALREADY_REGISTER = "Controller Already Register in System";
        public static string DELETE_DEFAULT = "Default record";




        // HTTP Return message
        public static string SUCCESS = "Success";
        public static string WARNING = "Success with Errors Found";
        public static string REMOVE_SUCCESS = "Removed";
        public static string NOT_FOUND = "Not Found";
        public static string CREATED = "Created";
        public static string INTERNAL_ERROR = "Internal Error";
        public static string REQUEST_TIMEOUT = "Request Timeout";
        public static string UNAUTHORIZED = "Unauthorized";

        // System
        public static string NO_SYSTEM_CONFIG_IN_DB = "No System Configuration In Database.";
        public static string NO_SCP_COMPONENT_IN_DB = "No Scp HardwareComponent In Database";




        public static string NOT_FOUND_RECORD = "Record Not Found.";
        public static string DUPLICATE_USER = "Duplicate Record.";
        public static string FOUND_RELATE_REFERENCE = "Found Relate Reference";

        // SIO


        // Upload message
        public static string UPLOAD_COMPARE_SCP_DATA = "Compare Controller data...";
        public static string UPLOAD_CONTROL_POINT = "Upload Control Point...";
        public static string UPLOAD_SUCCESS = "Upload Successful";

        // password message 
        public static string OLD_PASSPORT_INCORRECT = "Old password incorrect";
        public static string PASSWORD_UNASSIGN = "password unassign";


    }
}
