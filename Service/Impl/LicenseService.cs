using AeroService.Constant;
using AeroService.Data;
using AeroService.Dto.License;
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

        public async Task<ResponseDto<bool>> InitialSessionAsync()
        {

            // Step 1 : Generate Dh and Load Signer from
            var Dh = EncryptHelper.CreateDh();
            var pubDh = Dh.ExportSubjectPublicKeyInfo();

            // Step 2 : Get Public Sign from file
            string pubSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pub_sign.key");
            if (!File.Exists(pubSignFile))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR,"Sign public key file not found");
            }

            if(new FileInfo(pubSignFile).Length <= 0)
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR, "Sign public key empty");
            }

            // Step 3 : Exchange Key with license server
            var pubSign = await File.ReadAllBytesAsync(pubSignFile);

            var body = new ExchangeRequest(Convert.ToBase64String(pubDh),Convert.ToBase64String(pubSign));

            var response = await api.ExchangeAsync(body);
            
            // Step 4 : Calculate License server response
            var licDhPub = Convert.FromBase64String(response.dhPub);
            var licSignPub = Convert.FromBase64String(response.signPub);
            var licSignature = Convert.FromBase64String(response.signature);

            var licVerifyData = EncryptHelper.ExportDhPublicKey(Dh).Concat(licDhPub).ToArray();
            if (!EncryptHelper.VerifyData(licVerifyData,licSignature,licSignPub))
            {
                // Verify fail
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR,"Exchange key verify data fail");
            }

            // Step 5 : Get Sign private key for sign data
            string priSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pri_sign.key");
            if (!File.Exists(priSignFile))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR,"Sign private key file not found");
            }

            if(new FileInfo(pubSignFile).Length <= 0)
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR, "Sign private key empty");
            }

            var priSign = await File.ReadAllBytesAsync(priSignFile);

            // Step 6 : Create signature for lic server to verify
            var signData = licDhPub.Concat(pubDh).ToArray();
            var signer = EncryptHelper.LoadSignerPrivateKey(priSign);
            var signature = EncryptHelper.SignData(signer,signData);

            // Step 7 : Send back to license server
            var verifyBody = new VerifyRequest(Convert.ToBase64String(signature)); 
            var verifyResponse = await api.VerifyAsync(verifyBody);

            if (!verifyResponse) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.INTERNAL_ERROR,"Initial Session Verify signature fail");

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> CheckLicenseAsync()
        {
            // Step 1 : License File Exist
            string folderPath = Path.Combine(AppContext.BaseDirectory, "data/license.lic");
            if (!File.Exists(folderPath) && new FileInfo(folderPath).Length > 0)
            {
                return ResponseHelper.NotFoundBuilder<bool>(["License not found"]);
            }

            // Step 2 : Check Secret Key Exist
            string secFile = Path.Combine(AppContext.BaseDirectory, "encrypt_data/secret");
            if (!File.Exists(secFile) && new FileInfo(secFile).Length > 0)
            {
                return ResponseHelper.NotFoundBuilder<bool>(["Secret key not found"]);
            }

            // Step 3 : Validate License Content
            // ... (omitted for brevity)

            return ResponseHelper.SuccessBuilder(true);
        }
    }
}
