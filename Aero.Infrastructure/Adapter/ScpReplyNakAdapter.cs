using Aero.Application.Interfaces;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public sealed class ScpReplyNakAdapter(SCPReplyMessage.SCPReplyCmndStatus cmnd_sts) : IScpReplyNAK
    {
        public short reason => cmnd_sts.nak.reason;

        public int data => cmnd_sts.nak.reason;

        public byte[] command => cmnd_sts.nak.command;

        public int description_code => cmnd_sts.nak.description_code;
    }
}
