using System;

namespace Aero.Domain.Interfaces;

public interface ITypeCoS
{
       byte status{ get; }

            //
            // Summary:
            //     previous status (prior to this CoS report)
             byte old_sts{ get; }
}
