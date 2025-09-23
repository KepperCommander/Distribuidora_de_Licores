using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Usuarios
    {
        [Key]
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string HashPass { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public Roles? Rol { get; set; }
    }
}
