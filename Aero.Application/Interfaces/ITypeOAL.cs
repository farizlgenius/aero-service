using System;

namespace Aero.Application.Interfaces;

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
