using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Dtos.CuentaDtos;
using API.Services.CuentaServices.Queries.GetCuentasByIdQuery;
using API.Services.CuentaServices.Commands.CreateCuentaCommand;
using API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[ApiController]
[Route("cuenta")]
public class CuentaController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IHubContext<CuentaHub> _hubContext;

  public CuentaController(IMediator mediator, IHubContext<CuentaHub> hubContext)
  {
    _mediator = mediator;
    _hubContext = hubContext;
  }

  [HttpGet("{id}")] // Obtener cuentas de un id especifico
  public Task<ListaCuentasDto> GetCuentasById(int id)
  {
    var cuentasByIdentificador = _mediator.Send(new GetCuentasByIdQuery(id));
    return cuentasByIdentificador;
  }


  [HttpPost]
  public async Task<CuentaDto> CreateCuenta(CreateCuentaCommand command)
  {
    var cuentaCreada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("RecibirSaldoDepositado", "Se ha depositado");

    return cuentaCreada;
  }


  [HttpPost("transferencia")]
  public async Task<CuentaDto> CreateCuenta(CreateTransferenciaCuentaCommand command)
  {
    var cuentaCreada = await _mediator.Send(command);

    await _hubContext.Clients.All.SendAsync("RecibirSaldoTransferido", "Se ha transferido");

    return cuentaCreada;
  }

}
