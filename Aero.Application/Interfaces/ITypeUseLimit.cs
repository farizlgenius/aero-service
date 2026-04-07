using System;

namespace Aero.Application.Interfaces;

public interface ITypeUseLimit
{
      //
            // Summary:
            //     the updated use count as a result of this access
             short use_count {get;}

            //
            // Summary:
            //     cardholder ID number
             long cardholder_id {get;}
}
