using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.Hubs;
using HIDAeroService.Service;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DI for custom service
            builder.Services.AddScoped<ScpService>();
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<MessageHandler>();
            builder.Services.AddScoped<CpService>();
            builder.Services.AddScoped<SioService>();
            builder.Services.AddScoped<AcrService>();
            builder.Services.AddScoped<SysService>();
            builder.Services.AddScoped<TZService>();
            builder.Services.AddScoped<CardFormatService>();
            builder.Services.AddScoped<AlvlService>();
            builder.Services.AddScoped<MpService>();
            builder.Services.AddScoped<HelperService>();
            builder.Services.AddScoped<CredentialService>();
            builder.Services.AddScoped<CmndService>();
            builder.Services.AddSignalR();

            // Register AeroDriver
            builder.Services.AddSingleton<WriteAeroDriver>();
            builder.Services.AddSingleton<ReadAeroDriver>();

            builder.Services.AddSingleton<AppConfigData>(provider =>
            {
                var read = provider.GetRequiredService<ReadAeroDriver>();
                var write = provider.GetRequiredService<WriteAeroDriver>();
                return new AppConfigData
                {
                    read = read,
                    write = write
                };
            });

            // Add Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder => {
                    builder.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });

            });

            var app = builder.Build();
            // Resolve driver from DI
            var writeDriver = app.Services.GetRequiredService<WriteAeroDriver>();
            var readDriver = app.Services.GetRequiredService<ReadAeroDriver>();

            // Initial Driver

            writeDriver.TurnOnDebug();

            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var sys = scopedServices.GetRequiredService<SysService>();

                // Now you can safely use sys here
                sys.InitialDriver(3333);
            }



            var threadListener = new Thread(readDriver.GetTransactionUntilShutDown);
            threadListener.Start();


            app.Lifetime.ApplicationStopping.Register(() =>
            {
                readDriver.SetShutDownflag();
                Thread.Sleep(1000);
                writeDriver.TurnOffDebug();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigins");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<EventHub>("/eventHub");
            app.MapHub<ScpHub>("/scpHub");
            app.MapHub<SioHub>("/sioHub");
            app.MapHub<CpHub>("/cpHub");
            app.MapHub<MpHub>("/mpHub");
            app.MapHub<AcrHub>("/acrHub");
            app.MapHub<CredentialHub>("/credentialHub");
            app.MapHub<CmndHub>("/cmndHub");

            app.Run();


        }
    }
}
