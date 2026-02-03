using System;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TranStatusAdapter(SCPReplyMessage.SCPReplyTranStatus tran_sts) : ITranStatus
{
      public int capacity => tran_sts.capacity;

      public int oldest => tran_sts.oldest;

      public int last_rprtd => tran_sts.last_rprtd;
      public int last_loggd => tran_sts.last_loggd;
      public short disabled => tran_sts.disabled;
}
