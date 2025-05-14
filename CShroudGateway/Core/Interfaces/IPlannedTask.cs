namespace CShroudGateway.Core.Interfaces;

public interface IPlannedTask
{
    DateTime PlannedTime { get; set; }
    Task ActionAsync(IPlanner planner, DateTime currentTime, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}