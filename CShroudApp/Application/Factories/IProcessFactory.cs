using System.Diagnostics;
using CShroudApp.Infrastructure.Data.Config;
using CShroudApp.Infrastructure.Services;

namespace CShroudApp.Application.Factories;

public interface IProcessFactory
{
    BaseProcess Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None);
}