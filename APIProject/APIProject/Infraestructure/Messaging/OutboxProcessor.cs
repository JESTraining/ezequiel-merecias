
using APIProject.Infraestructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Infraestructure.Messaging
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<OutboxProcessor> logger;

        public OutboxProcessor(IServiceScopeFactory _serviceScopeFactory, ILogger<OutboxProcessor> _logger)
        {
            serviceScopeFactory = _serviceScopeFactory;
            logger = _logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) //only when app is executing
            {
                try
                {
                    await ProcessOutboxMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error ocurred while processing outbox messages");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Checking for pending outbox messages");
            using var scope = serviceScopeFactory.CreateScope(); //Create new scope to execute the backgroundjob

            var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

            var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

            //search only pending messages by 100 batch
            var messages = await context.OutboxMessages.Where(x => x.Processed == null)
                                                        .OrderBy(x => x.Ocurred)
                                                        .Take(100)
                                                        .ToListAsync(cancellationToken);
            foreach( var message in messages )
            {
                try
                {
                    await publisher.PublishAsync(message.Type, message.Payload, cancellationToken);

                    message.Processed = DateTime.Now;
                    message.Error = null;

                    logger.LogInformation("message published Id:{MessageId} Type:{MessageType}", message.Id, message.Type);
                }
                catch(Exception ex)
                {
                    message.Error = ex.Message;

                    logger.LogError("message failed Id:{MessageId} Type:{MessageType}", message.Id, message.Type);
                }

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
