using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class CcWebConfigNetwork(CC_WEB_CONFIG_NETWORK web_network) : ICcWebConfigNetwork
{
      public short scp_number  => web_network.scp_number;

      public short method  => web_network.method;
      public int cIpAddr  => web_network.cIpAddr;

      public int cSubnetMask  => web_network.cSubnetMask;

      public int cDfltGateway  => web_network.cDfltGateway;

      public char[] cHostName  => web_network.cHostName;

      public short dnsType  => web_network.dnsType;

      public int cDns  => web_network.cDns;

      public char[] cDnsSuffix  => web_network.cDnsSuffix;

      public short method2  => web_network.method2;

      public int cIpAddr2  => web_network.cIpAddr2;

      public int cSubnetMask2 => web_network.cSubnetMask2;

      public int cDfltGateway2  => web_network.cDfltGateway2;

      public int cDns2  => web_network.cDns2;

      public short TnlEnable  => web_network.TnlEnable;

      public int cIpTnl  => web_network.cIpTnl;

      public int cPortTnl  => web_network.cPortTnl;
}
