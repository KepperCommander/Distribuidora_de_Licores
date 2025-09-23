using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }

        [Required, StringLength(20)]
        public string Tipo { get; set; } = string.Empty; // Minorista, Mayorista, Bar, Restaurante

        [Required, StringLength(120)]
        public string RazonSocial { get; set; } = string.Empty;

        [StringLength(40)]
        public string? NIT { get; set; }

        [StringLength(30)]
        public string? Telefono { get; set; }

        [StringLength(80)]
        public string? Ciudad { get; set; }
    }
}
