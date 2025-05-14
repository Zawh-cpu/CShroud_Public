using System.Collections;
using System.Text.Json;
using CShroudGateway.Core.Constants;
using CShroudGateway.Core.Interfaces;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Infrastructure.Tasks;


public class PaymentsCheckTask : IPlannedTask
{
    public HashSet<int> triggeredDates = new() { 0, 1, 3 };
    public DateTime PlannedDate { get; private set; }
    
    
    public PaymentsCheckTask(DateTime plannedTime)
    {
        PlannedDate = plannedTime;
    }

    public virtual async Task ActionAsync(IPlanner planner, DateTime currentTime, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository>();
        var rateManager = scope.ServiceProvider.GetRequiredService<IRateManager>();
        var notifyManager = scope.ServiceProvider.GetRequiredService<INotificationManager>();

        var baseRate = await baseRepository.GetFirstDefaultRateAsync();
        if (baseRate is null)
        {
            PlannedTime = currentTime.AddHours(24);
            planner.AddTask(this);
            throw new NullReferenceException("Base repository returned null");
        }
        
        var users = await baseRepository.GetUsersPayedUntilAsync(x => currentTime.AddDays(-3) <= x.PayedUntil && x.PayedUntil <= currentTime);
        var expiredUsers = await baseRepository.GetUsersPayedUntilAsync(x => x.PayedUntil <= currentTime.AddDays(1), x => x.Include(u => u.Keys));

        var notifiesLift = new List<Mail>();

        foreach (var user in expiredUsers)
        {
            user.RateId = baseRate.Id;
            user.Rate = baseRate;
            user.PayedUntil = null;
            await rateManager.ChangeRateAsync(user, saveChanges: false);
            notifiesLift.Add(new Mail()
            {
                Type = MailType.RateExpired,
                SenderId = ReservedUsers.System,
                RecipientId = user.Id
            });
        }

        if (users.Any())
            await baseRepository.SaveContextAsync();

        foreach (var user in users)
        {
            if (user.PayedUntil is null ) continue;

            notifiesLift.Add(new Mail()
            {
                Type = MailType.RateExpiration,
                SenderId = ReservedUsers.System,
                RecipientId = user.Id,
                ExtraData = JsonSerializer.SerializeToDocument(new Dictionary<string, object>()
                {
                    ["DaysLeft"] = user.PayedUntil - currentTime
                })
            });
        }

        if (expiredUsers.Any() || notifiesLift.Any())
        {
            // And notifies and save the whole context
            await baseRepository.AddRangeAsync(notifiesLift);
            
            notifyManager.CallAndForget(notifiesLift);
        }
        
        PlannedTime = currentTime.AddHours(24);
        planner.AddTask(this);
    }

    public DateTime PlannedTime { get; set; }
}