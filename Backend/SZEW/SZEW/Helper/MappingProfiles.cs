using AutoMapper;
using SZEW.DTO;
using SZEW.Models;

namespace SZEW.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<Tool, CreateToolDto>().ReverseMap();

            CreateMap<WorkshopClient, WorkshopClientDto>()
                .ForMember(dest => dest.VehicleIds, opt => opt.MapFrom(src => src.Vehicles != null
                ? src.Vehicles.Select(v => v.Id).ToList()
                : new List<int>()));

            CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id));
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<UpdateVehicleDto, Vehicle>();

            CreateMap<WorkshopJob, WorkshopJobDto>();
            CreateMap<WorkshopJobDto, WorkshopJob>();
            CreateMap<CreateWorkshopJobDto, WorkshopJob>();
            
            CreateMap<User, UserDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<WorkshopTask, WorkshopTaskDto>();
            CreateMap<CreateWorkshopTaskDto, WorkshopTask>();

            CreateMap<SparePart, SparePartDto>();
            CreateMap<CreateSparePartDto, SparePart>();

            CreateMap<SparePartsOrder, SparePartsOrderDto>();

            
            CreateMap<CreateToolsRequestDto, ToolsRequest>();
            CreateMap<ToolsRequest, ToolsRequestDto>();
            CreateMap<ToolsRequestDto, ToolsRequest>();
            CreateMap<UpdateToolsRequestDto, ToolsRequest>();

            CreateMap<SaleDocument, SaleDocumentDto>();
            CreateMap<SaleDocumentDto, SaleDocument>();
            CreateMap<CreateSaleDocumentDto, SaleDocument>();

            CreateMap<ToolsOrder, ToolsOrderDto>();
            CreateMap<CreateToolsOrderDto, ToolsOrder>();
            CreateMap<UpdateToolsOrderDto, ToolsOrder>();
            CreateMap<ToolsOrder, ToolsOrderDto>()
                .ForMember(dest => dest.Tools, opt => opt.MapFrom(src => src.Tools != null
                ? src.Tools.Select(v => v.Id).ToList()
                : new List<int>()));
        }
    }
}
