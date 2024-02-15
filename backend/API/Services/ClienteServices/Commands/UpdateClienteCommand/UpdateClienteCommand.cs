using API.Dtos.ClienteDtos;
using MediatR;
using System.Text.Json.Serialization;

namespace API.Services.ClienteServices.Commands.UpdateClienteCommand
{
  public class UpdateClienteCommand : IRequest<ClienteDto>
  {
    [JsonIgnore]
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Identificacion { get; set; } = null!;
    public float Saldo { get; set; }
    public bool Estado { get; set; }
  }
}
