using System;

namespace Aero.Api.Exceptions;

public class ForbiddenException : Exception
{
      public ForbiddenException()
      {
            
      }
      public ForbiddenException(string message) : base(message)
      {
            
      }

      public ForbiddenException(string message, Exception innerException) : base(message, innerException)
      {
            
      }
}
