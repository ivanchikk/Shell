using FluentValidation;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Queries.GetFile
{
    public class GetFileQueryValidator : AbstractValidator<GetFileQuery>
    {
        public GetFileQueryValidator()
        {
            RuleFor(getFileQuery => getFileQuery.Path)
                .SetValidator(new FilePathValidator());
        }
    }
}
