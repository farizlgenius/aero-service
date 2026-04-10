using System;

namespace Aero.Api.Exceptions;

public class UnauthorizedException : Exception
{
      public UnauthorizedException()
      {
            
      }
      public UnauthorizedException(string message) : base(message)
      {
            
      }

      public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
      {
            
      }
}
