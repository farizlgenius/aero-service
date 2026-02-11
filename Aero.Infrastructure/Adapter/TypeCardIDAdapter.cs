using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeCardIDAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCardID
{
      public short format_number => tran.c_id.format_number;

      public int cardholder_id => tran.c_id.cardholder_id;

      public short floor_number => tran.c_id.floor_number;

      public short card_type_flags => tran.c_id.card_type_flags;

      public short elev_cab => tran.c_id.elev_cab;
}
