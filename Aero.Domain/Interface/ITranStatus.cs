using System;

namespace Aero.Domain.Interface;

public interface ITranStatus
{
      //
            // Summary:
            //     the transaction buffer size (allocated)
             int capacity{ get; }

            //
            // Summary:
            //     serial number of the oldest TR in the file
             int oldest{ get; }

            //
            // Summary:
            //     serial number of the last reported TR
             int last_rprtd{ get; }

            //
            // Summary:
            //     serial number of the last logged TR
             int last_loggd{ get; }

            //
            // Summary:
            //     non-zero if disabled with (Command_303)
             short disabled{ get; }
}
