namespace CShroudDAW.Core.Interfaces;

public interface IProcessManager
{
    void Register(IProcess process);
    void KillAll();
    
    Task KillAllAsync();
}