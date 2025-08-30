using HIDAeroService.AeroLibrary;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Hubs;
using HIDAeroService.Service;
using HIDAeroService.Service.Impl;
using HIDAeroService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using HIDAeroService.Mapper;
using Serilog;
using HIDAeroService.Logging;

namespace HIDAeroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind AppSettings section
            // Bind AppSettings section to AppSettings class
            builder.Services.Configure<AppConfigSettings>(
                builder.Configuration.GetSection("AppSettings")
                );

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CardFormatProfile>();
                cfg.AddProfile<AccessLevelProfile>();
                cfg.AddProfile<TimeZoneProfile>();
                cfg.AddProfile<IntervalProfile>();
                // Add more profiles here if needed
            });

            // SeriLog
            // Read Serilog config from appsettings.json
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .Enrich.With<CustomLogging>()
                .Enrich.FromLogContext()        // important
                .CreateLogger();

            // Replace default logging with Serilog
            builder.Host.UseSerilog();

            // DI for custom service
            builder.Services.AddScoped<ScpService>();
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<MessageHandler>();
            builder.Services.AddScoped<CpService>();
            builder.Services.AddScoped<SioService>();
            builder.Services.AddScoped<AcrService>();
            builder.Services.AddScoped<SysService>();
            builder.Services.AddScoped<ITimeZoneService,TimeZoneService>();
            builder.Services.AddScoped<IIntervalService,IntervalService>();
            builder.Services.AddScoped<IAccessLevelService, AccessLevelService>();
            builder.Services.AddScoped<IHolidayService, HolidayService>();
            builder.Services.AddScoped<MpService>();
            builder.Services.AddScoped<HelperService>();
            builder.Services.AddScoped<CredentialService>();
            builder.Services.AddScoped<CmndService>();
            builder.Services.AddScoped<ICardFormatService,CardFormatService>();
            builder.Services.AddSignalR();


            // Register AeroDriver
            builder.Services.AddSingleton<WriteAeroDriver>();
            builder.Services.AddSingleton<ReadAeroDriver>();

            builder.Services.AddSingleton<AeroLibMiddleware>(provider =>
            {
                var read = provider.GetRequiredService<ReadAeroDriver>();
                var write = provider.GetRequiredService<WriteAeroDriver>();
                return new AeroLibMiddleware
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

            // Logging
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Initial Driver

            logger.LogInformation("Application starting at {time}", DateTime.Now);
            writeDriver.TurnOnDebug();

            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var sys = scopedServices.GetRequiredService<SysService>();

                // Now you can safely use sys here
                if(sys.ConfigureDriver() != Constants.ConstantsHelper.INITIAL_DRIVER_SUCCESS)
                {
                    logger.LogError("Initial driver failed. Shutting down app...");
                    app.Lifetime.StopApplication(); // graceful shutdown
                }
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
            // This logs every HTTP request automatically
            app.UseSerilogRequestLogging();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<EventHub>(Constants.ConstantsHelper.EVENT_HUB);
            app.MapHub<ScpHub>(Constants.ConstantsHelper.SCP_HUB);
            app.MapHub<SioHub>(Constants.ConstantsHelper.SIO_HUB);
            app.MapHub<CpHub>(Constants.ConstantsHelper.CP_HUB);
            app.MapHub<MpHub>(Constants.ConstantsHelper.MP_HUB);
            app.MapHub<AcrHub>(Constants.ConstantsHelper.ACR_HUB);
            app.MapHub<CredentialHub>(Constants.ConstantsHelper.CREDENTIAL_HUB);
            app.MapHub<CmndHub>(Constants.ConstantsHelper.CMND_HUB);

            app.Run();


        }
    }
}
