using AutoMapper;
using SZEW.DTO;
using SZEW.Models;

namespace SZEW.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<WorkshopClient, WorkshopClientDto>();
            CreateMap<User, UserDto>();
            CreateMap<WorkshopJob, WorkshopJobDto>();
            CreateMap<WorkshopTask, WorkshopTaskDto>();
        }
    }
}
