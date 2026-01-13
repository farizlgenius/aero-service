using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Entity;
using AeroService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
#if WINDOWS
using System.Management;
#endif

namespace AeroService.Service.Impl
{
    public sealed class LicenseService(AppDbContext context,IApiService api,IOptions<AppConfigSettings> options) : ILicenseService
    {
        private readonly AppConfigSettings settings = options.Value;
        public async Task<ResponseDto<MachineFingerPrintDto>> GetMachineIdAsync()
        {
            var dto = new MachineFingerPrintDto
            {
                FingerPrint = MachineFingerprint.Get()
            };
            return ResponseHelper.SuccessBuilder(dto);
        }

        
        public Task<ResponseDto<bool>> AddLicenseAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<bool>> ExchangeAsync(TrustServerDto dto)
        {

            // Step 1 : Checking key pair in database
            var key = await context.key_pair
            .AsNoTracking()
            .OrderBy(x => x.key_uuid)
            .FirstOrDefaultAsync(x => x.is_revoked == false);

            if(key is null) return ResponseHelper.NotFoundBuilder<bool>();

            var body = new TrustServerDto("","");

            await api.PostTrustedLicenseServer(settings.LicenseServerUrl, body);
            return ResponseHelper.SuccessBuilder(true);
        }


    }
}
