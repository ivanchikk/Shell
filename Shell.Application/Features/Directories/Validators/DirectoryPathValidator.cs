using FluentValidation;

namespace Shell.Application.Features.Directories.Validators
{
    public class DirectoryPathValidator : AbstractValidator<string>
    {
        public DirectoryPathValidator()
        {
            RuleFor(directoryPath => directoryPath)
                .MaximumLength(260)
                .Matches(@"^(?!.*\\(?:CON|PRN|AUX|NUL|COM[0-9][¹²³]?|LPT[0-9][¹²³]?|[\x00-\x1F])\.?(?:\\|$)).*$")
                .WithMessage("{PropertyValue} Directory path contains reserved words.")
                .Matches(@"^.*[^\\]$")
                .WithMessage("{PropertyValue} Last character can't be \\")
                .Matches(@"^[A-Z]:\\(?:(?:[^/:*?""<>|\r\n\s\\]+[^/:*?""<>|\r\n\\]+[^/:*?""<>\r\n\s\\.]+|[^/:*?""<>|\r\n\s\\]?[^/:*?""<>\r\n\s\\.])\\?)+$")
                .WithMessage("{PropertyValue} Directory path is invalid.");
        }
    }
}
