using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Drives.Commands.CreateDrive;

namespace Shell.WebApi.Models.Drive
{
    public class CreateDriveDto : IMapWith<CreateDriveCommand>
    {
        public string Name { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDriveDto, CreateDriveCommand>();
        }
    }
}
