

namespace Aero.Domain.Entities;

public class BaseDomain 
{
    public int LocationId { get; private set; } = 1;
    public bool IsActive { get; private set; } = true;

    public BaseDomain() { }

    public BaseDomain(int location,bool status) 
    {
        SetLocation(location);
        IsActive = status;
    }

    private void SetLocation(int location) 
    {
        if (location <= 0) throw new ArgumentException("Location invalid.");

        this.LocationId = location;
    }


}
