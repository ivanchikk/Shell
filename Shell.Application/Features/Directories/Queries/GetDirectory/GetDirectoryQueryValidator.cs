using FluentValidation;
using Shell.Application.Features.Directories.Validators;

namespace Shell.Application.Features.Directories.Queries.GetDirectory
{
    public class GetDirectoryQueryValidator : AbstractValidator<GetDirectoryQuery>
    {
        public GetDirectoryQueryValidator()
        {
            RuleFor(getDirectoryQuery => getDirectoryQuery.Path)
                .SetValidator(new DirectoryPathValidator());
        }
    }
}
