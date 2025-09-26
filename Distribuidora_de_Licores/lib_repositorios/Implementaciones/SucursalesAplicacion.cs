using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class SucursalesAplicacion : ISucursalesAplicacion
    {
        private IConexion? IConexion = null;

        public SucursalesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

       
        public static bool Validar(Sucursales entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            //operaciones
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("el nombre es obligatorio");
            if (string.IsNullOrWhiteSpace(entidad.Ciudad))
                throw new Exception("la ciudad es obligatoria");
            if (string.IsNullOrWhiteSpace(entidad.Direccion))
                throw new Exception("la direccion es obligatoria ");

            if (entidad.Nombre.Length > 80)
                throw new Exception("supera lo 80 caracteres el nombre");
            if (entidad.Ciudad.Length > 80)
                throw new Exception("supera los 60 caracteres la ciudad");
            if (entidad.Direccion.Length > 120)
                throw new Exception("supera los 120 caracteres la direccion");

            return true;
        }

        public Sucursales? Guardar(Sucursales? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId != 0)
                throw new Exception("lbYaSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            this.IConexion!.Sucursales!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Sucursales? Modificar(Sucursales? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId == 0)
                throw new Exception("lbNoSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            var entry = this.IConexion!.Entry<Sucursales>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Sucursales? Borrar(Sucursales? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.SucursalId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Sucursales!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Sucursales> Listar()
        {
            return this.IConexion!.Sucursales!.Take(20).ToList();
        }

        public List<Sucursales> PorNombre(Sucursales? filtro)
        {
            var nombre = filtro?.Nombre ?? string.Empty;
            return this.IConexion!.Sucursales!
                     .Where(x => x.Nombre.Contains(nombre))
                     .Take(20)
                     .ToList();
        }
    }
}
