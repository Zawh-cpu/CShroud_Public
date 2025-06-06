using CShroudApp.Core.Interfaces;

namespace CShroudApp.Infrastructure.Tasks;

public class TestTask : IPlannedTask
{
    public DateTime PlannedTime { get; set; }
    private IVpnCore _vpnCore;

    public TestTask(DateTime plannedTime, IVpnCore vpnCore)
    {
        PlannedTime = plannedTime;
        _vpnCore = vpnCore;
    }

    public async Task ActionAsync(IPlanner planner, DateTime currentTime, IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        if (_vpnCore.IsRunning == false)
        {
            Console.WriteLine("VpnCore strangely is not running. Trying to restart it.");
        }
        planner.AddTask(new TestTask(currentTime.AddMinutes(15), _vpnCore));
    }
}