using FluentValidation;
using Shell.Application.Features.Directories.Validators;

namespace Shell.Application.Features.Directories.Commands.DeleteDirectory
{
    public class DeleteDirectoryCommandValidator : AbstractValidator<DeleteDirectoryCommand>
    {
        public DeleteDirectoryCommandValidator()
        {
            RuleFor(deleteDirectoryCommand => deleteDirectoryCommand.Path)
                .SetValidator(new DirectoryPathValidator());
        }
    }
}
