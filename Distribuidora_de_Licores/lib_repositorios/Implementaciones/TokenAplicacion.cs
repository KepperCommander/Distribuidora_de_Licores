using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;


using Microsoft.Extensions.Configuration;

namespace lib_repositorios.Implementaciones
{
    public class TokenAplicacion
    {
        private IConexion? IConexion = null;
        private string llave = string.Empty;

        public TokenAplicacion(IConexion iConexion )
        {
            this.IConexion = iConexion;

            try
            {
                
                this.llave = Configuracion.ObtenerValor("TokenKey");
                // Si no existe en el JSON, usar una temporal
                if (string.IsNullOrWhiteSpace(this.llave))
                    this.llave = "LlaveTemporalDesarrollo123!";
            }
            catch
            {
                this.llave = "LlaveTemporalDesarrollo123!";
            }
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        
        public string Llave(Usuarios? entidad)
        {
            var usuario = this.IConexion!.Usuarios!
                .FirstOrDefault(x => x.Username == entidad!.Username &&
                                     x.HashPass == entidad.HashPass &&
                                     x.Activo == true);

            if (usuario == null)
                return string.Empty;

            // Devuelve la llave leída desde secrets.json
            return this.llave;
        }

        public bool Validar(Dictionary<string, object> datos)
        {
            if (!datos.ContainsKey("Llave"))
                return false;

            return this.llave == datos["Llave"]?.ToString();
        }
    }
}
