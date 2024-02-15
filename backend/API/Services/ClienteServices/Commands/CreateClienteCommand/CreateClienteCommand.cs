using API.Dtos.ClienteDtos;
using MediatR;

namespace API.Services.ClienteServices.Commands.CreateClienteCommand
{
  public class CreateClienteCommand : IRequest<ClienteDto>
  {
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Identificacion { get; set; } = null!;
    public float Saldo { get; set; }
    public bool Estado { get; set; }
  }
}
