using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ProveedoresAplicacion : IProveedoresAplicacion
    {
        private IConexion? IConexion = null;

        public ProveedoresAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

       
        public static bool Validar(Proveedores entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("el nombre es obligatorio");

            if (entidad.Nombre.Length > 100)
                throw new Exception("supera los 100 caracteres");

            if (entidad.Ciudad != null && entidad.Ciudad.Length > 80)
                throw new Exception("supera los 80 caracteres");

            if (entidad.Telefono != null && entidad.Telefono.Length > 30)
                throw new Exception("supera llos 30 caracteres");

            return true;
        }

        public Proveedores? Guardar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProveedorId != 0)
                throw new Exception("guardado");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            this.IConexion!.Proveedores!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Proveedores? Modificar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProveedorId == 0)
                throw new Exception("no se guardo");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            var entry = this.IConexion!.Entry<Proveedores>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Proveedores? Borrar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProveedorId == 0)
                throw new Exception("no se guardo");

            this.IConexion!.Proveedores!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Proveedores> Listar()
        {
            return this.IConexion!.Proveedores!.Take(20).ToList();
        }

       
        public List<Proveedores> PorNombreOCiudad(Proveedores? filtro)
        {
            var nombre = filtro?.Nombre ?? string.Empty;
            var ciudad = filtro?.Ciudad ?? string.Empty;

            var q = this.IConexion!.Proveedores!.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                q = q.Where(x => x.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(ciudad))
                q = q.Where(x => x.Ciudad != null && x.Ciudad.Contains(ciudad));

            return q.Take(20).ToList();
        }
    }
}
