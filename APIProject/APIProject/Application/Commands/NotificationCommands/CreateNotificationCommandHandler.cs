using APIProject.Domain.Entities;
using APIProject.Domain.Events;
using APIProject.Infraestructure.Messaging;
using APIProject.Infraestructure.Persistance;
using MediatR;
using System.Text.Json;

namespace APIProject.Application.Commands.NotificationCommands
{
    public sealed class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationResult>
    {
        private readonly NotificationDbContext _context;

        public CreateNotificationCommandHandler(NotificationDbContext context) 
        {
            _context = context;
        }

        public async Task<CreateNotificationResult> Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
        {
            var notification = new Notification(command.UserId, command.Subject, command.Content, command.Channel, command.Priority);
            var notificationEvent = new NotificationCreatedEvent(Guid.NewGuid(), notification.Id, notification.UserId, notification.Subject, notification.Content,
                notification.Channel, notification.Priority, notification.CreatedDate);

            var outbox = new OutboxMessage()
            {
                Id = notificationEvent.MessageId,
                Type = "notification.created",
                Payload = JsonSerializer.Serialize(notificationEvent),
                Ocurred = DateTime.Now
            };

            _context.Notifications.Add(notification);
            _context.OutboxMessages.Add(outbox);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateNotificationResult(notification.Id, notification.Status, notification.CreatedDate);
        }
    }
}
