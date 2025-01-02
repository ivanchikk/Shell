using MediatR;

namespace Shell.Application.Features.Drives.Commands.CreateDrive
{
    public class CreateDriveCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
    }
}
