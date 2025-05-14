namespace CShroud.Infrastructure.Interfaces;

public interface IProcessManager
{
    void Register(IProcess process);
    void KillAll();
}