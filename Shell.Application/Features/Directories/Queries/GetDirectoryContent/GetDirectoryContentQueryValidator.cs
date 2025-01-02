using FluentValidation;
using Shell.Application.Features.Directories.Validators;

namespace Shell.Application.Features.Directories.Queries.GetDirectoryContent
{
    public class GetDirectoryContentQueryValidator : AbstractValidator<GetDirectoryContentQuery>
    {
        public GetDirectoryContentQueryValidator()
        {
            RuleFor(getDirectoryContentQuery => getDirectoryContentQuery.Path)
                .SetValidator(new DirectoryPathValidator());
        }
    }
}
