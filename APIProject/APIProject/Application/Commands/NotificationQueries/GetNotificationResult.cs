using APIProject.Domain.Enums;

namespace APIProject.Application.Commands.NotificationQueries
{
    public sealed record GetNotificationResult(Guid Id, Guid UserId, string Subject, string Content, NotificationChannelEnum Channel,
        NotificationPriorityEnum Priority, NotificationStatusEnum Status, DateTime CreatedDate);
}
