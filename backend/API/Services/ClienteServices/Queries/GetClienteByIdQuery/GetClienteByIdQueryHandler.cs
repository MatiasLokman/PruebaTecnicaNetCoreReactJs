using API.Data;
using API.Dtos.ClienteDtos;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ClienteServices.Queries.GetClienteByIdQuery
{
    public class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, ClienteDto>
    {
        private readonly ControlGlobalContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GetClienteByIdQuery> _validator;

        public GetClienteByIdQueryHandler(ControlGlobalContext context, IMapper mapper, IValidator<GetClienteByIdQuery> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ClienteDto> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var ClienteVacio = new ClienteDto
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage)),
                        IsSuccess = false
                    };

                    return ClienteVacio;
                }
                else
                {
                    var clienteId = request.id;

                    var cliente = await _context.Clientes
                    .Where(x => x.IdCliente == clienteId)
                    .Select(x => new ClienteDto
                    {
                        IdCliente = x.IdCliente,
                        Nombre = x.Nombre,
                        Apellido = x.Apellido,
                        Identificacion = x.Identificacion,
                        Saldo = x.Saldo,
                        Estado = x.Estado
                    })
                    .FirstOrDefaultAsync();

                    if (cliente == null)
                    {
                        var ClienteVacio = new ClienteDto
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            ErrorMessage = "No existe el cliente",
                            IsSuccess = false
                        };

                        return ClienteVacio;
                    }
                    else
                    {
                        var clienteDto = new ClienteDto
                        {
                            IdCliente = cliente.IdCliente,
                            Nombre = cliente.Nombre,
                            Apellido = cliente.Apellido,
                            Identificacion = cliente.Identificacion,
                            Saldo = cliente.Saldo,
                            Estado = cliente.Estado,
                            StatusCode = StatusCodes.Status200OK,
                            IsSuccess = true,
                            ErrorMessage = ""
                        };

                        return clienteDto;
                    }
                }
            }
            catch (Exception ex)
            {
                var ClienteVacio = new ClienteDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };

                return ClienteVacio;
            }
        }
    }
}