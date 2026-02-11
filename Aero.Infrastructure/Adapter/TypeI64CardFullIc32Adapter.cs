using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class TypeI64CardFullIc32Adapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeI64CardFullIc32
    {
        public short format_number => tran.c_fulli64i32.format_number;

        public int facility_code => tran.c_fulli64i32.facility_code;

        public long cardholder_id => tran.c_fulli64i32.cardholder_id;

        public int issue_code => tran.c_fulli64i32.issue_code;

        public short floor_number => tran.c_fulli64i32.floor_number;

        public byte[] encoded_card => tran.c_fulli64i32.encoded_card;
    }
}
