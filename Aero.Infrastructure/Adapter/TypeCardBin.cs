using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeCardBin(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCardBin
{
      public short bit_count => tran.c_bin.bit_count;

      public byte[] bit_array => tran.c_bin.bit_array;
}
