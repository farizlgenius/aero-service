

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ILicenseService
    {
        Task<ResponseDto<MachineFingerPrintDto>> GetMachineIdAsync();
        Task<ResponseDto<bool>> AddLicenseAsync();
        Task<ResponseDto<bool>> CheckLicenseAsync();
        Task<ResponseDto<bool>> GenerateDemoLicenseAsync(GenerateDemoRequest request);
    }
}
