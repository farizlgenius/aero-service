using System;

namespace Aero.Domain.Interfaces;

public interface IHostCommIpClient
{
      //
      // Summary:
      //     Host IP address
      int cHostIP {get;}

      //
      // Summary:
      //     Port number
      short nPort {get;}

      //
      // Summary:
      //     Request interval (5, 10, 20)
       short rqIntvl {get;}

      //
      // Summary:
      //     Connection Mode ( 0=Continuous, 1=OnDemand)
       short connMode {get;}

      //
      // Summary:
      //     Host Name
       char[] cHostName {get;}

      //
      // Summary:
      //     Enable Authorized IP addresses (0=disable, 1=enable)
       short nNicSel {get;}



}
