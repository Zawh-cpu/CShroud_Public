using System.Diagnostics;
using CShroudDAW.Infrastructure.Data.Config;
using CShroudDAW.Infrastructure.Services;

namespace CShroudDAW.Application.Factories;

public interface IProcessFactory
{
    BaseProcess Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None);
}