using System;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class SioCommandService : BaseAeroCommand, ISioCommand
{
  public bool SioDriverConfiguration(short ScpId, short SioDriverNo, short IoModulePort, int BaudRate, short ProtocolType)
  {
    CC_MSP1 cc_msp1 = new CC_MSP1();
    cc_msp1.lastModified = 0;
    cc_msp1.scp_number = ScpId;
    cc_msp1.msp1_number = SioDriverNo;
    cc_msp1.port_number = IoModulePort;
    cc_msp1.baud_rate = BaudRate;
    cc_msp1.reply_time = 90;
    cc_msp1.nProtocol = ProtocolType;
    cc_msp1.nDialect = 0;
    bool flag = Send((short)enCfgCmnd.enCcMsp1, cc_msp1);
    return flag;
  }

  public bool SioPanelConfiguration(short ScpId, short SioNo, short Model, short nInput, short nOutput, short nReader, short ModuleAddress, short SIODriverPort, bool isEnable)
  {
    CC_SIO cc_sio = new CC_SIO();
    cc_sio.lastModified = 0;
    cc_sio.scp_number = ScpId;
    cc_sio.sio_number = SioNo;
    cc_sio.nInputs = nInput;
    cc_sio.nOutputs = nOutput;
    cc_sio.nReaders = nReader;
    cc_sio.model = Model;
    cc_sio.revision = 0;
    cc_sio.ser_num_low = 0;
    cc_sio.ser_num_high = -1;
    cc_sio.enable = isEnable ? (short)1 : (short)0;
    cc_sio.port = SIODriverPort;
    cc_sio.channel_out = 0;
    cc_sio.channel_in = 0;
    cc_sio.address = ModuleAddress;
    cc_sio.e_max = 3;
    cc_sio.flags = 0x20;
    cc_sio.nSioNextIn = -1;
    cc_sio.nSioNextOut = -1;
    cc_sio.nSioNextRdr = -1;
    cc_sio.nSioConnectTest = 0;
    cc_sio.nSioOemCode = 0;
    cc_sio.nSioOemMask = 0;

    bool flag = Send((short)enCfgCmnd.enCcSio, cc_sio);
    return flag;
  }

  
}
