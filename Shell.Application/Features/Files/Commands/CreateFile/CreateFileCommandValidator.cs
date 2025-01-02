using FluentValidation;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        public CreateFileCommandValidator()
        {
            RuleFor(createFileCommand => createFileCommand.Path)
                .SetValidator(new FilePathValidator());
        }
    }
}
