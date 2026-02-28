using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.DTOs
{

    public sealed record HolidayDto(int Id,short DriverId,string Name,short Year,short Month,short Day,short Extend,short TypeMask,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
}
