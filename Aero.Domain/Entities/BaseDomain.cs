

namespace Aero.Domain.Entities;

public class BaseDomain 
{
    public int LocationId { get; private set; } = 1;

    public BaseDomain() { }

    public BaseDomain(int location) 
    {
        SetLocation(location);
    }

    private void SetLocation(int location) 
    {
        if (location <= 0) throw new ArgumentException("Location invalid.");

        this.LocationId = location;
    }
}
