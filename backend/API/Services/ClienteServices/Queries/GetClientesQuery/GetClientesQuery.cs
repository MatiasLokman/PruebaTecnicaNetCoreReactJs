using API.Dtos.ClienteDtos;
using MediatR;

namespace API.Services.ClienteServices.Queries.GetClientesQuery
{
  public class GetClientesQuery : IRequest<ListaClientesDto>
  {
  }
}
