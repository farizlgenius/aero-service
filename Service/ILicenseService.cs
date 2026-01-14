using AeroService.DTO;
using AeroService.DTO.License;

namespace AeroService.Service
{
    public interface ILicenseService
    {
        Task<ResponseDto<MachineFingerPrintDto>> GetMachineIdAsync();
        Task<ResponseDto<bool>> AddLicenseAsync();
        Task<ResponseDto<bool>> InitialSessionAsync();
        Task<ResponseDto<bool>> CheckLicenseAsync();
    }
}
