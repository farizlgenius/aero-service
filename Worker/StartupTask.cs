using System;
using AeroService.Data;
using AeroService.Helpers;

namespace AeroService.Worker;

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

            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Check for key in specific table
            string folderPath = Path.Combine(AppContext.BaseDirectory, "encrypt_data");
            if (!Directory.Exists(folderPath))
            {
                  Directory.CreateDirectory(folderPath);
            }

            // secret
            string secFile = Path.Combine(folderPath, "secret");
            if (!File.Exists(secFile))
            {
                  File.Create(secFile).Close(); // Close immediately to release handle
            }

            // public
            string pubFile = Path.Combine(folderPath, "pub");
            if (!File.Exists(pubFile))
            {
                  File.Create(pubFile).Close(); // Close immediately to release handle
            }
            // Check if file has content
            bool pubContent = new FileInfo(pubFile).Length > 0;

            // private
            string priFile = Path.Combine(folderPath, "pri");
            if (!File.Exists(priFile))
            {
                  File.Create(priFile).Close(); // Close immediately to release handle
            }
            // Check if file has content
            bool priContent = new FileInfo(priFile).Length > 0;

            // Generate keys if not exist
            if (!pubContent || !priContent)
            {
                  var (publicKey, privateKey) = EncryptHelper.GenerateEcdhKeyPair();

                  // Write to files
                  await File.WriteAllBytesAsync(pubFile, publicKey);
                  await File.WriteAllBytesAsync(priFile, privateKey);
            }
      }
}
