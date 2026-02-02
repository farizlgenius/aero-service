using System;

namespace Aero.Domain.Interfaces;

public interface ITypeI64CardFullIc32
{
      //
            // Summary:
            //     index to the format table that was used, negative if reverse
             short format_number{ get; }

            //
            // Summary:
            //     facility code
             int facility_code{ get; }

            //
            // Summary:
            //     cardholder ID number
             long cardholder_id{ get; }

            //
            // Summary:
            //     issue code
             int issue_code{ get; }

            //
            // Summary:
            //     zero if not available (or not supported), else 1==first floor, ...
             short floor_number{ get; }

            //
            // Summary:
            //     Large encoded ID. (up to 32 bytes reported)
             byte[] encoded_card{ get; }
}
