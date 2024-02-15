using API.Dtos.CuentaDtos;
using MediatR;

namespace API.Services.CuentaServices.Commands.CreateCuentaCommand
{
  public class CreateCuentaCommand : IRequest<CuentaDto>
  {
    public float Importe { get; set; }
    public string Descripcion { get; set; } = null!;
    public int IdCliente { get; set; }
  }
}
