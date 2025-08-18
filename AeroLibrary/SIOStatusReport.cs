using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIDAeroService.AeroLibrary
{
    public sealed class SIOStatusReport
    {
        public short SioNumber { get; set; }
        public short ComStatus { get; set; }
        public string ComStatusDesc { get; set; }
        public short Msp1DNum { get; set; }
        public int ComRetries { get; set; }
        public short CtState { get; set; }
        public string CtStateDesc { get; set; }
        public short PwState { get; set; }
        public string PwStateDescDesc { get; set; }
        public short Model { get; set; }
        public string ModelDesc { get; set; }
        public short Revision { get; set; }
        public int SerialNumber { get; set; }
        public short Inputs { get; set; }
        public short Outputs { get; set; }
        public short Readers { get; set; }
        public Dictionary<short, string> InputStatus { get; set; }
        public Dictionary<short, string> OutputStatus { get; set; }
        public Dictionary<short, string> ReaderStatus { get; set; }
        public short NExtendedInfoValid { get; set; }
        public short NHardwareId { get; set; }
        public string NHardwareIdDesc { get; set; }
        public short NHardwareRev { get; set; }
        public short ProductId { get; set; }


    }
}
