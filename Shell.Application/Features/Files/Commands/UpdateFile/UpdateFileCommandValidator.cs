using FluentValidation;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Commands.UpdateFile
{
    public class UpdateFileCommandValidator : AbstractValidator<UpdateFileCommand>
    {
        public UpdateFileCommandValidator()
        {
            RuleFor(updateFileCommand => updateFileCommand.Path)
                .NotEqual(updateFileCommand => updateFileCommand.NewPath)
                .WithMessage("The new path must be different")
                .SetValidator(new FilePathValidator());

            RuleFor(updateFileCommand => updateFileCommand.NewPath)
                .SetValidator(new FilePathValidator());
        }
    }
}
