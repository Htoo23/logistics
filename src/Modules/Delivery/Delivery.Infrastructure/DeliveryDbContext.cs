using BuildingBlocks.Infrastructure;
using Delivery.Domain;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure;

public sealed class DeliveryDbContext : AppDbContextBase
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) {}

    public DbSet<DeliveryAggregate> Deliveries => Set<DeliveryAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveryAggregate>(b =>
        {
            b.ToTable("deliveries");
            b.HasKey(x => x.Id);
            b.Property(x => x.TrackingNumber).HasMaxLength(64).IsRequired();
            b.HasIndex(x => x.TrackingNumber).IsUnique();

            b.OwnsOne(x => x.Pod, pod =>
            {
                pod.Property(p => p.RecipientName).HasColumnName("pod_recipient_name");
                pod.Property(p => p.DeliveredAt).HasColumnName("pod_delivered_at");
                pod.Property(p => p.PhotoUrl).HasColumnName("pod_photo_url");
                pod.Property(p => p.Notes).HasColumnName("pod_notes");
            });

            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<OutboxMessage>().ToTable("outbox");
    }
}
