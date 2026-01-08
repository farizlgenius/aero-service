using HIDAeroService.Entity;

namespace AeroService.Entity.Interface
{
    public interface IHardware
    {
        string mac { get; set; }
        Hardware hardware { get; set; }
    }
}
