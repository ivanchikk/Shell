using FluentValidation;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Drives.Commands.UpdateDrive
{
    public class UpdateDriveCommandValidator : AbstractValidator<UpdateDriveCommand>
    {
        public UpdateDriveCommandValidator()
        {
            RuleFor(updateDriveCommand => updateDriveCommand.Name)
                .NotEqual(updateDriveCommand => updateDriveCommand.NewName)
                .WithMessage("The new name must be different")
                .SetValidator(new DriveNameValidator());

            RuleFor(updateDriveCommand => updateDriveCommand.NewName)
                .SetValidator(new DriveNameValidator());
        }
    }
}
