using System.Diagnostics;
using CShroudDAW.Core.Configs;
using CShroudDAW.Infrastructure.Services;

namespace CShroudDAW.Application.Factories;

public interface IProcessFactory
{
    BaseProcess Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None);
}