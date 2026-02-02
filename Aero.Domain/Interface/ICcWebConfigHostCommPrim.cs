using System;

namespace Aero.Domain.Interfaces;

public interface ICcWebConfigHostCommPrim
{
      short scp_number {get;}
      short address {get;}
      short dataSecurity {get;}
      short cType {get;}
      IHostCommIpServer ipserver {get;}
      IHostCommIpClient ipclient {get;}
}
