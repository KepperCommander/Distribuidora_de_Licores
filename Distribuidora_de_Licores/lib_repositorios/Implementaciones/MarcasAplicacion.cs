using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class MarcasAplicacion : IMarcasAplicacion
    {
        private IConexion? IConexion = null;

        public MarcasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public static bool Validar(Marcas entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            //operaciones

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("el nombre de la marca es obligatorio");

            if (entidad.Nombre.Length > 80)
                throw new Exception("supera los 80 caracteres");

            return true;
        }

        public Marcas? Guardar(Marcas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MarcaId != 0)
                throw new Exception("lbYaGuardado");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            this.IConexion!.Marcas!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Marcas? Modificar(Marcas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MarcaId == 0)
                throw new Exception("lbNoSeGuardo");

            if (!Validar(entidad))
                throw new Exception("lbNoEsValido");

            var entry = this.IConexion!.Entry<Marcas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Marcas? Borrar(Marcas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MarcaId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Marcas!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Marcas> Listar()
        {
            return this.IConexion!.Marcas!.Take(20).ToList();
        }

        public List<Marcas> PorNombre(Marcas? filtro)
        {
            var nombre = filtro?.Nombre ?? string.Empty;
            return this.IConexion!.Marcas!
                     .Where(x => x.Nombre.Contains(nombre))
                     .Take(20)
                     .ToList();
        }
    }
}
