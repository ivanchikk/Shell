using MediatR;

namespace Shell.Application.Features.Directories.Commands.CreateDirectory
{
    public class CreateDirectoryCommand : IRequest<string>
    {
        public string Path { get; set; } = null!;
    }
}
