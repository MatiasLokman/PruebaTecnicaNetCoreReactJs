using AutoMapper;
using API.Models;

using API.Dtos.ClienteDtos;
using API.Services.ClienteServices.Commands.CreateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateEstadoClienteCommand;

using API.Dtos.CuentaDtos;
using API.Services.CuentaServices.Commands.CreateCuentaCommand;
using API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand;

namespace API.Mapper
{
  public class MapperConfig : Profile
  {
    public MapperConfig()
    {
      // Mappers para Clientes
      CreateMap<ClienteDto, Cliente>().ReverseMap();
      CreateMap<Cliente, CreateClienteCommand>().ReverseMap();
      CreateMap<Cliente, UpdateClienteCommand>().ReverseMap();
      CreateMap<Cliente, UpdateEstadoClienteCommand>().ReverseMap();

      // Mappers para Cuentas
      CreateMap<Cuenta, CuentaDto>()
          .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre));
      CreateMap<Cuenta, CreateCuentaCommand>().ReverseMap();
      CreateMap<Cuenta, CreateTransferenciaCuentaCommand>().ReverseMap();
    }
  }
}
