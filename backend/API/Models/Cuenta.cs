using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Cuenta
    {
        public int IdCuenta { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public float Importe { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdCliente { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; } = null!;
    }
}
