using API.Dtos.CuentaDtos;
using MediatR;

namespace API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand
{
  public class CreateTransferenciaCuentaCommand : IRequest<CuentaDto>
  {
    public float Importe { get; set; }
    public string Descripcion { get; set; } = null!;
    public int IdCliente { get; set; }
    public string Identificacion { get; set; } = null!;
  }
}
