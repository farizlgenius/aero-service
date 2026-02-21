using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interface
{
    public interface IScpReplyCmndStatus
    {
        short status { get; }
        int sequence_number { get; }
        IScpReplyNAK nak { get; }
    }
}
