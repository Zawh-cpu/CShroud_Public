using System.Diagnostics;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Infrastructure.Services;

public class ProcessFactory : IProcessFactory
{
    private readonly IProcessManager _processManager;
    
    public ProcessFactory(IProcessManager processManager)
    {
        _processManager = processManager;
    }
    
    public BaseProcess Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None)
    {
        var process = new BaseProcess(processStartInfo, _processManager, debug);
        return process;
    }
}