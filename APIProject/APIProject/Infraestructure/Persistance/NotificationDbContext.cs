using APIProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Infraestructure.Persistance
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications => Set<Notification>();
    }
}
