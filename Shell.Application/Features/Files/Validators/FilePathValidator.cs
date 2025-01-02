using FluentValidation;

namespace Shell.Application.Features.Files.Validators
{
    public class FilePathValidator : AbstractValidator<string>
    {
        public FilePathValidator()
        {
            RuleFor(filePath => filePath)
                .MaximumLength(260)
                .Matches(@"^(?!.*\\(?:CON|PRN|AUX|NUL|COM[0-9][¹²³]?|LPT[0-9][¹²³]?|[\x00-\x1F])\.?(?:\\|$)).*$")
                .WithMessage("{PropertyValue} File path contains reserved words.")
                .Matches(@"^.*[^\\]$")
                .WithMessage("{PropertyValue} Last character can't be \\")
                .Matches(@"^[A-Z]:\\(?:(?:[^/:*?""<>|\r\n\s\\]+[^/:*?""<>|\r\n\\]+[^/:*?""<>\r\n\s\\]+|[^/:*?""<>|\r\n\s\\]?[^/:*?""<>\r\n\s\\])\\?)+$")
                .WithMessage("{PropertyValue} File path is invalid.");
        }
    }
}
