namespace APIProject.Domain.Events
{
    public sealed record NotificationDeliveredEvent(Guid MessageId, Guid NotificationId, DateTime DeliveredDate);
}
