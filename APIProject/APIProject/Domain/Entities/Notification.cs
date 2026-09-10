using APIProject.Domain.Enums;

namespace APIProject.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
        public NotificationChannelEnum Channel { get; private set; }
        public NotificationPriorityEnum Priority { get; private set; }
        public NotificationStatusEnum Status { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public Notification(Guid userId, string subject, string content, NotificationChannelEnum channel, NotificationPriorityEnum priority)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Subject = subject;
            Content = content;
            Channel = channel;
            CreatedDate = DateTime.Now;
            Priority = priority;
            Status = NotificationStatusEnum.Pending;
        }

    }
}
