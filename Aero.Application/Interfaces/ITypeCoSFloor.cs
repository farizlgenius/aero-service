using System;

namespace Aero.Application.Interfaces;

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
