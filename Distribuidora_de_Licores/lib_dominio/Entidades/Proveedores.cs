using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Proveedores
    {
        [Key]
        public int ProveedorId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Ciudad { get; set; }

        [StringLength(30)]
        public string? Telefono { get; set; }
    }
}
