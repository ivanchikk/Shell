using MediatR;

namespace Shell.Application.Features.Directories.Commands.DeleteDirectory
{
    public class DeleteDirectoryCommand : IRequest<Unit>
    {
        public string Path { get; set; } = null!;
    }
}
