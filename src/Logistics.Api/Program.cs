using Delivery.Application.Commands;
using Delivery.Application.Queries;
using Delivery.Infrastructure;
using Fleet.Application.Commands;
using Fleet.Infrastructure;
using Logistics.Api.RealTime;
using Logistics.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<LogisticsDbContext>(o =>
    o.UseNpgsql(cs, npgsql => npgsql.UseNetTopologySuite()));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateDeliveryCommand>();
    cfg.RegisterServicesFromAssemblyContaining<IngestLocationCommand>();
});

builder.Services.AddScoped<Delivery.Application.Contracts.IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<Fleet.Application.Contracts.IFleetRepository, FleetRepository>();

builder.Services.AddSingleton<IVehicleLocationBroadcaster, SignalRVehicleLocationBroadcaster>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHub<FleetHub>("/hubs/fleet");

app.Run();
