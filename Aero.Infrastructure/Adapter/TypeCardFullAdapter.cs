using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeCardFullAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCardFull
{
      public short format_number => tran.c_full.format_number;

      public int facility_code => tran.c_full.facility_code;

      public int cardholder_id => tran.c_full.cardholder_id;

      public short issue_code => tran.c_full.issue_code;

      public short floor_number => tran.c_full.floor_number;

      public byte[] encoded_card => tran.c_full.encoded_card;
}
