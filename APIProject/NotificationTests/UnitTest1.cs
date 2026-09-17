using APIProject.Domain.Entities;
using APIProject.Domain.Enums;

namespace NotificationTests
{
    public class NotificationTests
    {
        [Fact]
        public void Notification_Should_Create_Pending_Notification()
        {
            var userId = Guid.NewGuid();

            // Act
            var notification = new Notification(userId, "Test", "Test", NotificationChannelEnum.Email, 
                NotificationPriorityEnum.High);

            // Assert
            Assert.NotEqual(Guid.Empty, notification.Id);
            Assert.Equal(userId, notification.UserId);
            Assert.Equal("Test", notification.Subject);
            Assert.Equal("Test", notification.Content);
            Assert.Equal(NotificationChannelEnum.Email, notification.Channel);
            Assert.Equal(NotificationPriorityEnum.High, notification.Priority);
            Assert.Equal(NotificationStatusEnum.Pending, notification.Status);
        }
    }
}