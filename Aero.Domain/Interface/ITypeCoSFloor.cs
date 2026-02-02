using System;

namespace Aero.Domain.Interfaces;

public interface ITypeCoSFloor
{
      //
            // Summary:
            //     Previous Floor Status
             byte prevFloorStatus{ get; }

            //
            // Summary:
            //     Floor Number
             byte floorNumber{ get; }
}
