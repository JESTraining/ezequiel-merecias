using APIProject.Domain.Entities;
using APIProject.Infraestructure.Persistance;
using MediatR;

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

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateNotificationResult(notification.Id, notification.Status, notification.CreatedDate);
        }
    }
}
