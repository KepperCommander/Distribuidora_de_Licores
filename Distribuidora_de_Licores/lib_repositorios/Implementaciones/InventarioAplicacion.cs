using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class InventarioAplicacion : IInventarioAplicacion
    {
        private IConexion? IConexion = null;

        public InventarioAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

     
        public static bool Validar(Inventario entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");

            //operaciones
            if (entidad.SucursalId <= 0) throw new Exception("el id de la sucrusal es obligatorio");
            if (entidad.ProductoId <= 0) throw new Exception("el id del producto es obligatorio");
            if (entidad.Stock < 0) throw new Exception("el stock no puede ser negativo");
            return true;
        }

        
        public Inventario? Guardar(Inventario? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.InventarioId != 0) throw new Exception("ya se guardo");
            if (!Validar(entidad)) throw new Exception("no valido");


            var existente = this.IConexion!.Inventario!
                .FirstOrDefault(x => x.SucursalId == entidad.SucursalId &&
                                     x.ProductoId == entidad.ProductoId);

            if (existente != null)
            {
                existente.Stock += entidad.Stock;
                var entry = this.IConexion.Entry<Inventario>(existente);
                entry.State = EntityState.Modified;
                this.IConexion.SaveChanges();
                return existente;
            }

           
            this.IConexion!.Inventario!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

       
        public Inventario? GuardarEstricto(Inventario? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.InventarioId != 0) throw new Exception("ya se guardo");
            if (!Validar(entidad)) throw new Exception("lbNoEsValido");

            bool existe = this.IConexion!.Inventario!
                .Any(x => x.SucursalId == entidad.SucursalId &&
                          x.ProductoId == entidad.ProductoId);
            if (existe) throw new Exception("esta duplicado ell sucrsal+producto (es unique)");

            this.IConexion!.Inventario!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Inventario? Modificar(Inventario? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.InventarioId == 0) throw new Exception("lbNoSeGuardo");
            if (!Validar(entidad)) throw new Exception("ni es valido");

            var entry = this.IConexion!.Entry<Inventario>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Inventario? Borrar(Inventario? entidad)
        {
            if (entidad == null) throw new Exception("lbFaltaInformacion");
            if (entidad.InventarioId == 0) throw new Exception("lbNoSeGuardo");

            this.IConexion!.Inventario!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Inventario> Listar()
        {
            return this.IConexion!.Inventario!.Take(20).ToList();
        }

        public List<Inventario> PorSucursalProducto(Inventario? filtro)
        {
            var suc = filtro?.SucursalId ?? 0;
            var pro = filtro?.ProductoId ?? 0;

            var q = this.IConexion!.Inventario!.AsQueryable();
            if (suc > 0) q = q.Where(x => x.SucursalId == suc);
            if (pro > 0) q = q.Where(x => x.ProductoId == pro);

            return q.Take(20).ToList();
        }
    }
}
