using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacion
    {
        private IConexion? IConexion = null;

        public EmpleadosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public static bool Validar(Empleados entidad)
        {
            if (entidad == null)
                throw new Exception(" le falta informacion");

            if (entidad.SucursalId <= 0)
                throw new Exception("se tiene que ingresar la sucuersal obligatoriamente");

            if (string.IsNullOrWhiteSpace(entidad.Nombres))
                throw new Exception("los nombres son pobligatorios");
            if (entidad.Nombres.Length > 80)
                throw new Exception("supera los 80 caracteres");

            if (string.IsNullOrWhiteSpace(entidad.Apellidos))
                throw new Exception("los apellidos son obligatorios");
            if (entidad.Apellidos.Length > 80)
                throw new Exception("supera los 80 caracteres");

            if (string.IsNullOrWhiteSpace(entidad.Cargo))
                throw new Exception("se debe poner el cargo");
            if (entidad.Cargo.Length > 60)
                throw new Exception("supera los 60 caracteres");

            return true;
        }

        public Empleados? Guardar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.EmpleadoId != 0)
                throw new Exception("guardado");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            this.IConexion!.Empleados!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Empleados? Modificar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.EmpleadoId == 0)
                throw new Exception("guardado");

            if (!Validar(entidad))
                throw new Exception("no es valido");

            var entry = this.IConexion!.Entry<Empleados>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Empleados? Borrar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("le falta info");
            if (entidad.EmpleadoId == 0)
                throw new Exception("no se ha guardado");

            this.IConexion!.Empleados!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Empleados> Listar()
        {
            return this.IConexion!.Empleados!.Take(20).ToList();
        }

        public List<Empleados> PorNombres(Empleados? filtro)
        {
            var nom = filtro?.Nombres ?? string.Empty;
            return this.IConexion!.Empleados!
                     .Where(x => x.Nombres.Contains(nom))
                     .Take(20)
                     .ToList();
        }
    }
}
