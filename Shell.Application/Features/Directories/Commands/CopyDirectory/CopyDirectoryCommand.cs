using MediatR;

namespace Shell.Application.Features.Directories.Commands.CopyDirectory
{
    public class CopyDirectoryCommand : IRequest<string>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;
    }
}
