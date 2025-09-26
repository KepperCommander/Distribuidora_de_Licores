using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class VentasAplicacion : IVentasAplicacion
    {
        private IConexion? IConexion = null;

        public VentasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        
        public static bool Validar(Ventas entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");


            //operaciones
            if (entidad.ClienteId <= 0)
                throw new Exception("el id del clientes es obligatorio");
            if (entidad.SucursalId <= 0)
                throw new Exception("la sucursal is es obligatoria");
            if (entidad.EmpleadoId <= 0)
                throw new Exception("el id del empleado es obligatorio");

            if (entidad.Fecha == DateTime.MinValue)
                throw new Exception("la fecha es obligatoria");

            if (entidad.Total < 0)
                throw new Exception("el total no puede ser negativo");

            return true;
        }

        public Ventas? Guardar(Ventas? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaId != 0) throw new Exception("lbYaSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            this.IConexion!.Ventas!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Ventas? Modificar(Ventas? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaId == 0) throw new Exception("lbNoSeGuardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            // evitar conflicto de tracking si ya hay una instancia local
            var local = this.IConexion!.Ventas!.Local
                .FirstOrDefault(x => x.VentaId == entidad.VentaId);
            if (local != null && !ReferenceEquals(local, entidad))
                this.IConexion.Entry(local).State = EntityState.Detached;

            var entry = this.IConexion.Entry<Ventas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Ventas? Borrar(Ventas? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.VentaId == 0) throw new Exception("lbNoSeGuardo");

            this.IConexion!.Ventas!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Ventas> Listar()
        {
            return this.IConexion!.Ventas!.Take(20).ToList();
        }

        // filtro por cliente de la lista
        public List<Ventas> PorCliente(Ventas? filtro)
        {
            var clienteId = filtro?.ClienteId ?? 0;
            var q = this.IConexion!.Ventas!.AsQueryable();
            if (clienteId > 0) q = q.Where(v => v.ClienteId == clienteId);
            return q.Take(20).ToList();
        }

        // y filtro de cliente con el rango de fechas (falta utilizarlo)
        public List<Ventas> PorClienteFecha(int clienteId, DateTime? desde, DateTime? hasta)
        {
            var q = this.IConexion!.Ventas!.AsQueryable();
            if (clienteId > 0) q = q.Where(v => v.ClienteId == clienteId);
            if (desde != null) q = q.Where(v => v.Fecha >= desde);
            if (hasta != null) q = q.Where(v => v.Fecha <= hasta);
            return q.Take(50).ToList();
        }
    }
}
