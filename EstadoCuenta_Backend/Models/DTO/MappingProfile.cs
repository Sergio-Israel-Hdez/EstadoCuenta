using AutoMapper;

namespace EstadoCuenta_Backend.Models.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<EstadoCuentaResponse, EstadoCuentaResponseDTO>();
            CreateMap<InsertarCompraCommand, InsertarCompraCommandDTO>().ReverseMap();
            CreateMap<InsertarPagoCommand, InsertarPagoCommandDTO>().ReverseMap();    
            CreateMap<TransaccionesMensualesResponse,TransaccionesMensualesResponseDTO>();
            CreateMap<ComprasResponse, ComprasResponseDTO>();
        }
    }
}
