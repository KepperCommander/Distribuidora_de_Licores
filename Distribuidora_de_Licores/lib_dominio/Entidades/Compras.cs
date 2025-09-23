using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Compras
    {
        [Key]
        public int CompraId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Total { get; set; } = 0m;
    }
}
