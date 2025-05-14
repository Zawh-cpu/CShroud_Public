using CShroudApp.Core.Interfaces;

namespace CShroudApp.Core.Interfaces;

public interface IPlannedTask
{
    DateTime PlannedTime { get; set; }
    Task Action(IPlanner planner, DateTime currentTime);
}