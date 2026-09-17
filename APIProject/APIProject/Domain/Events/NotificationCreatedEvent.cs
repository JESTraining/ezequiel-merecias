using APIProject.Domain.Enums;

namespace APIProject.Domain.Events
{
    public sealed record NotificationCreatedEvent(Guid MessageId, Guid NotificationId, Guid UserId, string subject, string Content, NotificationChannelEnum Channel,
        NotificationPriorityEnum Priority, DateTime CreatedDate);
}
