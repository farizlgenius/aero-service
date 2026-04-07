using System;

namespace Aero.Application.Constants;

public static class ResponseMessage
{
      public static string SUCCESS = "Success.";
      public static string UNSUCCESS = "Unsuccess.";
      public static string NOT_FOUND = "Record not found.";
    public static string BAD_REQUEST = "Bad request.";
      public static string FOUND_REFERENCE = "Found reference.";
      public static string COMPONENT_EXCEED_LIMIT = "Component exceed license limit.";
      public static string DUPLICATE_RECORD = "Duplicate record.";
    public static string DUPLICATE_NAME = "Duplicate name.";
    public static string UNAUTHORIZED = "Unauthorized.";
      public static string DELETE_DEFAULT = "Delete default.";
      public static string COMMAND_UNSUCCESS = "Send command unsuccess.";
      public static string UPLOAD_HW_CONFIG_FAIL = "Upload hardware configuration fail.";
      public static string SAVE_DATABASE_UNSUCCESS = "Save to database unsuccess.";
      public static string DELETE_DATABASE_UNSUCCESS = "Delete from database unsuccess.";
      public static string UPDATE_RECORD_UNSUCCESS = "Update record in database unsuccess.";
      public static string UPDATE_DOOR_MODE_UNSUCCESS = "Update door mode record unsuccess.";
      public static string REMOVE_OLD_REF_UNSUCCESS = "Remove old reference for update unsucccess.";
      public static string PASSWORD_UNASSIGN = "Password unassigned.";
      public static string OLD_PASSPORT_INCORRECT = "Old password incorrect.";
      public static string LICENSE_ERR = "License error.";
    public static string TRANSACTION_ENABLE_FAIL = "Enable transaction fail.";
}
