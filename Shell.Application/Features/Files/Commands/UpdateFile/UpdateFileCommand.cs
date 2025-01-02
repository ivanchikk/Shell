using MediatR;

namespace Shell.Application.Features.Files.Commands.UpdateFile
{
    public class UpdateFileCommand : IRequest<Unit>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;
    }
}
