using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<WebhookEvent> WebhookEvents { get; set; } = default!;
}