using System;

namespace Aero.Application.Interfaces;

public interface ITypeUserCmnd
{
      //
      // Summary:
      //     number of user command digits entered
      short nKeys { get; }

      //
      // Summary:
      //     null terminated string: '0' though '9'
      char[] keys { get; }
}
