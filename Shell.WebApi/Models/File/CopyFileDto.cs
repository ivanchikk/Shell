using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Files.Commands.CopyFile;

namespace Shell.WebApi.Models.File
{
    public class CopyFileDto : IMapWith<CopyFileCommand>
    {
        public string Path { get; set; } = null!;
        public string NewPath { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CopyFileDto, CopyFileCommand>();
        }
    }
}
