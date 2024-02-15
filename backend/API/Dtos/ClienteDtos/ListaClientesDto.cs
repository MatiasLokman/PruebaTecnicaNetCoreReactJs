using API.AnswerBase;

namespace API.Dtos.ClienteDtos
{
  public class ListaClientesDto : RespuestaBase
  {
    public List<ListaClienteDto>? Clientes { get; set; }
  }
}
