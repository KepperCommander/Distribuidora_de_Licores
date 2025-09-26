using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class VentaDetalleAplicacion : IVentaDetalleAplicacion
    {
        private IConexion? IConexion = null;

        public VentaDetalleAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        
        public static bool Validar(VentaDetalle entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");


            //operaciones

            if (entidad.VentaId <= 0)
                throw new Exception("el id de venta es obligatorio");

            if (entidad.ProductoId <= 0)
                throw new Exception("el id del producto es obligatorio");

            if (entidad.Cantidad <= 0)
                throw new Exception("la cantidad debe ser mayor que 0");

            if (entidad.PrecioVenta <= 0)
                throw new Exception("el precio venta debe ser mayor que 0");

            return true;
        }

        public VentaDetalle? Guardar(VentaDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaDetId != 0) throw new Exception("lbYaSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            this.IConexion!.VentaDetalle!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public VentaDetalle? Modificar(VentaDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaDetId == 0) throw new Exception("lbNoSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            var local = this.IConexion!.VentaDetalle!.Local
                .FirstOrDefault(x => x.VentaDetId == entidad.VentaDetId);
            if (local != null && !ReferenceEquals(local, entidad))
                this.IConexion.Entry(local).State = EntityState.Detached;

            var entry = this.IConexion.Entry<VentaDetalle>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public VentaDetalle? Borrar(VentaDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaDetId == 0) throw new Exception("lbNoSeGuardo");

            this.IConexion!.VentaDetalle!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<VentaDetalle> Listar()
        {
            return this.IConexion!.VentaDetalle!.Take(20).ToList();
        }

        // el filto son los todos los detalles
        public List<VentaDetalle> PorVenta(VentaDetalle? filtro)
        {
            var ventaId = filtro?.VentaId ?? 0;
            var q = this.IConexion!.VentaDetalle!.AsQueryable();
            if (ventaId > 0) q = q.Where(d => d.VentaId == ventaId);
            return q.Take(50).ToList();
        }
    }
}
