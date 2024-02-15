namespace API.Dtos.CuentaDtos
{
  public class ListaCuentaDto
  {
    public int IdCuenta { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public float Importe { get; set; }
    public string Descripcion { get; set; } = null!;
    public string? NombreCliente { get; set; }
  }
}
