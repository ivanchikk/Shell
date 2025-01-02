using FluentValidation;
using Shell.Application.Features.Drives.Validators;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Commands.CopyFile
{
    public class CopyFileCommandValidator : AbstractValidator<CopyFileCommand>
    {
        public CopyFileCommandValidator()
        {
            RuleFor(copyFileCommand => copyFileCommand.Path)
                .NotEqual(copyFileCommand => copyFileCommand.NewPath)
                .WithMessage("The new path must be different")
                .SetValidator(new FilePathValidator());

            RuleFor(copyFileCommand => copyFileCommand.NewPath)
                .SetValidator(new FilePathValidator())
                .Unless(copyFileCommand => new DriveNameValidator().Validate(copyFileCommand.NewPath).IsValid); ;
        }
    }
}
