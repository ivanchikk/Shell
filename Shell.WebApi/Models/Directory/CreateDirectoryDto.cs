using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Directories.Commands.CreateDirectory;

namespace Shell.WebApi.Models.Directory
{
    public class CreateDirectoryDto : IMapWith<CreateDirectoryCommand>
    {
        public string Path { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDirectoryDto, CreateDirectoryCommand>();
        }
    }
}
