using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeSysAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeSys
{
      public short error_code => tran.sys.error_code;
}
