using System.Text;
using System.Threading.Channels;
using Aero.Api.Constants;
using Aero.Api.Exceptions.Middleware;
using Aero.Api.Hubs;
using Aero.Api.Logging;
using Aero.Api.Publisher;
using Aero.Api.Worker;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Services;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using Aero.Infrastructure.Listenser;
using Aero.Infrastructure.Mapper;
using Aero.Infrastructure.Repositories;
using Aero.Infrastructure.Services;
using Aero.Infrastructure.Settings;
using AeroService.Service.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;

namespace AeroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // read config
            var jwtCfg = builder.Configuration.GetSection("Jwt");
            var jwtSecret = jwtCfg["Secret"];
            var issuer = jwtCfg["Issuer"];
            var audience = jwtCfg["Audience"];
            var accessTokenMinute = int.Parse(jwtCfg["AccessTokenMinutes"] ?? "10");

            // Register redis 
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"] ?? "localhost:6379";
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddSingleton(sp =>
            {
                var mux = sp.GetRequiredService<IConnectionMultiplexer>();
                return mux.GetDatabase();
            });

            // Bind AppSettings section
            // Bind AppSettings section to AppSettings class
            builder.Services
                .AddOptions<AppSettings>()
                .Bind(builder.Configuration.GetSection("AppSettings"))
                .ValidateOnStart();

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt")
            );

            builder.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true; // optional
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("PostgresConnection"),
                    npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    ));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            // Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.RequireHttpsMetadata = true; // set false only for local dev
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });


            // Http Service
            
            builder.Services.AddHttpClient();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true; // optional
            });

            builder.Services.AddSwaggerGen(c => 
                {
                    c.SwaggerDoc("v1", new() { Title = "HIS API", Version = "v1" });

                    // Add JWT Authorization header
                    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Please enter JWT with Bearer prefix. Example: Bearer {token}",
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                }
            );
            builder.Services.AddAuthorization();


            // SeriLog
            // Read Serilog config from appsettings.json
            Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .Enrich.With<CLog>()
                .Enrich.FromLogContext()        // important
                .CreateLogger();

            // Replace default logging with Serilog
            builder.Host.UseSerilog();

            // DI for App Setting
            builder.Services.AddSingleton<IAppSettings,AppSettings>();
            builder.Services.AddSingleton<IJwtSettings,JwtSettings>();
            builder.Services.AddSingleton<IRedisSettings,RedisSettings>();

            // DI for Command
            builder.Services.AddScoped<IAeroDriverCommand,AeroDriverCommandService>();
            builder.Services.AddScoped<IAlvlCommand,AlvlCommandService>();
            builder.Services.AddScoped<IAreaCommand,AreaCommandService>();
            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddSingleton<BaseAeroCommand>();
            builder.Services.AddScoped<ICfmtCommand,CfmtCommandService>();
            builder.Services.AddScoped<ICpCommand,CpCommandService>();
            builder.Services.AddScoped<IDoorCommand,DoorCommandService>();
            builder.Services.AddScoped<IHolCommand,HolCommandService>();
            builder.Services.AddScoped<IHolderCommand,HolderCommandService>();
            builder.Services.AddScoped<IMpCommand,MpCommandService>();
            builder.Services.AddScoped<IMpgCommand,MpgCommandService>();
            builder.Services.AddScoped<IProcCommand,ProcCommandService>();
            builder.Services.AddScoped<IScpCommand,ScpCommandService>();
            builder.Services.AddScoped<ISioCommand,SioCommandService>();
            builder.Services.AddScoped<ITrigCommand,TriggerCommandService>();
            builder.Services.AddScoped<ITzCommand,TzCommandService>();
            

            // DI for Repository
            builder.Services.AddScoped<IAlvlRepository,AlvlRepository>();
            builder.Services.AddScoped<IQAlvlRepository,QAlvlRepository>();
            builder.Services.AddScoped<IAreaRepository,AreaRepository>();
            builder.Services.AddScoped<IQAreaRepository,QAreaRepository>();
            builder.Services.AddScoped<IAuthRepository,AuthRepository>();
            builder.Services.AddScoped<ICfmtRepository,CfmtRepository>();
            builder.Services.AddScoped<IQCfmtRepository,QCfmtRepository>();
            builder.Services.AddScoped<ICpRepository,CpRepository>();
            builder.Services.AddScoped<IQCpRepository,QControlPointRepository>();
            builder.Services.AddScoped<ICredRepository,CredRepository>();
            builder.Services.AddScoped<IQCredRepository,QCredRepository>();
            builder.Services.AddScoped<IDoorRepository,DoorRepository>();
            builder.Services.AddScoped<IQDoorRepository,QDoorRepository>();
            builder.Services.AddScoped<IHolderRepository,HolderRepository>();
            builder.Services.AddScoped<IQHolderRepository,QHolderRepository>();
            builder.Services.AddScoped<IHttpRepository,HttpRepository>();
            builder.Services.AddScoped<IHwRepository,HwRepository>();
            builder.Services.AddScoped<IQHwRepository,QHwRepository>();
            builder.Services.AddScoped<IIntervalRepository,IntervalRepository>();
            builder.Services.AddScoped<IQIntervalRepository,QIntervalRepository>();
            builder.Services.AddScoped<ILocationRepository,LocationRepository>();
            builder.Services.AddScoped<IQLocationRespository,QLocationRepository>();
            builder.Services.AddScoped<IQModuleRepository,QModuleRepository>();
            builder.Services.AddScoped<IMpgRepository,MpgRepository>();
            builder.Services.AddScoped<IQMpgRepository,QMpgRepository>();
            builder.Services.AddScoped<IMpRepository,MpRepository>();
            builder.Services.AddScoped<IQMpRepository,QMpRepository>();
            builder.Services.AddScoped<IOperatorRepository,OperatorRepository>();
            builder.Services.AddScoped<IQOperatorRepository,QOperatorRepository>();
            builder.Services.AddScoped<IProcedureRepository,ProcedureRepository>();
            builder.Services.AddScoped<IQProcRepository,QProcRepository>();
            builder.Services.AddScoped<ITriggerRepository,TriggerRepository>();
            builder.Services.AddScoped<IQTrigRepository,QTrigRepository>();
            builder.Services.AddScoped<IRedisRepository,RedisRepository>();
            builder.Services.AddScoped<IRoleRepository,RoleRepository>();
            builder.Services.AddScoped<IQRoleRepository,QRoleRepository>();
            builder.Services.AddScoped<ISettingRepository,SettingRepository>();
            builder.Services.AddScoped<IQSettingRepository,QSettingRepository>();
            builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
            builder.Services.AddScoped<IQTransactionRepository,QTransactionRepository>();
            builder.Services.AddScoped<ITzRepository,TzRepository>();
            builder.Services.AddScoped<IQTzRepository,QTzRepository>();
            builder.Services.AddScoped<IQFeatureRepository, QFeatureRepository>();
            builder.Services.AddScoped<IQHolRepository, QHolRepository>();
            builder.Services.AddScoped<IHolRepository, HolRepository>();
            builder.Services.AddScoped<IQIdReportRepository, QIdReportRepository>();
            builder.Services.AddScoped<IQActionRepository, QActionRepository>();


            // DI for Service
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
            builder.Services.AddScoped<IAeroCommandService, AeroCommandService>();
            builder.Services.AddScoped<ICredentialService, CredentialService>();
            builder.Services.AddScoped<ICardHolderService, CardHolderService>();
            builder.Services.AddScoped<IControlPointService, ControlPointService>();
            builder.Services.AddScoped<ILicenseService, LicenseService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();
            builder.Services.AddScoped<ILocationService,LocationService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IFeatureService, FeatureService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddScoped<ITriggerService, TriggerService>();
            builder.Services.AddScoped<IMonitorGroupService, MonitorGroupService>();
            builder.Services.AddScoped<IAeroCommandService, AeroCommandService>();
            builder.Services.AddScoped<ISettingService, SettingService>();
            builder.Services.AddScoped<IFeatureService, FeatureService>();
            builder.Services.AddScoped<ILicenseService,LicenseService>();
            builder.Services.AddScoped<IMachineFingerprint,MachineFingerprint>();
            builder.Services.AddScoped<INotificationPublisher, ScpNotificationPublisher>();

            builder.Services.AddScoped<IAeroCommandService,AeroCommandService>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IdReportService>();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddHostedService<StartupTask>();



            // Register AeroDriver
            builder.Services.AddSingleton<AeroMessageListener>();


            builder.Services.AddSingleton(
                Channel.CreateBounded<IScpReply>(
                 new BoundedChannelOptions(10_000)
                    {
                        FullMode = BoundedChannelFullMode.DropOldest,
                        SingleReader = true,
                        SingleWriter = false
                    }
                )
             );

            builder.Services.AddHostedService<AeroWorker>();



            // Add Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder => {
                    builder.WithOrigins("http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    builder.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    builder.WithOrigins("http://192.168.1.170:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                    builder.WithOrigins("http://192.168.100.130:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });

            });

            var app = builder.Build();
            // Resolve driver from DI
            var writeDriver = app.Services.GetRequiredService<BaseAeroCommand>();
            var readDriver = app.Services.GetRequiredService<AeroMessageListener>();

            // CLog
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Initial Driver

            logger.LogInformation("Application starting at {time}", DateTime.UtcNow.ToLocalTime());
            writeDriver.TurnOnDebug();

            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var sys = scopedServices.GetRequiredService<IAeroDriverCommand>();

                // Now you can safely use sys here
                if(!sys.SystemLevelSpecification())
                {
                    logger.LogError("Initial driver failed. Shutting down app...");
                    app.Lifetime.StopApplication(); // graceful shutdown
                }

                // Now you can safely use sys here
                if(!sys.CreateChannel())
                {
                    logger.LogError("Initial driver failed. Shutting down app...");
                    app.Lifetime.StopApplication(); // graceful shutdown
                }
            }

            // Adding Exception Middlewre
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            var threadListener = new Thread(readDriver.GetTransactionUntilShutDown);
            threadListener.Start();


            app.Lifetime.ApplicationStopping.Register(async () =>
            {
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    // use db here
                    

                    // Delete all pending id report 
                    db.id_report.RemoveRange(db.id_report.ToArray());
                }                

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

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();
            app.MapHub<AeroHub>(HubConstants.WEB_SOCKET_HUB);


            app.Run();


        }
    }
}
