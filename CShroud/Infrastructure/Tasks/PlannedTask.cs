using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Tasks;

public class PlannedTask
{
    public DateTime PlannedTime;

    public PlannedTask(DateTime plannedTime)
    {
        PlannedTime = plannedTime;
    }

    public virtual async Task Action(IPlanner planner, DateTime currentTime)
    {
    }
}