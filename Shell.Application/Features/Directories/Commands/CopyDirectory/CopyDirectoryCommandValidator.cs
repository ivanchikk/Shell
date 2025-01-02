using FluentValidation;
using Shell.Application.Features.Directories.Validators;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Directories.Commands.CopyDirectory
{
    public class CopyDirectoryCommandValidator : AbstractValidator<CopyDirectoryCommand>
    {
        public CopyDirectoryCommandValidator()
        {
            RuleFor(copyDirectoryCommand => copyDirectoryCommand.Path)
                .NotEqual(copyDirectoryCommand => copyDirectoryCommand.NewPath)
                .WithMessage("The new path must be different")
                .SetValidator(new DirectoryPathValidator());

            RuleFor(copyDirectoryCommand => copyDirectoryCommand.NewPath)
                .SetValidator(new DirectoryPathValidator())
                .Unless(copyDirectoryCommand => new DriveNameValidator().Validate(copyDirectoryCommand.NewPath).IsValid);
        }
    }
}
