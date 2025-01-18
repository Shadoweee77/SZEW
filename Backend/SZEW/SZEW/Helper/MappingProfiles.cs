using AutoMapper;
using SZEW.DTO;
using SZEW.Models;

namespace SZEW.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id));

            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<Tool, CreateToolDto>().ReverseMap();

            CreateMap<WorkshopClient, WorkshopClientDto>()
                .ForMember(dest => dest.VehicleIds, opt => opt.MapFrom(src => src.Vehicles != null
                ? src.Vehicles.Select(v => v.Id).ToList()
                : new List<int>()));
            //CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<CreateVehicleDto, Vehicle>();

            CreateMap<WorkshopJob, WorkshopJobDto>();
            CreateMap<WorkshopJobDto, WorkshopJob>();
            CreateMap<CreateWorkshopJobDto, WorkshopJob>();
            //CreateMap<WorkshopClient, WorkshopClientDto>();
            
            CreateMap<User, UserDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<WorkshopTask, WorkshopTaskDto>();
            CreateMap<CreateWorkshopTaskDto, WorkshopTask>();

            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<SparePart, SparePartDto>();
            CreateMap<CreateSparePartDto, SparePart>();
            CreateMap<SparePartsOrder, SparePartsOrderDto>();
            CreateMap<ToolsOrder, ToolsOrderDto>();

            CreateMap<ToolsRequest, ToolsRequestDto>();
            CreateMap<ToolsRequestDto, ToolsRequest>();
            CreateMap<CreateToolsRequestDto, ToolsRequest>();

            CreateMap<SaleDocument, SaleDocumentDto>();

            CreateMap<CreateToolsOrderDto, ToolsOrder>();
            CreateMap<UpdateToolsOrderDto, ToolsOrder>();
            CreateMap<ToolsOrder, ToolsOrderDto>()
                .ForMember(dest => dest.Tools, opt => opt.MapFrom(src => src.Tools != null
                ? src.Tools.Select(v => v.Id).ToList()
                : new List<int>()));


        }
    }
}
