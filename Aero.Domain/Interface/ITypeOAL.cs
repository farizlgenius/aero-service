using System;

namespace Aero.Domain.Interfaces;

public interface ITypeOAL
{
      //
            // Summary:
            //     Reason Code
             byte nReasonCode {get;}

            //
            // Summary:
            //     OAL Data
             byte[] nData {get;}
}
