using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class SrSioAdapter : ISrSio
{
    private readonly SCPReplyMessage.SCPReplySrSio _src;

    public SrSioAdapter(SCPReplyMessage.SCPReplySrSio sts_sio)
    {
        _src = sts_sio;
    }

    public short number => _src.number;
    public short com_status => _src.com_status;
    public short msp1_dnum => _src.msp1_dnum;
    public int com_retries => _src.com_retries;

    public short ct_stat => _src.ct_stat;
    public short pw_stat => _src.pw_stat;
    public short model => _src.model;
    public short revision => _src.revision;
    public int serial_number => _src.serial_number;

    public short inputs => _src.inputs;
    public short outputs => _src.outputs;
    public short readers => _src.readers;

    public short[] ip_stat => _src.ip_stat;
    public short[] op_stat => _src.op_stat;
    public short[] rdr_stat => _src.rdr_stat;

    public short nExtendedInfoValid => _src.nExtendedInfoValid;
    public short nHardwareId => _src.nHardwareId;
    public short nHardwareRev => _src.nHardwareRev;
    public short nProductId => _src.nProductId;
    public short nProductVer => _src.nProductVer;

    public short nFirmwareBoot => _src.nFirmwareBoot;
    public short nFirmwareLdr => _src.nFirmwareLdr;
    public short nFirmwareApp => _src.nFirmwareApp;
    public short nOemCode => _src.nOemCode;

    public byte nEncConfig => _src.nEncConfig;
    public byte nEncKeyStatus => _src.nEncKeyStatus;
    public byte[] mac_addr => _src.mac_addr;

    public short emg_stat => _src.emg_stat;
}
