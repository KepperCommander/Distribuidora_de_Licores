using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Ventas
    {
        [Key]
        public int VentaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Total { get; set; } = 0m;
    }
}
