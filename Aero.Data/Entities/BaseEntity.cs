using System;

namespace Aero.Domain.Entities;

public class BaseEntity
{
    private short _componentId;
    public string Uuid { get; set; } = string.Empty;
    public short ComponentId { 
        get => _componentId; 
        set
        {
            if(value <= 0)
            {
                throw new Exception("Invalid Component Id");
            }
            _componentId = value;
        } 
    }
    public string HardwareName { get; set; } = string.Empty;
    public string Mac { get; set; } = string.Empty;
    private short _locationId;
    public short LocationId 
    { 
        get => _locationId;
        set
        {
            if(value <= 0)
            {
                throw new Exception("Invalid Location Id");
            }
            _locationId = value;
        }
    }
    public bool IsActive { get; set; }


}
