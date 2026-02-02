using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public class HostCommIpClientAdapter(HOST_COMM_IPCLIENT ipclient) : IHostCommIpClient
{
      public int cHostIP => ipclient.cHostIP;

      public short nPort => ipclient.nPort;

      public short rqIntvl => ipclient.rqIntvl;

      public short connMode => ipclient.connMode;

      public char[] cHostName => ipclient.cHostName;

      public short nNicSel =>  ipclient.nNicSel;
}
