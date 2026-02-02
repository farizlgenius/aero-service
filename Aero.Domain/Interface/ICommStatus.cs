using System;

namespace Aero.Domain.Interfaces;

public interface ICommStatus
{
       int status {get;}
       int error_code {get;}
       short nChannelId {get;}

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
       short current_primary_comm {get;}

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
       short previous_primary_comm {get;}

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
       short current_alternate_comm {get;}
      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
       short previous_alternate_comm {get;}
}
