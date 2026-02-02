using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class StrStatusAdapter(SCPReplyMessage.SCPReplyStrStatus str_sts) : IStrStatus
{
      public short nListLength => str_sts.nListLength;

      public IStrSpec[] sStrSpec => str_sts.sStrSpec.Select(x => new StrSpecAdapter
      {
            nActive = x.nActive,
            nStrType = x.nStrType,
            nRecords = x.nRecords,
            nRecSize = x.nRecSize
      }).ToArray();
}
