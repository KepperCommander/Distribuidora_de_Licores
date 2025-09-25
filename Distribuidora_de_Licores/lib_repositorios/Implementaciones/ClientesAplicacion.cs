using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ClientesAplicacion : IClientesAplicacion
    {
        private IConexion? IConexion = null;

        public ClientesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

  
        public static bool Validar(Clientes entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");

            
            if (string.IsNullOrWhiteSpace(entidad.Tipo))
                throw new Exception("el tipo es obligatorio");
            if (string.IsNullOrWhiteSpace(entidad.RazonSocial))
                throw new Exception("ingresar la razon social es obligatorio");

            
            if (entidad.Tipo.Length > 20)
                throw new Exception("supera los 20 caracteres");
            if (entidad.RazonSocial.Length > 120)
                throw new Exception("supera los 120 caracteres");
            if (entidad.NIT != null && entidad.NIT.Length > 40)
                throw new Exception("supera los 40 caracteres");
            if (entidad.Telefono != null && entidad.Telefono.Length > 30)
                throw new Exception("supera los 30 caracteress");
            if (entidad.Ciudad != null && entidad.Ciudad.Length > 80)
                throw new Exception("supra los 80 acracteres");

            return true;
        }

        public Clientes? Guardar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ClienteId != 0)
                throw new Exception("ya se guardo");

            if (!Validar(entidad))
                throw new Exception("no valido");

            this.IConexion!.Clientes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Clientes? Modificar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ClienteId == 0)
                throw new Exception("no se guardo");

            if (!Validar(entidad))
                throw new Exception("no valido");

            var entry = this.IConexion!.Entry<Clientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Clientes? Borrar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ClienteId == 0)
                throw new Exception("no se guardo");

            this.IConexion!.Clientes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Clientes> Listar()
        {
            return this.IConexion!.Clientes!.Take(20).ToList();
        }

        // Filtro por Razón Social y/o NIT (Contains)
        public List<Clientes> PorRazonSocialONit(Clientes? filtro)
        {
            var razon = filtro?.RazonSocial ?? string.Empty;
            var nit = filtro?.NIT ?? string.Empty;

            var q = this.IConexion!.Clientes!.AsQueryable();

            if (!string.IsNullOrWhiteSpace(razon))
                q = q.Where(x => x.RazonSocial.Contains(razon));

            if (!string.IsNullOrWhiteSpace(nit))
                q = q.Where(x => x.NIT != null && x.NIT.Contains(nit));

            return q.Take(20).ToList();
        }
    }
}
