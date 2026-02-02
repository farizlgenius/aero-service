using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class CommStatusAdapter(SCPReplyMessage.SCPReplyCommStatus comm) : ICommStatus
{
      public int status => comm.status;
      public int error_code => (int)comm.error_code;

      public short nChannelId => comm.nChannelId;

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
      public short current_primary_comm => comm.current_primary_comm; 

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
      public short previous_primary_comm => comm.previous_primary_comm;

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
      public short current_alternate_comm => comm.current_alternate_comm;

      //
      // Summary:
      //     0 == detached, 1 == not tried, 2 == off-line, 3 == on-line
      public short previous_alternate_comm => comm.previous_alternate_comm;

}
