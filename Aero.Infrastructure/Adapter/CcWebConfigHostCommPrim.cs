using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class CcWebConfigHostCommPrim(CC_WEB_CONFIG_HOST_COMM_PRIM web_host_comm_prim) : ICcWebConfigHostCommPrim
{
      public short scp_number => web_host_comm_prim.scp_number;

      public short address => web_host_comm_prim.address;

      public short dataSecurity => web_host_comm_prim.dataSecurity;

      public short cType => web_host_comm_prim.cType;

      public IHostCommIpServer ipserver => new HostCommIpServerAdapter(web_host_comm_prim.ipserver);

      public IHostCommIpClient ipclient => new HostCommIpClientAdapter(web_host_comm_prim.ipclient);
}
