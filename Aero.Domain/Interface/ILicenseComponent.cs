using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Interface
{
    internal interface ILicenseComponent
    {
         short Location { get; set; }
         short Operator { get; set; }
         short Role { get; set; }
        short Hardware { get; set; }
        short Module { get; set; }
        short Camera { get; set; }
         short ControlPoint { get; set; }
         short MonitorPoint { get; set; }
        short MonitorGroup { get; set; }
        short Door { get; set; }
        short CardHolder { get; set; }
        short AccessLevel { get; set; }
        short AccessArea { get; set; }
        short Timezone { get; set; }
        short Holiday { get; set; }
        short Trigger { get; set; }
        short Procedure { get; set; }
        

    }
}
