using API.Data;
using API.Dtos.CuentaDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand
{
    public class CreateTransferenciaCuentaCommandHandler : IRequestHandler<CreateTransferenciaCuentaCommand, CuentaDto>
    {
        private readonly ControlGlobalContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTransferenciaCuentaCommand> _validator;

        public CreateTransferenciaCuentaCommandHandler(ControlGlobalContext context, IMapper mapper, IValidator<CreateTransferenciaCuentaCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CuentaDto> Handle(CreateTransferenciaCuentaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var cuentaVacia = new CuentaDto();
                    cuentaVacia.StatusCode = StatusCodes.Status400BadRequest;
                    cuentaVacia.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
                    cuentaVacia.IsSuccess = false;
                    return cuentaVacia;
                }
                else
                {
                    // Verificar que el saldo del cliente origen sea suficiente
                    var clienteOrigen = await _context.Clientes.FindAsync(request.IdCliente);
                    if (clienteOrigen == null || clienteOrigen.Saldo < request.Importe)
                    {
                        var cuentaVacia = new CuentaDto();
                        cuentaVacia.StatusCode = StatusCodes.Status400BadRequest;
                        cuentaVacia.ErrorMessage = "Saldo insuficiente para realizar la transferencia";
                        cuentaVacia.IsSuccess = false;
                        return cuentaVacia;
                    }

                    var cuentaToCreate = _mapper.Map<Cuenta>(request);
                    cuentaToCreate.FechaMovimiento = DateTime.Now.ToUniversalTime();

                    await _context.AddAsync(cuentaToCreate);

                    // Restar el importe al saldo del cliente de origen
                    clienteOrigen.Saldo -= request.Importe;

                    // Obtener el cliente que recibe el dinero
                    var clienteDestino = await _context.Clientes.SingleOrDefaultAsync(c => c.Identificacion == request.Identificacion);
                    if (clienteDestino != null)
                    {
                        // Aumentar el importe al saldo del cliente destino
                        clienteDestino.Saldo += request.Importe;
                    }

                    await _context.SaveChangesAsync();

                    var cuentaDto = _mapper.Map<CuentaDto>(cuentaToCreate);
                    cuentaDto.StatusCode = StatusCodes.Status200OK;
                    cuentaDto.IsSuccess = true;
                    cuentaDto.ErrorMessage = string.Empty;

                    return cuentaDto;
                }
            }
            catch (Exception ex)
            {
                var cuentaVacia = new CuentaDto();
                cuentaVacia.StatusCode = StatusCodes.Status400BadRequest;
                cuentaVacia.ErrorMessage = ex.Message;
                cuentaVacia.IsSuccess = false;
                return cuentaVacia;
            }
        }
    }
}