using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeCardBcdAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCardBcd
{
      public short digit_count => tran.c_bcd.digit_count;

      public byte[] bcd_array => tran.c_bcd.bcd_array;
}
