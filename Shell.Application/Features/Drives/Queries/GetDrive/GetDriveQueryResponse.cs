using AutoMapper;
using Shell.Application.Common.Mappings;
using Shell.Domain.Entities;

namespace Shell.Application.Features.Drives.Queries.GetDrive
{
    public class GetDriveQueryResponse : IMapWith<Drive>
    {
        public string Name { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Drive, GetDriveQueryResponse>();
        }
    }
}
