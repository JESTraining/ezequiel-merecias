using APIProject.Application.Commands.NotificationCommands;
using FluentValidation;

namespace APIProject.Validators.Notifications
{
    public class CreateNotificationValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.Channel).IsInEnum();
            RuleFor(x => x.Priority).IsInEnum();
        }
    }
}
