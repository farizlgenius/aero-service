using System;

namespace Aero.Application.Interfaces;

public interface ITypeSysComm
{
      short error_code { get; }

      //
      // Summary:
      //     0 == off-line, 1 == active, 2 == standby
      short current_primary_comm { get; }

      //
      // Summary:
      //     0 == off-line, 1 == active, 2 == standby
      short previous_primary_comm { get; }

      //
      // Summary:
      //     0 == off-line, 1 == active, 2 == standby
      short current_alternate_comm { get; }

      //
      // Summary:
      //     0 == off-line, 1 == active, 2 == standby
      short previous_alternate_comm { get; }
}
