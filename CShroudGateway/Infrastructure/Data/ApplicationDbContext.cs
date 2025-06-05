


using CShroudGateway.Core.Entities;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CShroudGateway.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<ProtocolSettings> ProtocolsSettings { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<Mail> Mails { get; set; }
    
    public DbSet<Token> Tokens { get; set; }
    public DbSet<LoginHistory> LoginHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProtocolSettings>()
            .HasKey(x => new { x.ServerId, x.Protocol });
        
        /*var listToArrayConverter = new ValueConverter<List<VpnProtocol>, string[]>(
            v => v.Select(e => e.ToString()).ToArray(),
            v => v.Select(e => Enum.Parse<VpnProtocol>(e)).ToList());
        
        modelBuilder.Entity<Server>(entity =>
        {
            var property = entity.Property(e => e.SupportedProtocols);
            property.HasConversion(listToArrayConverter);
            property.Metadata.SetValueComparer(listComparer); // Вызов отдельно
            property.HasColumnType("text[]"); // PostgreSQL массив строк
        });*/
    }
}