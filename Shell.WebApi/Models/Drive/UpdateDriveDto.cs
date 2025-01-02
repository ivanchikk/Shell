using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Drives.Commands.UpdateDrive;

namespace Shell.WebApi.Models.Drive
{
    public class UpdateDriveDto : IMapWith<UpdateDriveCommand>
    {
        public string Name { get; set; } = null!;
        public string NewName { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDriveDto, UpdateDriveCommand>();
        }
    }
}
