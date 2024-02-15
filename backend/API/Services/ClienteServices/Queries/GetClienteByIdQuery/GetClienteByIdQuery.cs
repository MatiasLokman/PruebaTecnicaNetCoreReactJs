using API.Dtos.ClienteDtos;
using MediatR;

namespace API.Services.ClienteServices.Queries.GetClienteByIdQuery
{
  public record GetClienteByIdQuery(int id) : IRequest<ClienteDto>;
}
