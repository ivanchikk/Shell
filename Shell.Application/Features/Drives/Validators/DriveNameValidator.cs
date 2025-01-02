using FluentValidation;

namespace Shell.Application.Features.Drives.Validators
{
    public class DriveNameValidator : AbstractValidator<string>
    {
        public DriveNameValidator()
        {
            RuleFor(name => name)
                .Matches(@"^[A-Z]:\\$")
                .WithMessage("{PropertyValue} Drive name is invalid.");
        }
    }
}
