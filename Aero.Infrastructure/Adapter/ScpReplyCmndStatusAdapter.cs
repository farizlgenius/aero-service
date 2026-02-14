using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Adapter
{
    public class ScpReplyCmndStatusAdapter(SCPReplyMessage.SCPReplyCmndStatus cmnd_sts) : IScpReplyCmndStatus
    {
        public short status => cmnd_sts.status;

        public int sequence_number => cmnd_sts.sequence_number;

        public IScpReplyNAK nak => cmnd_sts.nak is null ? null :  new ScpReplyNakAdapter(cmnd_sts);
    }
}
