using AeroService.Entity;

namespace AeroService.Aero.CommandService
{
    public interface IHolidayCommandService
    {
        Task<bool> HolidayConfigurationAsync(Holiday dto, short ScpId);
        Task<bool> DeleteHolidayConfigurationAsync(Holiday dto, short ScpId);
        Task<bool> ClearHolidayConfigurationAsync(short ScpId);
    }
}
