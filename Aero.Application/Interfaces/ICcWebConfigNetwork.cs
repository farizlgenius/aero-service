using System;

namespace Aero.Application.Interfaces;

public interface ICcWebConfigNetwork
{
      short scp_number{get;}
      short method {get;}
      int cIpAddr {get;}
      int cSubnetMask {get;}
       int cDfltGateway {get;}

      //
      // Summary:
      //     Host Name
       char[] cHostName {get;}

      //
      // Summary:
      //     dnsType
       short dnsType {get;}

      //
      // Summary:
      //     cDns
       int cDns {get;}

      //
      // Summary:
      //     DNS Suffix
       char[] cDnsSuffix {get;}

      //
      // Summary:
      //     method2
       short method2 {get;}

      //
      // Summary:
      //     cIpAddr2
       int cIpAddr2 {get;}

      //
      // Summary:
      //     cSubnetMask2
       int cSubnetMask2 {get;}

      //
      // Summary:
      //     cDfltGateway2
       int cDfltGateway2 {get;}

      //
      // Summary:
      //     cDns2
       int cDns2 {get;}

      //
      // Summary:
      //     LSPEnable
       short TnlEnable {get;}

      //
      // Summary:
      //     cIpLSP
       int cIpTnl {get;}

      //
      // Summary:
      //     cPortLSP
       int cPortTnl {get;}



}
