using API.Dtos.CuentaDtos;
using MediatR;

namespace API.Services.CuentaServices.Queries.GetCuentasByIdQuery
{
  public record GetCuentasByIdQuery(int id) : IRequest<ListaCuentasDto>;
}
