using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Helpers;
using Microsoft.Win32;
#if WINDOWS
using System.Management;
#endif

namespace AeroService.Service.Impl
{
    public sealed class LicenseService : ILicenseService
    {
        public async Task<ResponseDto<MachineFingerPrintDto>> GetMachineIdAsync()
        {
            var dto = new MachineFingerPrintDto
            {
                FingerPrint = MachineFingerprint.Get()
            };
            return ResponseHelper.SuccessBuilder<MachineFingerPrintDto>(dto);
        }

        

        public Task<ResponseDto<bool>> AddLicenseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
