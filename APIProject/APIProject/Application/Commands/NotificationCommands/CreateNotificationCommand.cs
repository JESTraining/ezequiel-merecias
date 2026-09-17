using APIProject.Domain.Enums;
using MediatR;

namespace APIProject.Application.Commands.NotificationCommands
{
    public record CreateNotificationCommand(Guid UserId, string Subject, string Content, NotificationChannelEnum Channel, NotificationPriorityEnum Priority) : IRequest<CreateNotificationResult>;
}
