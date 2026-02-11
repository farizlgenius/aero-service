using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TypeDblCardFullAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeDblCardFull
{
      public short format_number => tran.c_fulldbl.format_number;

      public int facility_code => tran.c_fulldbl.facility_code;

      public double cardholder_id => tran.c_fulldbl.cardholder_id;

      public short issue_code => tran.c_fulldbl.issue_code;

      public short floor_number => tran.c_fulldbl.floor_number;

      public byte[] encoded_card => tran.c_fulldbl.encoded_card;
}
