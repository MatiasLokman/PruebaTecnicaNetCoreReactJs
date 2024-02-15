using API.Data;
using API.Dtos.CuentaDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.CuentaServices.Commands.CreateCuentaCommand
{
  public class CreateCuentaCommandHandler : IRequestHandler<CreateCuentaCommand, CuentaDto>
  {
    private readonly ControlGlobalContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCuentaCommand> _validator;

    public CreateCuentaCommandHandler(ControlGlobalContext context, IMapper mapper, IValidator<CreateCuentaCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<CuentaDto> Handle(CreateCuentaCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var CuentaVacia = new CuentaDto();

          CuentaVacia.StatusCode = StatusCodes.Status400BadRequest;
          CuentaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          CuentaVacia.IsSuccess = false;

          return CuentaVacia;
        }
        else
        {
          var cuentaToCreate = _mapper.Map<Cuenta>(request);

          cuentaToCreate.FechaMovimiento = DateTime.Now.ToUniversalTime();

          await _context.AddAsync(cuentaToCreate);
          await _context.SaveChangesAsync();

          // Obtener el cliente al que pertenece la cuenta
          var cliente = await _context.Clientes.FindAsync(request.IdCliente);
          if (cliente != null)
          {
            // Actualizar el saldo del cliente sumando el importe de la nueva cuenta
            cliente.Saldo += request.Importe;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
          }

          var cuentaDto = _mapper.Map<CuentaDto>(cuentaToCreate);

          cuentaDto.StatusCode = StatusCodes.Status200OK;
          cuentaDto.IsSuccess = true;
          cuentaDto.ErrorMessage = string.Empty;

          return cuentaDto;
        }
      }
      catch (Exception ex)
      {
        var CuentaVacia = new CuentaDto();

        CuentaVacia.StatusCode = StatusCodes.Status400BadRequest;
        CuentaVacia.ErrorMessage = ex.Message;
        CuentaVacia.IsSuccess = false;

        return CuentaVacia;
      }
    }

  }
}
