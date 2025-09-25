using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ProductosAplicacion : IProductosAplicacion
    {
        private IConexion? IConexion = null;

        public ProductosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

       
        public static bool Validar(Productos entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            // FKs requeridas 
            if (entidad.CategoriaId <= 0)
                throw new Exception("CategoriaId es obligatorio.");
            if (entidad.MarcaId <= 0)
                throw new Exception("MarcaId es obligatorio.");
            if (entidad.ProveedorId <= 0)
                throw new Exception("ProveedorId es obligatorio.");

           
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("ingresar el nombre es obligatorio");
            if (string.IsNullOrWhiteSpace(entidad.Unidad))
                throw new Exception("ingresar la unidad es obligatorio");

            
            if (entidad.Nombre.Length > 120)
                throw new Exception("supera 120 caracteres");
            if (entidad.Unidad.Length > 20)
                throw new Exception("supera los 20 caracteres");

            
            if (entidad.ContenidoML <= 0)
                throw new Exception("los mililitros no pueden ser negativos");
            if (entidad.PrecioUnit <= 0)
                throw new Exception("el precio unidad no puede ser negativo");

            return true;
        }

        public Productos? Guardar(Productos? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProductoId != 0)
                throw new Exception("se guardo ");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            this.IConexion!.Productos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Productos? Modificar(Productos? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProductoId == 0)
                throw new Exception("no se guardo");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            var entry = this.IConexion!.Entry<Productos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Productos? Borrar(Productos? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.ProductoId == 0)
                throw new Exception("no se guardo");

            this.IConexion!.Productos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Productos> Listar()
        {
            return this.IConexion!.Productos!.Take(20).ToList();
        }

        public List<Productos> PorNombre(Productos? filtro)
        {
            var nombre = filtro?.Nombre ?? string.Empty;
            return this.IConexion!.Productos!
                     .Where(x => x.Nombre.Contains(nombre))
                     .Take(20)
                     .ToList();
        }
    }
}
