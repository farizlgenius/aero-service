using HIDAeroService.DTO;
using HIDAeroService.DTO.License;
using HIDAeroService.Helpers;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Win32;
using System.Management;

namespace HIDAeroService.Service.Impl
{
    public sealed class LicenseService : ILicenseService
    {
        public async Task<ResponseDto<MachineIdentityDto>> GetMachineIdAsync()
        {
            var dto = new MachineIdentityDto
            {
                MotherBoardSerialNumber = GetMotherboardSerial(),
                MachineGuid = GetMachineGuid(),
            };
            return ResponseHelper.SuccessBuilder<MachineIdentityDto>(dto);
        }

        private string GetMotherboardSerial()
        {
            // Uses System.Management (Windows-only)
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT serial_number FROM Win32_BaseBoard"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        var val = mo["serial_number"]?.ToString();
                        if (!string.IsNullOrEmpty(val))
                            return val.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to query Win32_BaseBoard: " + ex.Message, ex);
            }
            return null;
        }

        private string GetMachineGuid()
        {
            const string key = @"SOFTWARE\Microsoft\Cryptography";
            const string value = "MachineGuid";
            using (var rk = Registry.LocalMachine.OpenSubKey(key))
            {
                if (rk == null) throw new InvalidOperationException("Registry key not found");
                var guidObj = rk.GetValue(value);
                return guidObj?.ToString() ?? throw new InvalidOperationException("MachineGuid not found");
            }
        }

        public Task<ResponseDto<bool>> AddLicenseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
