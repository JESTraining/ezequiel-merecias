using APIProject.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace APIProject.Application.Commands.NotificationQueries
{
    public sealed class GetNotificationHandler : IRequestHandler<GetNotificationQuery, GetNotificationResult?>
    {
        private readonly NotificationDbContext context;
        private readonly IConnectionMultiplexer redis;

        public GetNotificationHandler(NotificationDbContext _context, IConnectionMultiplexer _redis)
        {
            context = _context;
            redis = _redis;
        }

        public async Task<GetNotificationResult?> Handle(GetNotificationQuery query, CancellationToken cancellationToken)
        {
            var redisDb = redis.GetDatabase();
            var cacheKey = $"notification-{query.Id}";
            var cachedNotification = await redisDb.StringGetAsync(cacheKey);

            if (cachedNotification.HasValue)
            {
                return JsonSerializer.Deserialize<GetNotificationResult>(cachedNotification.ToString());
            }

            var notification = await context.Notifications
                .AsNoTracking() //we are only work with Select, we don't need recover Tracking
                .Where(x => x.Id == query.Id)
                .Select(x => new GetNotificationResult(
                    x.Id, x.UserId, x.Subject, x.Content, x.Channel,
                    x.Priority, x.Status, x.CreatedDate))
                .FirstOrDefaultAsync(cancellationToken);

            if (notification != null)
            {
                await redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(notification), TimeSpan.FromMinutes(5.0D));
            }

            return notification;
        }
    }
}
