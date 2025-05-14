using CShroud.Infrastructure.Interfaces;
using CShroud.Infrastructure.Tasks;

namespace CShroud.Infrastructure.Services;

public class Core : ICore
{
    private readonly IBaseRepository _baseRepository;
    private readonly IRateManager _rateManager;
    private readonly IKeyService _keySerivce;
    private readonly ITelegramManager _telegramManager;
    private readonly IVpnCore _vpnCore;
    private readonly IPlanner _planner;
    public static string WorkingDir = Environment.CurrentDirectory;
    
    public Core(IBaseRepository baseRepository, IRateManager rateManager, IVpnCore vpnCore, IPlanner planner, IKeyService keySerivce, ITelegramManager telegramManager)
    {
        _baseRepository = baseRepository;
        _rateManager = rateManager;
        _vpnCore = vpnCore;
        _planner = planner;
        _keySerivce = keySerivce;
        _telegramManager = telegramManager;
    }

    public static string BuildPath(params string[] paths)
    {
        return Path.Combine(Environment.CurrentDirectory, Path.Combine(paths));
    }
    
    public void Initialize()
    {
    }

    public void Shutdown()
    {
    }

    public void Start()
    {
        _vpnCore.Start();
        var task = new TestTask(DateTime.UtcNow.AddSeconds(5), _vpnCore);
        _planner.AddTask(task);

        var checkPaymentsTask = new CheckPayments(DateTime.UtcNow.AddSeconds(5), _baseRepository, _keySerivce, _rateManager, _telegramManager);
        _planner.AddTask(checkPaymentsTask);
    }
}