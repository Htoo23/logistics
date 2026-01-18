using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace BuildingBlocks.Infrastructure;

public abstract class AppDbContextBase : DbContext
{
    protected AppDbContextBase(DbContextOptions options) : base(options) {}

    public DbSet<OutboxMessage> Outbox => Set<OutboxMessage>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      
        var domainEvents = ChangeTracker.Entries<Entity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var evt in domainEvents)
        {
            Outbox.Add(OutboxMessage.FromDomainEvent(evt));
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entry in ChangeTracker.Entries<Entity>())
            entry.Entity.ClearDomainEvents();

        return result;
    }
}
