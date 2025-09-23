using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Lotes
    {
        [Key]
        public int LoteId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required, StringLength(40)]
        public string CodigoLote { get; set; } = string.Empty;

        public DateTime? Vencimiento { get; set; }

        [Required]
        public int Cantidad { get; set; }
    }
}
