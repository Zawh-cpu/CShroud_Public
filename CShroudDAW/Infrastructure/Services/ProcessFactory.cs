using System.Diagnostics;
using CShroudDAW.Application.Factories;
using CShroudDAW.Core.Interfaces;
using CShroudDAW.Infrastructure.Data.Config;

namespace CShroudDAW.Infrastructure.Services;

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