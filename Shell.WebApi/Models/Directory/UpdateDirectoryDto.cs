using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Directories.Commands.UpdateDirectory;

namespace Shell.WebApi.Models.Directory
{
    public class UpdateDirectoryDto : IMapWith<UpdateDirectoryCommand>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDirectoryDto, UpdateDirectoryCommand>();
        }
    }
}
