using System;

namespace Aero.Application.Constants;

public static class ResponseMessage
{
      public static string SUCCESS = "Success.";
      public static string UNSUCCESS = "Unsuccess.";
      public static string NOT_FOUND = "Record not found.";
      public static string FOUND_REFERENCE = "Found reference.";
      public static string COMPONENT_EXCEED_LIMIT = "Component exceed license limit.";
      public static string DUPLICATE_USER = "Duplicate user.";
      public static string UNAUTHORIZED = "Unauthorized.";
      public static string DELETE_DEFAULT = "Delete default.";
      public static string COMMAND_UNSUCCESS = "Send command unsuccess.";
      public static string UPLOAD_HW_CONFIG_FAIL = "Upload hardware configuration fail.";
      public static string SAVE_DATABASE_UNSUCCESS = "Save to database unsuccess.";
      public static string DELETE_DATABASE_UNSUCCESS = "Delete from database unsuccess.";
      public static string UPDATE_RECORD_UNSUCCESS = "Update record in database unsuccess.";
}
