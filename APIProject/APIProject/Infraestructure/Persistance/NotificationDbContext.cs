using APIProject.Domain.Entities;
using APIProject.Infraestructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Infraestructure.Persistance
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    }
}
