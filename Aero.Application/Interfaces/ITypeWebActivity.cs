using System;

namespace Aero.Application.Interfaces;

public interface ITypeWebActivity
{
      //
            // Summary:
            //     0 = Web page, 1 = PSIA
             byte iType{ get; }

            //
            // Summary:
            //     Current Logged in User, -1 = Invalid
             byte iCurUserId{ get; }

            //
            // Summary:
            //     Modified or Action User, -1 = Invalid
             byte iObjectUserId{ get; }

            //
            // Summary:
            //     Modified or Action User Name
             char[] szObjectUser{ get; }

            //
            // Summary:
            //     IP Address of attempt
             int ipAddress{ get; }
}
