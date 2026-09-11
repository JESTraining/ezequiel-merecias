using NotificationDeliveryServices.Providers;
using NotificationDeliveryServices;
using NotificationDeliveryServices.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<INotificationProvider, EmailNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, SMSNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, PushNotificationProvider>();
builder.Services.AddSingleton<INotificationProvider, InAppNotificationProvider>();

builder.Services.AddSingleton<NotificationProviderResolver>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
