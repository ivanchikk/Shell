using AutoMapper;
using Shell.Application.Common.Mappings;
using Directory = Shell.Domain.Entities.Directory;

namespace Shell.Application.Features.Directories.Queries.GetDirectory
{
    public class GetDirectoryQueryResponse : IMapWith<Directory>
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Directory, GetDirectoryQueryResponse>();
        }
    }
}
