using API.Dtos.ClienteDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.ClienteServices.Commands.UpdateEstadoClienteCommand
{
  public class UpdateEstadoClienteCommand : IRequest<ClienteDto>
  {
    [JsonIgnore]
    public int IdCliente { get; set; }

    public bool Estado { get; set; }
  }
}
