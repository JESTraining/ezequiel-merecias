using Microsoft.EntityFrameworkCore;
using NotificationDeliveryServices.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDeliveryServices.Data.Contexts
{
    public class DeliveryContext : DbContext
    {
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

        public DbSet<ProcessedMessage> ProcessedMessages => Set<ProcessedMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessedMessage>(x =>
            {
                x.ToTable("ProcessedMessages");
                x.HasKey(x => x.MessageId); //avoid duplication of notifications
                x.Property(x => x.ProcessedDate).IsRequired();
            });
        }

    }
}
