using APIProject.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Application.Commands.NotificationQueries
{
    public sealed class GetNotificationHandler : IRequestHandler<GetNotificationQuery, GetNotificationResult?>
    {
        private readonly NotificationDbContext _context;

        public GetNotificationHandler(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<GetNotificationResult?> Handle(
            GetNotificationQuery query, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .AsNoTracking() //we are only work with Select, we don't need recover Tracking
                .Where(x => x.Id == query.Id)
                .Select(x => new GetNotificationResult(
                    x.Id, x.UserId, x.Subject, x.Content, x.Channel,
                    x.Priority, x.Status, x.CreatedDate))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
