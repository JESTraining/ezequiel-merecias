using APIProject.Application.Commands.NotificationCommands;
using APIProject.Application.Commands.NotificationQueries;
using Azure.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {

        private readonly ISender _sender;
        private readonly IValidator<CreateNotificationCommand> _validator;

        public NotificationsController(ISender sender, IValidator<CreateNotificationCommand> validator) 
        { 
            _sender = sender;
            _validator = validator;
        }

        /// <summary>
        /// Create the notification 
        /// </summary>
        /// <remarks>
        ///     POST api/notifications
        ///     ```json
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "subject": "title",
        ///         "content": "message to send",
        ///         "channel": 1, Email = 1, Sms = 2, Push = 3, InApp = 4
        ///         "priority": 1, Low = 1, Medium = 2, High = 3, Critical = 4
        ///     }
        ///     ```
        /// </remarks>
        /// <param name="request">data to send as notification</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Id, Status and Date Created</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _sender.Send(request, cancellationToken);
            return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get the Notification
        /// </summary>
        /// <param name="id">Id of the notification</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Guid id, CancellationToken cancellationToken)
        {
            var query = new GetNotificationQuery(id);

            var result = await _sender.Send(query, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
