using System.Text;
using System.Text.Json;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
#if WINDOWS
using System.Management;
#endif

namespace Aero.Application.Services
{
    public sealed class LicenseService(AppDbContext context, IApiService api, IOptions<AppConfigSettings> options, IDatabase redis) : ILicenseService
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

        private async Task<ResponseDto<HandshakeResult>> InitHandshakeAsync()
        {

            // Step 1 : Generate Dh and Load Signer from
            var appDh = EncryptHelper.CreateDh();
            var appDhPublic = appDh.ExportSubjectPublicKeyInfo();

            // Step 2 : Get Public Sign from file
            string pubSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pub_sign.key");
            if (!File.Exists(pubSignFile))
            {
                return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, "Sign public key file not found");
            }

            if (new FileInfo(pubSignFile).Length <= 0)
            {
                return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, "Sign public key empty");
            }

            // Step 3 : Get Private Sign from file
            string priSignFile = Path.Combine(Path.Combine(AppContext.BaseDirectory, "data"), "pri_sign.key");
            if (!File.Exists(priSignFile))
            {
                return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, "Sign private key file not found");
            }

            if (new FileInfo(pubSignFile).Length <= 0)
            {
                return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, "Sign private key empty");
            }

            // Step 4 : Sign ECDH public key with Sign private key
            var appSingPublic = await File.ReadAllBytesAsync(pubSignFile);
            var appSignPrivate = await File.ReadAllBytesAsync(priSignFile);
            var signData = appDhPublic.Concat(appSingPublic).ToArray();
            var signature = EncryptHelper.SignData(EncryptHelper.LoadSignerPrivateKey(appSignPrivate), signData);

            // Step 5 : Exchange Key with license server

            var body = new ExchangeRequest(Guid.NewGuid().ToString(), Convert.ToBase64String(appDhPublic), Convert.ToBase64String(appSingPublic), Convert.ToBase64String(signature));

            var response = await api.ExchangeAsync(body);

            if (response.payload is null) return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, response.message);

            // Step 6 : Calculate License server response
            var serverDhPublic = Convert.FromBase64String(response.payload.dhPub);
            var serverSignPublic = Convert.FromBase64String(response.payload.signPub);
            var serverSignature = Convert.FromBase64String(response.payload.signature);

            var licVerifyData = serverDhPublic.Concat(serverSignPublic).ToArray();
            if (!EncryptHelper.VerifyData(licVerifyData, serverSignature, serverSignPublic))
            {
                // Verify fail
                return ResponseHelper.UnsuccessBuilderWithString<HandshakeResult>(ResponseMessage.INTERNAL_ERROR, "Exchange key verify data fail");
            }

            // Step 7 : Derive Shared Key
            var sharedKey = EncryptHelper.DeriveSecretKey(appDh, serverDhPublic);
            var aesKey = EncryptHelper.DeriveAesKey(sharedKey, options.Value.Secret);

            return ResponseHelper.SuccessBuilder(new HandshakeResult(response.payload.sessionId, aesKey));
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


        public async Task<ResponseDto<bool>> GenerateDemoLicenseAsync(GenerateDemoRequest request)
        {
            // Step 1 : Initial Handshake with license server
            var handshakeRes = await InitHandshakeAsync();
            if (handshakeRes.data is null) return ResponseHelper.UnsuccessBuilder<bool>(handshakeRes.message, handshakeRes.details);

            // Step 2 : Send Session Id to license server to generate demo license
            var body = new GenerateDemoRequest(request.company, request.customerSite, request.machineId, handshakeRes.data.sessionId);
            var response = await api.GenerateDemoLicenseAsync(body);

            if (response.payload is null) return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.INTERNAL_ERROR,response.message);

            // Step 3 : Decrypt License Content
            var decryptedLicense = EncryptHelper.DecryptAes(Convert.FromBase64String(response.payload.Payload), handshakeRes.data.sharedKey);

            // Step 4 : Parse License Payload
            var licensePayload = EncryptHelper.ParsePayload(decryptedLicense);

            // Step 5 : Verify License Signature
            if (!EncryptHelper.VerifyData(decryptedLicense, licensePayload.signature, Convert.FromBase64String(response.payload.ServerSignPublic)))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.INTERNAL_ERROR, "License signature verify fail");
            }

            // Step 6 : Encrypt License Data
            var key = EncryptHelper.Hash(options.Value.Secret);
            var d = EncryptHelper.EncryptAes(Encoding.UTF8.GetBytes(key),licensePayload.license);
            
            string licenseString = Encoding.UTF8.GetString(licensePayload.license);

            // Step 6 : Save License File
            string folderPath = Path.Combine(AppContext.BaseDirectory, "data/license.lic");
            await File.WriteAllTextAsync(folderPath, Convert.ToBase64String(d));

            return ResponseHelper.SuccessBuilder(true);
        }
    }
}
