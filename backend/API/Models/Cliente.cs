using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Cuenta = new HashSet<Cuenta>();
        }

        public int IdCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public float Saldo { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
