using Aero.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Entities
{
    public sealed class LicenseComponent : ILicenseComponent
    {
        public short Location { get; set; }
        public short Operator { get; set; }
        public short Role { get; set; }
        public short Hardware { get; set; }
        public short Module { get; set; }
        public short Camera { get; set; }
        public short ControlPoint { get; set; }
        public short MonitorPoint { get; set; }
        public short MonitorGroup { get; set; }
        public short Door { get; set; }
        public short CardHolder { get; set; }
        public short AccessLevel { get; set; }
        public short AccessArea { get; set; }
        public short Timezone { get; set; }
        public short Holiday { get; set; }
        public short Trigger { get; set; }
        public short Procedure { get; set; }
    }
}
