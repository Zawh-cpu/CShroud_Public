using System.Text.Json;
using System.Text.Json.Nodes;
using CShroudApp;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;
using CShroudApp.Core.Utils;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.VpnCores.SingBox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

void Unauthenticated(object? sender, EventArgs e)
{
    Console.WriteLine("Unauthenticated");
}

Console.WriteLine(AppConstants.ConfigFilePath);
FileChecker.CheckFiles();

var builder = new HostApplicationBuilder(args);
builder.Logging.AddConsole();

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

builder.Services.AddHttpClient("CrimsonShroudApiHook",
    client => client.BaseAddress = new Uri(cfg.Network.ReservedGatewayAddresses.First()));

builder.Services.AddSingleton<ApplicationConfig>(cfg);
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IApiRepository, ApiRepository>();
builder.Services.AddSingleton<IStorageManager, StorageManager>();
builder.Services.AddSingleton<ISessionManager, SessionManager>();
builder.Services.AddSingleton<INotificationManager, NotificationManager>();
builder.Services.AddSingleton<IProcessManager, ProcessManager>();
builder.Services.AddSingleton<ProcessFactory>();
builder.Services.AddSingleton<IInternalDataManager, InternalDataManager>();
builder.Services.AddSingleton<IVpnService, VpnService>();

builder.Services.AddSingleton<IVpnCore, SingBoxCore>();

var app = builder.Build();

var service = app.Services.GetRequiredService<ISessionManager>();
var service2 = app.Services.GetRequiredService<IApiRepository>();

var dto = Task.Run(async () => await service2.SignInAsync("123", "123")).Result;
if (dto.IsSuccess)
{
    service.RefreshToken = dto.Value.RefreshToken;
    service.ActionToken = dto.Value.ActionToken;
}
service.UnauthorizedSession += Unauthenticated;

Task.WaitAll(test.PseudoMain(app.Services));

while (true)
{
    Task.Delay(10000).Wait();
}
