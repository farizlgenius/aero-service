using System;

namespace Aero.Application.Interfaces;

public interface ITypeArea
{
       short status{ get; }

            //
            // Summary:
            //     occupancy count - standard users
             int occupancy{ get; }

            //
            // Summary:
            //     occupancy count - special users
             int occ_spc{ get; }

            //
            // Summary:
            //     flags before change
             short prior_status{ get; }
}
