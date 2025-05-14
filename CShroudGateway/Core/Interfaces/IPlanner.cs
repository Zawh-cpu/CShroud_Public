namespace CShroudGateway.Core.Interfaces;

public interface IPlanner
{
    void AddTask(IPlannedTask task);
    void ClearQueue();
    void RemoveTask(IPlannedTask task);
}