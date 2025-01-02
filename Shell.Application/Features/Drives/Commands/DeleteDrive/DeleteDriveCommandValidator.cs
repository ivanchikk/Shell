using FluentValidation;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Drives.Commands.DeleteDrive
{
    public class DeleteDriveCommandValidator : AbstractValidator<DeleteDriveCommand>
    {
        public DeleteDriveCommandValidator()
        {
            RuleFor(deleteDriveCommand => deleteDriveCommand.Name)
                .SetValidator(new DriveNameValidator());
        }
    }
}
