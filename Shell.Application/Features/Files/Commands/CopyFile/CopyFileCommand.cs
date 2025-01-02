using MediatR;

namespace Shell.Application.Features.Files.Commands.CopyFile
{
    public class CopyFileCommand : IRequest<string>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;
    }
}
