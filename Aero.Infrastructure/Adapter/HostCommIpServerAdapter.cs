using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class HostCommIpServerAdapter(HOST_COMM_IPSERVER ipserver) : IHostCommIpServer
{
      public int cAuthIP1 => ipserver.cAuthIP1;

      public int cAuthIP2 => ipserver.cAuthIP2;

      public short nPort => ipserver.nPort;

      public short enableAuthIP => ipserver.enableAuthIP;

      public short nNicSel => ipserver.nNicSel;
}
