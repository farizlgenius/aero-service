
using Aero.Infrastructure.Data.Entities;

namespace Aero.Domain.Interface
{
    public interface IHardware
    {
        string mac { get; set; }
        Hardware hardware { get; set; }
    }
}
