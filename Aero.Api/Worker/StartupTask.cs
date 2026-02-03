
using Aero.Domain.Helpers;
using Aero.Infrastructure.Data;

namespace Aero.Api.Worker;

public class StartupTask : IHostedService
{
      private readonly IServiceScopeFactory _scopeFactory;

      public StartupTask(IServiceScopeFactory scopeFactory)
      {
            _scopeFactory = scopeFactory;
      }

      public async Task StartAsync(CancellationToken cancellationToken)
      {
            await RunOnStartupAsync(cancellationToken);
      }

      public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

      private async Task RunOnStartupAsync(CancellationToken cancellationToken)
      {
            Console.WriteLine("Startup function executed...");
            // Check for key in specific table
            string folderPath = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(folderPath))
            {
                  Directory.CreateDirectory(folderPath);
            }

            // public
            string pubFile = Path.Combine(folderPath, "pub_sign.key");
            if (!File.Exists(pubFile))
            {
                  File.Create(pubFile).Close(); // Close immediately to release handle
            }
            // Check if file has content
            bool pubContent = new FileInfo(pubFile).Length > 0;

            // private
            string priFile = Path.Combine(folderPath, "pri_sign.key");
            if (!File.Exists(priFile))
            {
                  File.Create(priFile).Close(); // Close immediately to release handle
            }
            // Check if file has content
            bool priContent = new FileInfo(priFile).Length > 0;

            // Generate keys if not exist
            if (!pubContent || !priContent)
            {
                  var signer = EncryptHelper.CreateSigner();

                  // Write to files
                  await File.WriteAllBytesAsync(pubFile, signer.ExportSubjectPublicKeyInfo());
                  await File.WriteAllBytesAsync(priFile, signer.ExportPkcs8PrivateKey());
            }
      }
}
