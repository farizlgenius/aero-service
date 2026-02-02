using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public class SrCpAdapter(SCPReplyMessage.SCPReplySrCp sts_cp) : ISrCp
{
      public short first => sts_cp.first;
      public short count => sts_cp.count;
      public short[] status => sts_cp.status;
}
