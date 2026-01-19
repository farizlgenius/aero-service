
using Aero.Infrastructure.Data.Entities;

namespace Aero.Infrastructure.Data.Interface
{
    public interface IHardware
    {
        string mac { get; set; }
        Hardware hardware { get; set; }
    }
}
