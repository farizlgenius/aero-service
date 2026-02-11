using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class TypeI64CardIDAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeI64CardID
    {
        public short format_number => tran.c_idi64.format_number;

        public long cardholder_id => tran.c_idi64.cardholder_id;

        public short floor_number => tran.c_idi64.floor_number;

        public short card_type_flags => tran.c_idi64.card_type_flags;

        public short elev_cab => tran.c_idi64.elev_cab;
    }
}
