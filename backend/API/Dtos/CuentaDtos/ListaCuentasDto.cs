using API.AnswerBase;

namespace API.Dtos.CuentaDtos
{
  public class ListaCuentasDto : RespuestaBase
  {
    public List<ListaCuentaDto>? Cuentas { get; set; }
  }
}
