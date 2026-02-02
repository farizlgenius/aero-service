using System;

namespace Aero.Domain.Interfaces;

public interface ITypeMPG
{
      //
            // Summary:
            //     current mask count of this MPG
             short mask_count{ get; }

            //
            // Summary:
            //     number of active Monitor Points
             short nActiveMps{ get; }

            //
            // Summary:
            //     list of the first 10 active Point Pairs: "Type-Number"
             short[] nMpList{ get; }
}
