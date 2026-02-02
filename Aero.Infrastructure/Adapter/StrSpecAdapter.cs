using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class StrSpecAdapter : IStrSpec
{
      public short nStrType {get; set;}

      public int nRecords {get;set;}

      public int nRecSize {get; set;}

      public int nActive {get; set;}
}
