using MediatR;

namespace Shell.Application.Features.Directories.Commands.UpdateDirectory
{
    public class UpdateDirectoryCommand : IRequest<Unit>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;
    }
}
