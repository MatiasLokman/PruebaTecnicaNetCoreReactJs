using API.Data;
using API.Dtos.CuentaDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CuentaServices.Queries.GetCuentasByIdQuery
{
  public class GetCuentasByIdQueryHandler : IRequestHandler<GetCuentasByIdQuery, ListaCuentasDto>
  {
    private readonly ControlGlobalContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<GetCuentasByIdQuery> _validator;

    public GetCuentasByIdQueryHandler(ControlGlobalContext context, IMapper mapper, IValidator<GetCuentasByIdQuery> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ListaCuentasDto> Handle(GetCuentasByIdQuery request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ListaCuentasVacia = new ListaCuentasDto();

          ListaCuentasVacia.StatusCode = StatusCodes.Status400BadRequest;
          ListaCuentasVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ListaCuentasVacia.IsSuccess = false;

          return ListaCuentasVacia;
        }
        else
        {
          var clienteId = request.id;

          var cuentas = await _context.Cuentas
              .Where(x => x.IdCliente == clienteId)
              .Select(x => new ListaCuentaDto
              {
                IdCuenta = x.IdCuenta,
                FechaMovimiento = x.FechaMovimiento,
                Importe = x.Importe,
                Descripcion = x.Descripcion,
                NombreCliente = x.IdClienteNavigation.Nombre + " " + x.IdClienteNavigation.Apellido,
              })
              .OrderByDescending(x => x.FechaMovimiento)
              .ToListAsync();

          if (cuentas == null)
          {
            var ListaCuentasVacia = new ListaCuentasDto();

            ListaCuentasVacia.StatusCode = StatusCodes.Status404NotFound;
            ListaCuentasVacia.ErrorMessage = "No hay cuentas con ese cliente";
            ListaCuentasVacia.IsSuccess = false;

            return ListaCuentasVacia;
          }
          else
          {
            var listaCuentasDto = new ListaCuentasDto();
            listaCuentasDto.Cuentas = cuentas;

            listaCuentasDto.StatusCode = StatusCodes.Status200OK;
            listaCuentasDto.IsSuccess = true;
            listaCuentasDto.ErrorMessage = "";

            return listaCuentasDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ListaCuentasVacia = new ListaCuentasDto();

        ListaCuentasVacia.StatusCode = StatusCodes.Status400BadRequest;
        ListaCuentasVacia.ErrorMessage = ex.Message;
        ListaCuentasVacia.IsSuccess = false;

        return ListaCuentasVacia;
      }
    }

  }
}
