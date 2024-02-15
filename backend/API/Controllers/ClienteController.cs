using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.ClienteDtos;
using API.Services.ClienteServices.Queries.GetClientesQuery;
using API.Services.ClienteServices.Queries.GetClienteByIdQuery;
using API.Services.ClienteServices.Commands.CreateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateEstadoClienteCommand;

namespace API.Controllers;

[ApiController]
[Route("cliente")]
public class ClienteController : ControllerBase
{
  private readonly IMediator _mediator;

  public ClienteController(IMediator mediator)
  {
    _mediator = mediator;
  }


  [HttpGet]
  public Task<ListaClientesDto> GetClientes()
  {
    var clientes = _mediator.Send(new GetClientesQuery());
    return clientes;
  }


  [HttpGet("{id}")]
  public async Task<ClienteDto> GetClienteById(int id)
  {
    var cliente = await _mediator.Send(new GetClienteByIdQuery(id));
    return cliente;
  }


  [HttpPost]
  public async Task<ClienteDto> CreateCliente(CreateClienteCommand command)
  {
    var clienteCreado = await _mediator.Send(command);
    return clienteCreado;
  }


  [HttpPut("{id}")]
  public async Task<ClienteDto> UpdateCliente(int id, UpdateClienteCommand command)
  {
    command.IdCliente = id;
    var clienteActualizado = await _mediator.Send(command);
    return clienteActualizado;
  }


  [HttpPatch("{id}")]
  public async Task<ClienteDto> DeleteCliente(int id, UpdateEstadoClienteCommand command)
  {
    command.IdCliente = id;
    var clienteEliminado = await _mediator.Send(command);
    return clienteEliminado;
  }

}
