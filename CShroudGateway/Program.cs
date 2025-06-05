using System.Text.Json.Serialization;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data;
using CShroudGateway.Infrastructure.Data.Config;
using CShroudGateway.Infrastructure.Data.Entities;
using CShroudGateway.Infrastructure.Services;
using CShroudGateway.Infrastructure.Tasks;
using CShroudGateway.Presentation.Api.v1.Hubs;
using CShroudGateway.Presentation.DeprecatedApi.gRPC.v1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

namespace CShroudGateway;

internal static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add configuration
        var authConfigRaw = builder.Configuration.GetSection("AuthConfig");
        builder.Services.Configure<AuthConfig>(authConfigRaw);
        
        var protocolsConfigRaw = builder.Configuration.GetSection("ProtocolsConfig");
        builder.Services.Configure<ProtocolsConfig>(protocolsConfigRaw);

        var authConfig = authConfigRaw.Get<AuthConfig>() ?? new AuthConfig();
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.UseSecurityTokenValidators = true;
    
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authConfig.JwtIssuer,
                ValidAudience = authConfig.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(authConfig.SecretKey)
            };
    
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var token = context.SecurityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                    if (token == null)
                    {
                        context.Fail("Invalid token.");
                        return;
                    }

                    var serviceScopeFactory = context.HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>();
    
                    using var scope = serviceScopeFactory.CreateScope();
                    var tokenService = scope.ServiceProvider.GetRequiredService<IBaseRepository>();

                    if (!Guid.TryParse(token.Id, out Guid tokenId))
                    {
                        context.Fail("Invalid token.");
                        return;
                    }

                    var tokenStatus = await tokenService.GetTokenByIdAsync(tokenId);
                    if (tokenStatus == null || tokenStatus.IsRevoked)
                    {
                        context.Fail("Revoked token.");
                        return;
                    }

                    if ((tokenStatus.TokenType == TokenType.Refresh && context.HttpContext.Request.Path != "/api/v1/auth/refresh")
                        || (tokenStatus.TokenType == TokenType.Action && context.HttpContext.Request.Path == "/api/v1/auth/refresh"))
                        context.Fail("You cannot use refresh token to do actions.");
                }
            };
        });
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString),
            ServiceLifetime.Scoped);

        builder.Services.AddHttpClient("TelegramHook",
            client => client.BaseAddress = new Uri("http://localhost:5216"));

        builder.Services.AddScoped<IBaseRepository, BaseRepository>();

        builder.Services.AddSingleton<IGrpcPool, GrpcPool>();
        builder.Services.AddSingleton<IUpdatePrimitive, UpdatePrimitive>();
        builder.Services.AddSingleton<INotificationManager, NotificationManager>();
        builder.Services.AddSingleton<IPlanner, Planner>();
        builder.Services.AddSingleton<IJwtService, JwtService>();
        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddScoped<IVpnRepository, VpnRepository>();
        builder.Services.AddScoped<IVpnService, VpnService>();
        builder.Services.AddScoped<IVpnKeyService, VpnKeyService>();
        builder.Services.AddScoped<IVpnServerManager, VpnServerManager>();
        builder.Services.AddScoped<IVpnStorage, VpnStorage>();
        builder.Services.AddScoped<IRateManager, RateManager>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddSingleton<IQuickAuthService, QuickAuthServices>();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        
        builder.Services.AddGrpc();
        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        /*.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });*/
        
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseReDoc(options => options.SpecUrl("/openapi/v1.json"));
            app.MapScalarApiReference();
        }

        // app.UseHttpsRedirection();
        app.MapGrpcService<ControlService>();
        app.MapGrpcService<UpdateService>();
        app.MapGrpcService<MachineService>();
        
        app.MapControllers();
        app.MapHub<QuickAuthHub>("/api/v1/quick-auth-hub");

        Task.WhenEach(CheckForReservedConstants(app.Services));
        RunTasks(app.Services);

        app.Run();


        return 0;
    }

    public static async Task CheckForReservedConstants(IServiceProvider serviceProvider)
    {
        var baseRepository = serviceProvider.GetRequiredService<IBaseRepository>();

        if (await baseRepository.GetUserByIdAsync(ReservedUsers.System) == null)
        {
            var user = new User()
            {
                Id = ReservedUsers.System,
                IsActive = true,
                Nickname = "System",
                IsVerified = true,
            };

            await baseRepository.AddWithSaveAsync(user);
        }
    }

    public static void RunTasks(IServiceProvider serviceProvider)
    {
        var planner = serviceProvider.GetRequiredService<IPlanner>();

        planner.AddTask(new PaymentsCheckTask(DateTime.UtcNow.AddMinutes(5)));
    }
}