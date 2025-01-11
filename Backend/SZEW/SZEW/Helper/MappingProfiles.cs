using AutoMapper;
using SZEW.DTO;
using SZEW.Models;

namespace SZEW.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<WorkshopClient, WorkshopClientDto>();
            CreateMap<User, UserDto>();
        }
    }
}
