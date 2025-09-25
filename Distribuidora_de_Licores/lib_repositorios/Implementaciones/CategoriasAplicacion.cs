using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class CategoriasAplicacion : ICategoriasAplicacion
    {
        private IConexion? IConexion = null;

        public CategoriasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }
        public static bool Validar(Categorias entidad)
        {
            if (entidad == null)
                throw new Exception("le falta informacion");

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("el nombre es obligatorio");

            if (entidad.Nombre.Length > 60)
                throw new Exception("supera los 50 caracteres");

            if (entidad.Descripcion != null && entidad.Descripcion.Length > 200)
                throw new Exception("superan los 200 caracteres");

            return true;
        }

        public Categorias? Guardar(Categorias? entidad)
        {
            if (entidad == null)
                throw new Exception("falta info");
            if (entidad.CategoriaId != 0)
                throw new Exception("ya se guardo");

            if (!Validar(entidad))
                throw new Exception("no valido");

            this.IConexion!.Categorias!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Categorias? Modificar(Categorias? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.CategoriaId == 0)
                throw new Exception("no se guardo");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            var entry = this.IConexion!.Entry<Categorias>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Categorias? Borrar(Categorias? entidad)
        {
            if (entidad == null)
                throw new Exception("falta info");
            if (entidad.CategoriaId == 0)
                throw new Exception("no se guardo");

            this.IConexion!.Categorias!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Categorias> Listar()
        {
            return this.IConexion!.Categorias!.Take(20).ToList();
        }

        public List<Categorias> PorNombre(Categorias? filtro)
        {
            var nombre = filtro?.Nombre ?? string.Empty;
            return this.IConexion!.Categorias!
                     .Where(x => x.Nombre.Contains(nombre))
                     .Take(20)
                     .ToList();
        }
    }
}
