using HIDAeroService.Dto;
using HIDAeroService.Dto.TimeZone;
using HIDAeroService.Entity;
using System.Runtime.CompilerServices;

namespace HIDAeroService.Service.Interface
{
    public interface ITimeZoneService
    {
        IEnumerable<ArTimeZone> GetAllSetting();

        Task<Response<IEnumerable<TimeZoneDto>>> GetAsync();
        Task<Response<TimeZoneDto>> GetByIdAsync(short id);
        Task<Response<TimeZoneDto>> CreateAsync(TimeZoneDto dto);
        Task<Response<TimeZoneDto>> UpdateAsync(TimeZoneDto dto);
        Task<Response<TimeZoneDto>> DeleteAsync(short id);

        //void LogInfo(string message, [CallerMemberName] string methodName = "");
        //void LogWarn(string message, [CallerMemberName] string methodName = "");
        //void LogErr(string message, [CallerMemberName] string methodName = "");
        //void LogCri(string message, [CallerMemberName] string methodName = "");
    }
}
