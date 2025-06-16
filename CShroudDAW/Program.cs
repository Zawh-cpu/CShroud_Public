// DEMOCRATIC ACCESS WALL

using System.Text.Json;
using CShroudDAW;
using CShroudDAW.Application.Factories;
using CShroudDAW.Core.Configs;
using CShroudDAW.Core.Interfaces;
using CShroudDAW.Core.JsonContexts;
using CShroudDAW.Core.Utils;
using CShroudDAW.Infrastructure.Cores.Xray.Services;
using CShroudDAW.Infrastructure.Services;
using KeyService = CShroudDAW.Presentation.Services.KeyService;

FileChecker.CheckFiles();

var builder = WebApplication.CreateBuilder(args);

ApplicationConfig cfg;
try
{
    cfg = JsonSerializer.Deserialize<ApplicationConfig>(File.ReadAllText(AppConstants.ConfigFilePath),
        ConfigsJsonContext.Default.ApplicationConfig)!;
}
catch(Exception)
{
    cfg = new ApplicationConfig();
}
builder.Services.AddSingleton(cfg);

builder.Services.AddSingleton<IProcessManager, ProcessManager>();
builder.Services.AddTransient<IProcessFactory, ProcessFactory>();
builder.Services.AddSingleton<IVpnKeyService, VpnKeyService>();
builder.Services.AddSingleton<IGrpcPool, GrpcPool>();
builder.Services.AddSingleton<ISyncService, SyncService>();

switch (cfg.Vpn.RuntimeCore)
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
_ = Task.Run(async () =>
{
    await Task.Delay(7000);
    Console.WriteLine("[SYNC] Sync key process has been started.");
    try 
    {
        var res = await syncService.SyncKeys(cfg.GatewayAddress, cfg.SecretKey);
        if (res.IsSuccess)
            Console.WriteLine($"[SYNC] Sync keys completed. Successfully synced {res.Value.KeysSynced}/{res.Value.KeysCount} keys.");
        else
            Console.WriteLine($"[SYNC] Sync keys failed.");
    } catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();