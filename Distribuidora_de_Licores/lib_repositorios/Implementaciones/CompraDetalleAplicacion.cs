using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class CompraDetalleAplicacion : ICompraDetalleAplicacion
    {
        private IConexion? IConexion = null;

        public CompraDetalleAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        
        public static bool Validar(CompraDetalle entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            //operaciones

            if (entidad.CompraId <= 0)
                throw new Exception("el id de compra es obligatorio");

            if (entidad.ProductoId <= 0)
                throw new Exception("el id de producto es obligatorio");

            if (entidad.Cantidad <= 0)
                throw new Exception("la cantidad no puede ser negativa o 0");

            if (entidad.PrecioCompra <= 0)
                throw new Exception("el precio de compra no puede ser 0 o menor");

            // Subtotal es columna calculada en sql
            return true;
        }

        public CompraDetalle? Guardar(CompraDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.CompraDetId != 0) throw new Exception("lbYaSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            this.IConexion!.CompraDetalle!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public CompraDetalle? Modificar(CompraDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.CompraDetId == 0) throw new Exception("lbNoSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

   
           
            var entry = this.IConexion.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public CompraDetalle? Borrar(CompraDetalle? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.CompraDetId == 0) throw new Exception("lbNoSeGuardo");

            this.IConexion!.CompraDetalle!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<CompraDetalle> Listar()
        {
            return this.IConexion!.CompraDetalle!.Take(20).ToList();
        }

        // filtro para la lista
        public List<CompraDetalle> PorCompra(CompraDetalle? filtro)
        {
            var compraId = filtro?.CompraId ?? 0;
            var q = this.IConexion!.CompraDetalle!.AsQueryable();
            if (compraId > 0) q = q.Where(d => d.CompraId == compraId);
            return q.Take(50).ToList();
        }
    }
}
