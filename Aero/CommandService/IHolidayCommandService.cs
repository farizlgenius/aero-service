using HIDAeroService.Entity;

namespace HIDAeroService.Aero.CommandService
{
    public interface IHolidayCommandService
    {
        Task<bool> HolidayConfigurationAsync(Holiday dto, short ScpId);
        Task<bool> DeleteHolidayConfigurationAsync(Holiday dto, short ScpId);
        Task<bool> ClearHolidayConfigurationAsync(short ScpId);
    }
}
