using FluentValidation;
using Shell.Application.Features.Drives.Validators;

namespace Shell.Application.Features.Drives.Queries.GetDrive
{
    public class GetDriveQueryValidator : AbstractValidator<GetDriveQuery>
    {
        public GetDriveQueryValidator()
        {
            RuleFor(getDriveQuery => getDriveQuery.Name)
                .SetValidator(new DriveNameValidator());
        }
    }
}
