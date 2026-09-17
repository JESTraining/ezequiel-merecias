using APIProject.Domain.Enums;

namespace APIProject.Application.Commands.NotificationCommands
{
    public record CreateNotificationResult(Guid Id, NotificationStatusEnum Status, DateTime CreateDate);
}
