using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Application.Features.Files.Commands.CreateFile;

namespace Shell.WebApi.Models.File
{
    public class CreateFileDto : IMapWith<CreateFileCommand>
    {
        public string Path { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateFileDto, CreateFileCommand>();
        }
    }
}
