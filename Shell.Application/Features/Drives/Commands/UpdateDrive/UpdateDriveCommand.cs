using MediatR;

namespace Shell.Application.Features.Drives.Commands.UpdateDrive
{
    public class UpdateDriveCommand : IRequest<Unit>
    {
        public string Name { get; set; } = null!;
        public string NewName { get; set; } = null!;

    }
}
