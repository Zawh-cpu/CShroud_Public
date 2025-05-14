namespace CShroud.Infrastructure.Interfaces;

public interface IPlannedTask
{
    DateTime PlannedTime { get; set; }
    Task Action(IPlanner planner, DateTime currentTime);
}