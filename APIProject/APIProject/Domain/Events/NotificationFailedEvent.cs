namespace APIProject.Domain.Events
{
    public sealed record NotificationFailedEvent(Guid MessageId, Guid NotificationId, string Error, DateTime FailedDate);
}
