using FluentValidation;
using Shell.Application.Features.Directories.Validators;

namespace Shell.Application.Features.Directories.Commands.CreateDirectory
{
    public class CreateDirectoryCommandValidator : AbstractValidator<CreateDirectoryCommand>
    {
        public CreateDirectoryCommandValidator()
        {
            RuleFor(createDirectoryCommand => createDirectoryCommand.Path)
                .SetValidator(new DirectoryPathValidator());
        }
    }
}
