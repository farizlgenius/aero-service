using System;

namespace Aero.Domain.Interfaces;

public interface ITypeCoSElevatorAccess
{
      //
            // Summary:
            //     Card Id
             long cardholder_id{ get; }

            //
            // Summary:
            //     floors map
             byte[] floors{ get; }

            //
            // Summary:
            //     card format
             byte nCardFormat{ get; }
}
