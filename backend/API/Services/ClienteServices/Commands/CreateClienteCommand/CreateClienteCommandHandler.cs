using API.Data;
using API.Dtos.ClienteDtos;
using API.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace API.Services.ClienteServices.Commands.CreateClienteCommand
{
  public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, ClienteDto>
  {
    private readonly ControlGlobalContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateClienteCommand> _validator;

    public CreateClienteCommandHandler(ControlGlobalContext context, IMapper mapper, IValidator<CreateClienteCommand> validator)
    {
      _context = context;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<ClienteDto> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
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
          var clienteToCreate = _mapper.Map<Cliente>(request);
          await _context.AddAsync(clienteToCreate);
          await _context.SaveChangesAsync();

          var clienteDto = _mapper.Map<ClienteDto>(clienteToCreate);

          clienteDto.StatusCode = StatusCodes.Status200OK;
          clienteDto.IsSuccess = true;
          clienteDto.ErrorMessage = string.Empty;

          return clienteDto;
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
