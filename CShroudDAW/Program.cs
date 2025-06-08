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
builder.Services.AddSingleton<IGrpcPool, GrpcPool>();
builder.Services.AddSingleton<ISyncService, SyncService>();

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

var syncService = app.Services.GetRequiredService<ISyncService>();
Task.Run(async () =>
{
    await Task.Delay(7000);
    var res = await syncService.SyncKeys(config.GatewayAddress, config.SecretKey);
    if (res.IsSuccess)
        Console.WriteLine($"[SYNC] Sync keys completed. Successfully synced {res.Value.KeysSynced}/{res.Value.KeysCount} keys.");
    else
        Console.WriteLine($"[SYNC] Sync keys failed.");
});

app.Run();