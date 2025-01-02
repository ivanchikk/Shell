using FluentValidation;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Drives.Commands.CreateDrive
{
    public class CreateDriveCommandValidator : AbstractValidator<CreateDriveCommand>
    {
        public CreateDriveCommandValidator()
        {
            RuleFor(createDriveCommand => createDriveCommand.Name)
                .SetValidator(new DriveNameValidator());
        }
    }
}
