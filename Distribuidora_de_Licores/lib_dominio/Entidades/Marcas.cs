using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Marcas
    {
        [Key]
        public int MarcaId { get; set; }

        [Required, StringLength(80)]
        public string Nombre { get; set; } = string.Empty;
    }
}
