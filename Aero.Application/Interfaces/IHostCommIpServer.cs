using System;

namespace Aero.Application.Interfaces;

public interface IHostCommIpServer
{
      //
      // Summary:
      //     Authorized IP address 1
      int cAuthIP1 {get;}

      //
      // Summary:
      //     Authorized IP address 2
      int cAuthIP2 {get;}

      //
      // Summary:
      //     Port number
      short nPort {get;}

      //
      // Summary:
      //     Enable Authorized IP addresses (0=disable, 1=enable)
      short enableAuthIP{get;}

      //
      // Summary:
      //     Enable Authorized IP addresses (0=disable, 1=enable)
      short nNicSel{get;}
}
