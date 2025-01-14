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
            CreateMap<WorkshopClient, WorkshopClientDto>();
            CreateMap<User, UserDto>();
            CreateMap<WorkshopJob, WorkshopJobDto>();
            CreateMap<WorkshopTask, WorkshopTaskDto>();
            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<SparePart, SparePartDto>();
            CreateMap<SparePartsOrder, SparePartsOrderDto>();
            CreateMap<ToolsOrder, ToolsOrderDto>();
            CreateMap<ToolsRequest, ToolsRequestDto>();
            CreateMap<SaleDocument, SaleDocumentDto>();
        }
    }
}
