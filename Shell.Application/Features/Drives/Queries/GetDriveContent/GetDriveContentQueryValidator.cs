using FluentValidation;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Drives.Queries.GetDriveContent
{
    public class GetDriveContentQueryValidator : AbstractValidator<GetDriveContentQuery>
    {
        public GetDriveContentQueryValidator()
        {
            RuleFor(getDriveContentQuery => getDriveContentQuery.Name)
                .SetValidator(new DriveNameValidator());
        }
    }
}
