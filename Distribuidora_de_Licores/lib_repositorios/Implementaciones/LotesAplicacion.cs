using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class LotesAplicacion : ILotesAplicacion
    {
        private IConexion? IConexion = null;

        public LotesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

       
        public static bool Validar(Lotes entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            //operaciones

            if (entidad.ProductoId <= 0)
                throw new Exception("el id del producto es obligatorio");

            if (entidad.ProveedorId <= 0)
                throw new Exception("el id del proveedor es obligatorio y tiene q ser valido");

            if (string.IsNullOrWhiteSpace(entidad.CodigoLote))
                throw new Exception("el codigo del lote es obligatorio");

            if (entidad.CodigoLote.Length > 40)
                throw new Exception("supera los 40 caracteres");

            if (entidad.Cantidad <= 0)
                throw new Exception("la cantidad no puede ser cero ni negativo");

            
            return true;
        }

        public Lotes? Guardar(Lotes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.LoteId != 0)
                throw new Exception("lbYaSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            this.IConexion!.Lotes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Lotes? Modificar(Lotes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.LoteId == 0)
                throw new Exception("lbNoSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            var local = this.IConexion!.Lotes!.Local
                         .FirstOrDefault(x => x.LoteId == entidad.LoteId);
            if (local != null && !ReferenceEquals(local, entidad))
                this.IConexion.Entry(local).State = EntityState.Detached;

            var entry = this.IConexion.Entry<Lotes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Lotes? Borrar(Lotes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.LoteId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Lotes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Lotes> Listar()
        {
            return this.IConexion!.Lotes!.Take(20).ToList();
        }

       
        public List<Lotes> PorCodigo(Lotes? filtro)
        {
            var cod = filtro?.CodigoLote ?? string.Empty;
            return this.IConexion!.Lotes!
                     .Where(x => x.CodigoLote.Contains(cod))
                     .Take(20)
                     .ToList();
        }

    }
}
