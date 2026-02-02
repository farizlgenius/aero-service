using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeSioCommAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeSioComm
{
      public short comm_sts => tran.s_comm.comm_sts;

      public byte model => tran.s_comm.model;

      public byte revision => tran.s_comm.revision;

      public int ser_num => tran.s_comm.ser_num;

      public short nExtendedInfoValid => tran.s_comm.nExtendedInfoValid;

      public short nHardwareId => tran.s_comm.nHardwareId;

      public short nHardwareRev => tran.s_comm.nHardwareRev;

      public short nProductId => tran.s_comm.nProductId;

      public short nProductVer => tran.s_comm.nProductVer;

      public short nFirmwareBoot => tran.s_comm.nFirmwareBoot;

      public short nFirmwareLdr => tran.s_comm.nFirmwareLdr;

      public short nFirmwareApp => tran.s_comm.nFirmwareApp;

      public short nOemCode => tran.s_comm.nOemCode;

      public byte nEncConfig => tran.s_comm.nEncConfig;

      public byte nEncKeyStatus => tran.s_comm.nEncKeyStatus;

      public byte[] mac_addr => tran.s_comm.mac_addr;

      public int nHardwareComponents => tran.s_comm.nHardwareComponents;
}
