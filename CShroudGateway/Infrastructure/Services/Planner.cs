using CShroudGateway.Core.Interfaces;

namespace CShroudGateway.Infrastructure.Services;

public class Planner : IPlanner
{
    private List<IPlannedTask> _plannedTasks = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly IServiceProvider _serviceProvider;
    
    public Task RuntimeTask;

    public Planner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RuntimeTask = Task.CompletedTask;
    }

    public void AddTask(IPlannedTask task)
    {
        if (_plannedTasks.Count > 0 && task.PlannedTime < _plannedTasks[0].PlannedTime)
        {
            _cts.Cancel();
        }
        
        _plannedTasks.Add(task);
        _plannedTasks.Sort((item1, item2) => item1.PlannedTime.CompareTo(item2.PlannedTime));
        
        if (RuntimeTask.IsCompleted)
        {
            RuntimeTask = Task.Run(CheckLoop);
        }
    }

    public void ClearQueue()
    {
        _cts.Cancel();
        _plannedTasks.Clear();
    }

    public void RemoveTask(IPlannedTask task)
    {
        if (!_plannedTasks.Contains(task)) return;
        
        _cts.Cancel();
        _plannedTasks.Remove(task);
        RuntimeTask = Task.Run(CheckLoop);
    }
    
    private async Task CheckLoop()
    {
        try
        {
            while (this._plannedTasks.Count > 0)
            {
                DateTime currentTime = DateTime.UtcNow;
                int delay = 0;
                int startCount = _plannedTasks.Count;

                foreach (var task in _plannedTasks.ToList())
                {
                    if (task.PlannedTime > currentTime)
                    {
                        delay = (int)task.PlannedTime.Subtract(currentTime).TotalMilliseconds;
                        break;
                    }

                    _plannedTasks.Remove(task);
                    try
                    {
                        await task.ActionAsync(this, currentTime, _serviceProvider, default);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine(ex.Message);
                    }
                    
                }

                if (_plannedTasks.Count != startCount)
                {
                    _plannedTasks.Sort((item1, item2) => item1.PlannedTime.CompareTo(item2.PlannedTime));
                }
                
                if (delay > 0)
                {
                    await Task.Delay(delay, _cts.Token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task cancelled");
        }
    }
}