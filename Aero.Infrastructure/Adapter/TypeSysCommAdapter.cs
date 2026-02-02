using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeSysCommAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeSysComm
{
      public short error_code => tran.sys_comm.error_code;

      public short current_primary_comm => tran.sys_comm.current_primary_comm;

      public short previous_primary_comm => tran.sys_comm.previous_primary_comm;

      public short current_alternate_comm => tran.sys_comm.current_alternate_comm;

      public short previous_alternate_comm => tran.sys_comm.previous_alternate_comm;
}
