namespace API.Dtos.ClienteDtos
{
  public class ListaClienteDto
  {
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Identificacion { get; set; } = null!;
    public float Saldo { get; set; }
    public bool Estado { get; set; }
  }
}
