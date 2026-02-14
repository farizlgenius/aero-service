using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Interface
{
    public interface IScpReplyNAK
    {
        short reason { get; }
        int data { get; }
        byte[] command { get; }
        int description_code { get; }
    }
}
