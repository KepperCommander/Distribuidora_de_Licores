using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Empleados
    {
        [Key]
        public int EmpleadoId { get; set; }

        [Required]
        public int SucursalId { get; set; }

        public int? UsuarioId { get; set; }

        [Required, StringLength(80)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string Apellidos { get; set; } = string.Empty;

        [Required, StringLength(60)]
        public string Cargo { get; set; } = string.Empty;
    }
}