using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace lib_dominio.Entidades
    {
        public class Auditoria
        {
            public int AuditoriaId { get; set; }
            public DateTime Fecha { get; set; }
            public string? Usuario { get; set; }
            public string Tabla { get; set; } = default!;
            public string Llave { get; set; } = default!;
            public string Accion { get; set; } = default!;
            public string? ValoresAntes { get; set; }
            public string? ValoresDespues { get; set; }
        }
    }


