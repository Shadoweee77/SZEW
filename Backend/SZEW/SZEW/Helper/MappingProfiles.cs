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
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<WorkshopJob, WorkshopJobDto>();
            CreateMap<WorkshopJobDto, WorkshopJob>();
            CreateMap<CreateWorkshopJobDto, WorkshopJob>();
            CreateMap<WorkshopClient, WorkshopClientDto>();
            CreateMap<User, UserDto>();
            
            CreateMap<WorkshopTask, WorkshopTaskDto>();
        }
    }
}
