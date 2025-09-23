using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Productos
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int MarcaId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Unidad { get; set; } = string.Empty; // Botella, Caja, SixPack

        [Required]
        public int ContenidoML { get; set; }

        [Required]
        public decimal PrecioUnit { get; set; }
    }
}
