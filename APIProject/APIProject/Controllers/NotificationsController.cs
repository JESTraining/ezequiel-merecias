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
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            var result = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get the Notification
        /// </summary>
        /// <param name="id"></param>
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
