using FluentValidation;
using Shell.Application.Features.Files.Validators;

namespace Shell.Application.Features.Files.Queries.SearchFiles
{
    public class SearchFileQueryValidator : AbstractValidator<SearchFileQuery>
    {
        public SearchFileQueryValidator()
        {
            RuleFor(searchFileQuery => searchFileQuery.Path)
                .SetValidator(new FilePathValidator());

            RuleFor(searchFileQuery => searchFileQuery.Name)
                .MaximumLength(260)
                .Matches(@"^(?!.*\\(?:CON|PRN|AUX|NUL|COM[0-9][¹²³]?|LPT[0-9][¹²³]?|[\x00-\x1F])\.?(?:\\|$)).*$")
                .WithMessage("{PropertyValue} File path contains reserved words.")
                .Matches(@"^(?:(?:[^/:*?""<>|\r\n\s\\]+[^/:*?""<>|\r\n\\]+[^/:*?""<>\r\n\s\\]+|[^/:*?""<>|\r\n\s\\]?[^/:*?""<>\r\n\s\\]))+$")
                .WithMessage("{PropertyValue} File name is invalid.")
                .When(searchFileQuery => !string.IsNullOrEmpty(searchFileQuery.Name));

            //RuleFor(searchFileQuery => searchFileQuery.Type)
            //    .Matches(@"^(?:[^/:*?""""<>|\r\n\s\\]?[^/:*?""""<>|\r\n\s\\.]+)+$")
            //    .WithMessage("{PropertyValue} File type is invalid.")
            //    .When(searchFileQuery => !string.IsNullOrEmpty(searchFileQuery.Name));
        }
    }
}
