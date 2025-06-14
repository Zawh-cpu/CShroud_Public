using System.Diagnostics;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Processes;

namespace CShroudApp.Application.Factories;

public class ProcessFactory
{
    private readonly IProcessManager _processManager;
    
    public ProcessFactory(IProcessManager processManager)
    {
        _processManager = processManager;
    }
    
    public BaseProcess Create(ProcessStartInfo processStartInfo, LogLevelMode debug = LogLevelMode.Off)
    {
        var process = new BaseProcess(processStartInfo, _processManager, debug);
        return process;
    }
}