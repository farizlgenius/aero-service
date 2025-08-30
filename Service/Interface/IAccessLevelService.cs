using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Entity;

namespace HIDAeroService.Service.Interface
{
    public interface IAccessLevelService
    {
        Task<IEnumerable<AccessLevelDto>> GetAll();
        IEnumerable<ArAccessLevel> GetAllSetting();
        Task<AccessLevelTimeZoneDto> GetTimeZone(short AccessLevelNo);
        Task<AccessLevelDto> Create(CreateAccessLevelDto dto);
        Task<AccessLevelDto> Save(CreateAccessLevelDto dto, short AccessLevelNo);
        Task<AccessLevelDto> Remove(short AccessLevelNo);


    }
}
