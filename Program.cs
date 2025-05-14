using CShroud.Core.Domain.Entities;
using CShroud.Core.Domain.Interfaces;
using CShroud.Core.Domain.Services;
using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Services;
using CShroud.Presentation.gRPC.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<VpnCoreConfig>(builder.Configuration.GetSection("VpnCore"));
builder.Services.Configure<GlobalParams>(builder.Configuration.GetSection("GlobalParams"));

builder.Services.Configure<BaseConfig>(builder.Configuration.GetSection("BaseConfig"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<VpnCoreConfig>>().Value);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<GlobalParams>>().Value);;
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<BaseConfig>>().Value);

builder.Services.AddSingleton<IUpdatePrimitive, UpdatePrimitive>();

builder.Services.AddSingleton<IProcessManager, ProcessManager>();
builder.Services.AddSingleton<IVpnCore, VpnCore>();
builder.Services.AddSingleton<IBaseRepository, BaseRepository>();
builder.Services.AddSingleton<IVpnRepository, VpnRepository>();

builder.Services.AddSingleton<IKeyService, KeyService>();
builder.Services.AddSingleton<IRateManager, RateManager>();
builder.Services.AddSingleton<ITelegramManager, TelegramManager>();

builder.Services.AddSingleton<IPlanner, Planner>();

builder.Services.AddSingleton<IProtocolHandlerFactory, ProtocolHandlerFactory>();
builder.Services.AddSingleton<ICore, Core>();

builder.Services.AddGrpc();

var app = builder.Build();

var apiV1Group = app.MapGroup("");
apiV1Group.MapGrpcService<ControlService>();
apiV1Group.MapGrpcService<MachineService>();
apiV1Group.MapGrpcService<UpdateService>();


// var vpnCoreConfig = builder.Configuration.GetSection("VpnCore");
// Console.WriteLine($"Path: {vpnCoreConfig["Path"]}, Link: {vpnCoreConfig["Link"]}");

var core = app.Services.GetRequiredService<ICore>();
core.Start();

app.Run();