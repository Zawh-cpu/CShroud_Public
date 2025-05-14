// DEMOCRATIC ACCESS WALL
using CShroudDAW.Application.Factories;
using CShroudDAW.Core.Interfaces;
using CShroudDAW.Infrastructure.Cores.Xray.Services;
using CShroudDAW.Infrastructure.Data.Config;
using CShroudDAW.Infrastructure.Services;
using CShroudDAW.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection("Settings"));

ApplicationConfig config = builder.Configuration.Get<ApplicationConfig>()!;

builder.Services.AddSingleton<IProcessManager, ProcessManager>();
builder.Services.AddTransient<IProcessFactory, ProcessFactory>();
builder.Services.AddSingleton<IVpnKeyService, VpnKeyService>();

switch (config.Vpn.RuntimeCode)
{
    case VpnRuntimeCore.Xray:
        builder.Services.AddSingleton<IVpnCore, VpnCore>();
        builder.Services.AddSingleton<IVpnRepository, VpnRepository>();
        break;
    default:
        throw new NotSupportedException("Unsupported Runtime Core");
}

builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<KeyService>();

var vpnCore = app.Services.GetRequiredService<IVpnCore>();
vpnCore.Enable();

app.Run();