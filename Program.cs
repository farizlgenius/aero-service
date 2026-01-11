using AeroService.AeroLibrary;
using AeroService.Constants;
using AeroService.Data;
using AeroService.Hubs;
using AeroService.Service.Impl;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AeroService.Exceptions.Middleware;
using AeroService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Aero;
using HID.Aero.ScpdNet.Wrapper;
using System.Threading.Channels;

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

            // Redis config (StackExchange.Redis connection)
            var redisConn = builder.Configuration.GetValue<string>("Redis:Configuration") ?? "localhost:6379";

            // Bind AppSettings section
            // Bind AppSettings section to AppSettings class
            builder.Services.Configure<AppConfigSettings>(
                builder.Configuration.GetSection("AppSettings")
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

            builder.Services.AddEndpointsApiExplorer();
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
            builder.Services.AddScoped<ICommandService, CommandService>();
            builder.Services.AddScoped<ICredentialService, CredentialService>();
            builder.Services.AddScoped<ICardHolderService, CardHolderService>();
            builder.Services.AddScoped<IControlPointService, ControlPointService>();
            builder.Services.AddScoped<IHelperService, HelperService>();
            builder.Services.AddScoped<ILicenseService, LicenseService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();
            builder.Services.AddScoped<ILocationService,LocationService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IFeatureService, FeatureService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddScoped<ITriggerService, TriggerService>();
            builder.Services.AddScoped<IMonitorGroupService, MonitorGroupService>();
            builder.Services.AddScoped<ICommandService, CommandService>();
            builder.Services.AddScoped<ISettingService, SettingService>();

            // command Service
            //builder.Services.AddScoped<IAeroCommandService, BaseCommandService>();
            builder.Services.AddScoped<ITimeZoneCommandService, TimeZoneCommandService>();
            builder.Services.AddScoped<IHolidayCommandService, HolidayCommandService>();


            //

            builder.Services.AddScoped<MessageHandler>();

            
            builder.Services.AddScoped<SysService>();
            builder.Services.AddScoped(typeof(IHelperService<>), typeof(HelperService<>));

            
            builder.Services.AddScoped<CommandService>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IdReportService>();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            



            // Register AeroDriver
            builder.Services.AddSingleton<AeroCommandService>();
            builder.Services.AddSingleton<AeroMessage>();

            builder.Services.AddSingleton(
                Channel.CreateBounded<SCPReplyMessage>(
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
            var writeDriver = app.Services.GetRequiredService<AeroCommandService>();
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


            app.Lifetime.ApplicationStopping.Register(async () =>
            {
                var context = app.Services.GetRequiredService<AppDbContext>();

                // Delete all pending id report 
                context.id_report.RemoveRange(context.id_report.ToArray());

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
