using MediatR;

namespace Shell.Application.Features.Drives.Commands.DeleteDrive
{
    public class DeleteDriveCommand : IRequest<Unit>
    {
        public string Name { get; set; } = null!;

    }
}
