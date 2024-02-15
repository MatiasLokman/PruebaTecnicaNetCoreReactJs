using API.Data;
using API.Dtos.ClienteDtos;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.ClienteServices.Commands.UpdateClienteCommand
{
  public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, ClienteDto>
  {
    private readonly ControlGlobalContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateClienteCommand> _validator;

    public UpdateClienteCommandHandler(ControlGlobalContext context, IMapper mapper, IValidator<UpdateClienteCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ClienteDto> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
          var ClienteVacio = new ClienteDto();

          ClienteVacio.StatusCode = StatusCodes.Status400BadRequest;
          ClienteVacio.ErrorMessage = string.Join(". ", validationResult.Errors.Select(e => e.ErrorMessage));
          ClienteVacio.IsSuccess = false;

          return ClienteVacio;
        }
        else
        {
          var ClienteToUpdate = await _context.Clientes.FindAsync(request.IdCliente);

          if (ClienteToUpdate == null)
          {
            var ClienteVacio = new ClienteDto();

            ClienteVacio.StatusCode = StatusCodes.Status404NotFound;
            ClienteVacio.ErrorMessage = "El cliente no existe";
            ClienteVacio.IsSuccess = false;

            return ClienteVacio;
          }
          else
          {
            ClienteToUpdate.Nombre = request.Nombre;
            ClienteToUpdate.Apellido = request.Apellido;
            ClienteToUpdate.Identificacion = request.Identificacion;
            ClienteToUpdate.Saldo = request.Saldo;
            ClienteToUpdate.Estado = request.Estado;

            await _context.SaveChangesAsync();

            var clienteDto = _mapper.Map<ClienteDto>(ClienteToUpdate);

            clienteDto.StatusCode = StatusCodes.Status200OK;
            clienteDto.IsSuccess = true;
            clienteDto.ErrorMessage = "";

            return clienteDto;
          }
        }
      }
      catch (Exception ex)
      {
        var ClienteVacio = new ClienteDto();

        ClienteVacio.StatusCode = StatusCodes.Status400BadRequest;
        ClienteVacio.ErrorMessage = ex.Message;
        ClienteVacio.IsSuccess = false;

        return ClienteVacio;
      }
    }

  }
}
