using HIDAeroService.DTO;
using HIDAeroService.DTO.License;

namespace HIDAeroService.Service
{
    public interface ILicenseService
    {
        Task<ResponseDto<MachineIdentityDto>> GetMachineIdAsync();
        Task<ResponseDto<bool>> AddLicenseAsync();
    }
}
