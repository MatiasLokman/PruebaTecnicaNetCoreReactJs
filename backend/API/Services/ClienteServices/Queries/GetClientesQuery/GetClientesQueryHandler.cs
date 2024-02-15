using API.Data;
using API.Dtos.ClienteDtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Queries.GetClientesQuery
{
  public class GetClientesQueryHandler : IRequestHandler<GetClientesQuery, ListaClientesDto>
  {
    private readonly ControlGlobalContext _context;
    private readonly IMapper _mapper;
    public GetClientesQueryHandler(ControlGlobalContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ListaClientesDto> Handle(GetClientesQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var clientes = await _context.Clientes
            .Select(x => new ListaClienteDto
            {
              IdCliente = x.IdCliente,
              Nombre = x.Nombre,
              Apellido = x.Apellido,
              Identificacion = x.Identificacion,
              Saldo = x.Saldo,
              Estado = x.Estado
            })
            .OrderByDescending(x => x.Estado)
            .ThenBy(x => x.Nombre)
            .ToListAsync();

        if (clientes.Count > 0)
        {
          var listaClientesDto = new ListaClientesDto();
          listaClientesDto.Clientes = clientes;

          listaClientesDto.StatusCode = StatusCodes.Status200OK;
          listaClientesDto.ErrorMessage = string.Empty;
          listaClientesDto.IsSuccess = true;

          return listaClientesDto;
        }
        else
        {
          var listaClientesVacia = new ListaClientesDto();

          listaClientesVacia.StatusCode = StatusCodes.Status404NotFound;
          listaClientesVacia.ErrorMessage = "No se han encontrado clientes";
          listaClientesVacia.IsSuccess = false;

          return listaClientesVacia;
        }
      }
      catch (Exception ex)
      {
        var listaClientesVacia = new ListaClientesDto();

        listaClientesVacia.StatusCode = StatusCodes.Status400BadRequest;
        listaClientesVacia.ErrorMessage = ex.Message;
        listaClientesVacia.IsSuccess = false;

        return listaClientesVacia;
      }
    }
  }
}
