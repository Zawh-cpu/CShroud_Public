using System.Text.Json;
using CShroud.Infrastructure.Data;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Tasks;

public class CheckPayments : IPlannedTask
{
    public DateTime PlannedTime { get; set; }
    
    private IBaseRepository _baseRepository;
    private IKeyService _keyService;
    private IRateManager _rateManager;
    private ITelegramManager _telegramManager;
    
    public CheckPayments(DateTime plannedTime, IBaseRepository baseRepository, IKeyService keyService, IRateManager rateManager, ITelegramManager telegramManager)
    {
        PlannedTime = plannedTime;
        _baseRepository = baseRepository;
        _keyService = keyService;
        _rateManager = rateManager;
        _telegramManager = telegramManager;
    }

    public virtual async Task Action(IPlanner planner, DateTime currentTime)
    {
        var dbContext = new ApplicationContext();
        var checkingTime = PlannedTime.AddDays(-4);
        
        var users = await _baseRepository.GetUsersPayedUntilAsync(dbContext, checkingTime);
        var triggeredDates = new List<int>() { 4, 2, 1};
        var results = new List<Dictionary<string, object>>();
        
        foreach (var user in users)
        {
            var different = user.PayedUntil! - currentTime;
            if (different.Value.Days < 0)
            {
                user.RateId = 1;
                user.PayedUntil = null;
                await _rateManager.UpdateRate(dbContext, user);
            }

            if (triggeredDates.Contains(different.Value.Days) || different.Value.Days < 0)
            {
                if (user.TelegramId != null)
                {
                    results.Add(new Dictionary<string, object>()
                    {
                        {"UserId", user.Id},
                        {"DaysLeft", different.Value.Days}
                    });
                }
            }
        }

        if (results.Any())
        {
            await dbContext.SaveChangesAsync();
            _telegramManager.RateNotification(JsonSerializer.Serialize(results));
        }
        
        PlannedTime = currentTime.AddHours(24);
        planner.AddTask(this);
    }
}