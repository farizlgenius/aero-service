using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.License;
using AeroService.Entity;
using AeroService.Helpers;
using Microsoft.Win32;
#if WINDOWS
using System.Management;
#endif

namespace AeroService.Service.Impl
{
    public sealed class LicenseService(AppDbContext context) : ILicenseService
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

        public async Task<ResponseDto<bool>> TrustServerAsync(TrustServerDto dto)
        {
            // Step 1 : Generate ECDH key pair
            var (publicKey, privateKey) = EcdhCryptoHelper.GenerateEcdhKeyPair();

            // Step 2 : Derive shared secret
            var serverPublicKeyBytes = Convert.FromBase64String(dto.peerPublicKey);
            var sharedSecret = EcdhCryptoHelper.DeriveSharedSecret(privateKey, serverPublicKeyBytes);

            // Step 3 : Store the public key and shared secret in the database
            var keyPair = new KeyPair
            {
                public_key = publicKey,
                shared_secret = sharedSecret,
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow
            };
            await context.KeyPairs.AddAsync(keyPair);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }
    }
}
