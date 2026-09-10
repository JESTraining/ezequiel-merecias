using APIProject.Application.Commands.NotificationCommands;
using APIProject.Domain.Enums;
using MediatR;

namespace APIProject.Application.Commands.NotificationQueries
{
    public record GetNotificationQuery(Guid Id) : IRequest<GetNotificationResult?>;
}
