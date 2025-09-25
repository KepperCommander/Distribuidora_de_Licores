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

        // Validaciones básicas (fiel al ejemplo)
        public static bool Validar(Sucursales entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(entidad.Ciudad))
                throw new Exception("La Ciudad es obligatoria.");
            if (string.IsNullOrWhiteSpace(entidad.Direccion))
                throw new Exception("La Dirección es obligatoria.");

            if (entidad.Nombre.Length > 80)
                throw new Exception("El Nombre no puede superar 80 caracteres.");
            if (entidad.Ciudad.Length > 80)
                throw new Exception("La Ciudad no puede superar 80 caracteres.");
            if (entidad.Direccion.Length > 120)
                throw new Exception("La Dirección no puede superar 120 caracteres.");

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
