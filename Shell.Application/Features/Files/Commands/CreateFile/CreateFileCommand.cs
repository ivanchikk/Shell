using MediatR;

namespace Shell.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileCommand : IRequest<string>
    {
        public string Path { get; set; } = null!;
    }
}
