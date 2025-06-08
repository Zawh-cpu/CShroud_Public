namespace CShroudApp.Core.Entities.User;

public class User
{
    public required Guid Id { get; set; } = Guid.Empty;
    public required string Nickname { get; set; } = "Unknown";
    public required Role Role { get; set; } = new Role();
    public required Rate Rate { get; set; } = new Rate();
    public required bool IsVerified { get; set; } = false;

    public static User Unauthenticated()
    {
        return new User()
        {
            Id = Guid.Empty,
            Nickname = "Unknown",
            Role = new Role(),
            Rate = new Rate(),
            IsVerified = false
        };
    }
}