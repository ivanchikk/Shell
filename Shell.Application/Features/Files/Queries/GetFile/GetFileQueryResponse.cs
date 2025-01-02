using AutoMapper;
using Shell.Application.Common.Mappings;
using File = Shell.Domain.Entities.File;

namespace Shell.Application.Features.Files.Queries.GetFile
{
    public class GetFileQueryResponse : IMapWith<File>
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<File, GetFileQueryResponse>();
        }
    }
}
