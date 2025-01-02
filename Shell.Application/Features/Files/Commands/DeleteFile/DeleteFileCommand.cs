using MediatR;

namespace Shell.Application.Features.Files.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest<Unit>
    {
        public string Path { get; set; } = null!;
    }
}
