using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Inventario
    {
        [Key]
        public int InventarioId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
