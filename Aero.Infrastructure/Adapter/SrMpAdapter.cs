using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public class SrMpAdapter(SCPReplyMessage.SCPReplySrMp sts_mp) : ISrMp
{
      public short first => sts_mp.first;

      public short count => sts_mp.count;

      public short[] status => sts_mp.status;
}
