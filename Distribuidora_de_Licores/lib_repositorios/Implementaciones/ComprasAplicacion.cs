using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ComprasAplicacion : IComprasAplicacion
    {
        private IConexion? IConexion = null;

        public ComprasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

    
        public static bool Validar(Compras entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.ProveedorId <= 0)
                throw new Exception("el id del proveedor es obligatorio");
            if (entidad.SucursalId <= 0)
                throw new Exception("el id de la sucursal es obligatorio.");

            if (entidad.Fecha == DateTime.MinValue)
                throw new Exception("la fecha es obligatoria");

            if (entidad.Total < 0)
                throw new Exception("El total no puede ser negativo");

            return true;
        }

        public Compras? Guardar(Compras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CompraId != 0)
                throw new Exception("lbYaSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            this.IConexion!.Compras!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Compras? Modificar(Compras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CompraId == 0)
                throw new Exception("lbNoSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            var local = this.IConexion!.Compras!.Local
                .FirstOrDefault(x => x.CompraId == entidad.CompraId);
            if (local != null && !ReferenceEquals(local, entidad))
                this.IConexion.Entry(local).State = EntityState.Detached;

            var entry = this.IConexion.Entry<Compras>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Compras? Borrar(Compras? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CompraId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Compras!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Compras> Listar()
        {
            return this.IConexion!.Compras!.Take(20).ToList();
        }

  
        public List<Compras> PorProveedor(Compras? filtro)
        {
            var prov = filtro?.ProveedorId ?? 0;

            var q = this.IConexion!.Compras!.AsQueryable();
            if (prov > 0) q = q.Where(c => c.ProveedorId == prov);

            return q.Take(20).ToList();
        }

    }
}
