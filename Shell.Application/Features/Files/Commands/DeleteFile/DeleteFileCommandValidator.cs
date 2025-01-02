using FluentValidation;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Commands.DeleteFile
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(deleteFileCommand => deleteFileCommand.Path)
                .SetValidator(new FilePathValidator());
        }
    }
}
