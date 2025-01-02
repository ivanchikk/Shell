using FluentValidation;
using Shell.Application.Features.Directories.Validators;

namespace Shell.Application.Features.Directories.Commands.UpdateDirectory
{
    public class UpdateDirectoryCommandValidator : AbstractValidator<UpdateDirectoryCommand>
    {
        public UpdateDirectoryCommandValidator()
        {
            RuleFor(updateDirectoryCommand => updateDirectoryCommand.Path)
                .NotEqual(updateDirectoryCommand => updateDirectoryCommand.NewPath)
                .WithMessage("The new path must be different")
                .SetValidator(new DirectoryPathValidator());

            RuleFor(updateDirectoryCommand => updateDirectoryCommand.NewPath)
                .SetValidator(new DirectoryPathValidator());
        }
    }
}
