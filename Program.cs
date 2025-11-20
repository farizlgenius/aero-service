using HIDAeroService.AeroLibrary;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Hubs;
using HIDAeroService.Service.Impl;
using Microsoft.EntityFrameworkCore;
using HIDAeroService.Mapper;
using Serilog;
using HIDAeroService.Exceptions.Middleware;
using HIDAeroService.Entity;
using HIDAeroService.Service;
using HIDAeroService.DTO.Credential;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HIDAeroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // read config
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var key = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            // Bind AppSettings section
            // Bind AppSettings section to AppSettings class
            builder.Services.Configure<AppConfigSettings>(
                builder.Configuration.GetSection("AppSettings")
                );

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true; // set false only for local dev
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();

            // AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CardFormatProfile>();
                cfg.AddProfile<ScpProfile>();
                cfg.AddProfile<IdReportProfile>();
                cfg.AddProfile<SioProfile>();
                cfg.AddProfile<HolidayProfile>();
                cfg.AddProfile<AcrProfile>();
                cfg.AddProfile<AccessAreaProfile>();
                // Add more profiles here if needed
            });

            // SeriLog
            // Read Serilog config from appsettings.json
            Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .Enrich.With<Logging.CLog>()
                .Enrich.FromLogContext()        // important
                .CreateLogger();

            // Replace default logging with Serilog
            builder.Host.UseSerilog();


            // DI for custom service
            builder.Services.AddScoped<IHardwareService,HardwareService>();
            builder.Services.AddScoped<ITimeZoneService,TimeZoneService>();
            builder.Services.AddScoped<IAccessLevelService, AccessLevelService>();
            builder.Services.AddScoped<ICardFormatService, CardFormatService>();
            builder.Services.AddScoped<IControlPointService, ControlPointService>();
            builder.Services.AddScoped<IAccessAreaService, AccessAreaService>();
            builder.Services.AddScoped<IHolidayService, HolidayService>();
            builder.Services.AddScoped<IDoorService, DoorService>();
            builder.Services.AddScoped<IIntervalService, IntervalService>();
            builder.Services.AddScoped<IMonitorPointService, MonitorPointService>();
            builder.Services.AddScoped<IModuleService, ModuleService>();
            builder.Services.AddScoped<ICommand, CommandService>();
            builder.Services.AddScoped<ICredentialService, CredentialService>();
            builder.Services.AddScoped<ICardHolderService, CardHolderService>();
            builder.Services.AddScoped<IControlPointService, ControlPointService>();
            builder.Services.AddScoped<IHelperService, HelperService>();
            builder.Services.AddScoped<ILicenseService, LicenseService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();
            builder.Services.AddScoped<ILocationService,LocationService>();
            builder.Services.AddScoped<IRoleService, RoleService>();


            //
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<MessageHandler>();

            
            builder.Services.AddScoped<SysService>();
            builder.Services.AddScoped(typeof(IHelperService<>), typeof(HelperService<>));

            
            builder.Services.AddScoped<CmndService>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IdReportService>();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            



            // Register AeroDriver
            builder.Services.AddSingleton<AeroCommand>();
            builder.Services.AddSingleton<AeroMessage>();


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
            var writeDriver = app.Services.GetRequiredService<AeroCommand>();
            var readDriver = app.Services.GetRequiredService<AeroMessage>();

            // CLog
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Initial Driver

            logger.LogInformation("Application starting at {time}", DateTime.Now);
            writeDriver.TurnOnDebug();

            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var sys = scopedServices.GetRequiredService<SysService>();

                // Now you can safely use sys here
                if(!sys.ConfigureDriver())
                {
                    logger.LogError("Initial driver failed. Shutting down app...");
                    app.Lifetime.StopApplication(); // graceful shutdown
                }
            }

            // Adding Exception Middlewre
            app.UseMiddleware<ExceptionHandlingMiddleware>();



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
            app.UseAuthentication();


            app.MapControllers();
            app.MapHub<AeroHub>(HubConstants.WEB_SOCKET_HUB);
            app.MapHub<EventHub>(HubConstants.EVENT_HUB);
            app.MapHub<ScpHub>(HubConstants.SCP_HUB);
            app.MapHub<SioHub>(HubConstants.SIO_HUB);
            app.MapHub<CpHub>(HubConstants.CP_HUB);
            app.MapHub<MpHub>(HubConstants.MP_HUB);
            app.MapHub<AcrHub>(HubConstants.ACR_HUB);
            app.MapHub<CredentialHub>(HubConstants.CREDENTIAL_HUB);
            app.MapHub<CmndHub>(HubConstants.CMND_HUB);


            app.Run();


        }
    }
}
