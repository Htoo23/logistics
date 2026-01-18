using Analytics.Domain;
using BuildingBlocks.Infrastructure;
using Delivery.Domain;
using Fleet.Domain;
using Microsoft.EntityFrameworkCore;
using Notifications.Domain;
using Routing.Domain;

namespace Logistics.Persistence;

public sealed class LogisticsDbContext : AppDbContextBase
{
    public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options) {}

    public DbSet<DeliveryAggregate> Deliveries => Set<DeliveryAggregate>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleLocation> VehicleLocations => Set<VehicleLocation>();
    public DbSet<RoutePlan> RoutePlans => Set<RoutePlan>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();
    public DbSet<KpiSnapshot> KpiSnapshots => Set<KpiSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeliveryAggregate>(b =>
        {
            b.ToTable("deliveries");
            b.HasKey(x => x.Id);
            b.Property(x => x.TrackingNumber).HasColumnName("tracking_number").IsRequired();
            b.HasIndex(x => x.TrackingNumber).IsUnique();
            b.Property(x => x.Status).HasColumnName("status");
            b.OwnsOne(x => x.Pod, pod =>
            {
                pod.Property(x => x.DeliveredAt).HasColumnName("pod_delivered_at");
                pod.Property(x => x.RecipientName).HasColumnName("pod_recipient_name");
                pod.Property(x => x.PhotoUrl).HasColumnName("pod_photo_url");
                pod.Property(x => x.Notes).HasColumnName("pod_notes");
            });
            b.Navigation(x => x.Pod).IsRequired(false);
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Driver>(b =>
        {
            b.ToTable("drivers");
            b.HasKey(x => x.Id);
            b.Property(x => x.FullName).HasColumnName("full_name").IsRequired();
            b.Property(x => x.Phone).HasColumnName("phone").IsRequired();
            b.Property(x => x.Active).HasColumnName("active");
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Vehicle>(b =>
        {
            b.ToTable("vehicles");
            b.HasKey(x => x.Id);
            b.Property(x => x.PlateNumber).HasColumnName("plate_number").IsRequired();
            b.HasIndex(x => x.PlateNumber).IsUnique();
            b.Property(x => x.CapacityKg).HasColumnName("capacity_kg");
            b.Property(x => x.CapacityM3).HasColumnName("capacity_m3");
            b.Property(x => x.Active).HasColumnName("active");
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<VehicleLocation>(b =>
        {
            b.ToTable("vehicle_locations");
            b.HasKey(x => x.Id);
            b.Property(x => x.VehicleId).HasColumnName("vehicle_id");
            b.Property(x => x.RecordedAt).HasColumnName("recorded_at");
            b.Property(x => x.Position).HasColumnName("geom").HasColumnType("geometry(Point,4326)");
            b.Property(x => x.SpeedKph).HasColumnName("speed_kph");
        });

        modelBuilder.Entity<RoutePlan>(b =>
        {
            b.ToTable("route_plans");
            b.HasKey(x => x.Id);
            b.Property(x => x.RouteCode).HasColumnName("route_code").IsRequired();
            b.Property(x => x.ServiceDate).HasColumnName("service_date");
            b.Property(x => x.VehicleId).HasColumnName("vehicle_id");
            b.Property(x => x.DriverId).HasColumnName("driver_id");
            b.OwnsMany(x => x.Stops, sb =>
            {
                sb.ToTable("route_stops");
                sb.WithOwner().HasForeignKey("route_plan_id");
                sb.HasKey("Id");
                sb.Property(x => x.DeliveryId).HasColumnName("delivery_id");
                sb.Property(x => x.Sequence).HasColumnName("sequence");
                sb.Property(x => x.Eta).HasColumnName("eta");
            });
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<Zone>(b =>
        {
            b.ToTable("zones");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasColumnName("name").IsRequired();
            b.Property(x => x.Area).HasColumnName("area").HasColumnType("geometry(Polygon,4326)");
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<NotificationLog>(b =>
        {
            b.ToTable("notification_logs");
            b.HasKey(x => x.Id);
            b.Property(x => x.Channel).HasColumnName("channel");
            b.Property(x => x.Recipient).HasColumnName("recipient");
            b.Property(x => x.TemplateKey).HasColumnName("template_key");
            b.Property(x => x.PayloadJson).HasColumnName("payload_json");
            b.Property(x => x.CreatedAt).HasColumnName("created_at");
            b.Property(x => x.SentAt).HasColumnName("sent_at");
            b.Property(x => x.Status).HasColumnName("status");
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<KpiSnapshot>(b =>
        {
            b.ToTable("kpi_snapshots");
            b.HasKey(x => x.Id);
            b.Property(x => x.Day).HasColumnName("day");
            b.Property(x => x.DeliveriesPlanned).HasColumnName("deliveries_planned");
            b.Property(x => x.DeliveriesCompleted).HasColumnName("deliveries_completed");
            b.Property(x => x.OnTimeRatio).HasColumnName("on_time_ratio");
            b.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<OutboxMessage>(b =>
        {
            b.ToTable("outbox_messages");
            b.HasKey(x => x.Id);
            b.Property(x => x.Type).HasColumnName("type");
            b.Property(x => x.Payload).HasColumnName("payload");
            b.Property(x => x.OccurredAt).HasColumnName("occurred_at");
            b.Property(x => x.Processed).HasColumnName("processed");
            b.Property(x => x.ProcessedAt).HasColumnName("processed_at");
        });
    }
}
