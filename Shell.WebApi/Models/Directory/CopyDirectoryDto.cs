using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Directories.Commands.CopyDirectory;

namespace Shell.WebApi.Models.Directory
{
    public class CopyDirectoryDto : IMapWith<CopyDirectoryCommand>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyDirectoryDto, CopyDirectoryCommand>();
        }
    }
}
