using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class TypeI64CardFullAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeI64CardFull
    {
        public short format_number => tran.c_fulli64.format_number;

        public int facility_code => tran.c_fulli64.facility_code;

        public long cardholder_id => tran.c_fulli64.cardholder_id;

        public short issue_code => tran.c_fulli64.issue_code;

        public short floor_number => tran.c_fulli64.floor_number;

        public byte[] encoded_card => tran.c_fulli64.encoded_card;


    }
}
