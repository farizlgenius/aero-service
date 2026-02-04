using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class TypeCosAdapter(SCPReplyMessage.SCPReplyTransaction tran) : ITypeCoS
    {
        public byte status => tran.cos.status;

        public byte old_sts => tran.cos.old_sts;
    }
}
