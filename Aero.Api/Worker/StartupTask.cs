
using Aero.Domain.Helpers;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Aero.Api.Worker;

public class StartupTask : IHostedService
{
      private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StartupTask> _logger;

    public StartupTask(IServiceScopeFactory scopeFactory, ILogger<StartupTask> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

      public async Task StartAsync(CancellationToken cancellationToken)
      {
            await RunOnStartupAsync(cancellationToken);
      }

      public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

       private async Task RunOnStartupAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🚀 StartupTask started");

        using var scope = _scopeFactory.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            // ⭐ STEP 1 — Database migration
            var db = services.GetRequiredService<AppDbContext>();
            _logger.LogInformation("Applying database migrations...");
            await db.Database.MigrateAsync(cancellationToken);


            _logger.LogInformation("Seeding roles...");
            await DbInitializer.SeedRolesAsync(services);

            // ⭐ STEP 3 — Your existing RSA key generation
            await EnsureJwtKeysAsync();

            _logger.LogInformation("✅ StartupTask completed");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "❌ StartupTask failed — application will stop");
            throw; // crash app intentionally if startup fails
        }
    }

    private async Task EnsureJwtKeysAsync()
    {
        string folderPath = Path.Combine(AppContext.BaseDirectory, "data");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string pubFile = Path.Combine(folderPath, "pub_sign.key");
        string priFile = Path.Combine(folderPath, "pri_sign.key");

        if (!File.Exists(pubFile)) File.Create(pubFile).Close();
        if (!File.Exists(priFile)) File.Create(priFile).Close();

        bool pubContent = new FileInfo(pubFile).Length > 0;
        bool priContent = new FileInfo(priFile).Length > 0;

        if (!pubContent || !priContent)
        {
            _logger.LogInformation("Generating RSA signing keys...");

            var signer = EncryptHelper.CreateSigner();

            await File.WriteAllBytesAsync(pubFile, signer.ExportSubjectPublicKeyInfo());
            await File.WriteAllBytesAsync(priFile, signer.ExportPkcs8PrivateKey());
        }
    }
}
