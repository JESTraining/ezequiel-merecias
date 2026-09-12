using NotificationDeliveryServices.Providers;
using NotificationDeliveryServices;
using NotificationDeliveryServices.Messaging;
using NotificationDeliveryServices.Data.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<RabbitMqEventPublisher>();

builder.Services.AddDbContext<DeliveryContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<INotificationProvider, EmailNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, SMSNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, PushNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, InAppNotificationProvider>();

builder.Services.AddSingleton<NotificationProviderResolver>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
