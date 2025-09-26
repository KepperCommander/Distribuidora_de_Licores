using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class RolesAplicacion : IRolesAplicacion
    {
        private IConexion? IConexion = null;

        public RolesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Roles? Guardar(Roles? entidad)
        {
            ValidarEntidadParaGuardar(entidad!);

            // logica para no dejar duplicados
            if (this.IConexion!.Roles!.Any(r => r.Nombre == entidad!.Nombre))
                throw new Exception("Nombre duplicado, ya existe un rol con ese nombre");

            this.IConexion.Roles!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Roles? Modificar(Roles? entidad)
        {
            ValidarEntidadParaModificar(entidad!);

            // logica para los duplicados
            if (this.IConexion!.Roles!.Any(r => r.Nombre == entidad!.Nombre && r.RolId != entidad!.RolId))
                throw new Exception("Nombre duplicado, ya existe un rol con ese nombre");

            var entry = this.IConexion!.Entry<Roles>(entidad!);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Roles? Borrar(Roles? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.RolId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Roles!.Remove(entidad);
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public List<Roles> Listar()
        {
            return this.IConexion!.Roles!.Take(20).ToList();
        }

        public List<Roles> PorNombre(Roles? entidad)
        {
            return this.IConexion!.Roles!
                .Where(x => x.Nombre!.Contains(entidad!.Nombre!))
                .ToList();
        }

        private static void ValidarEntidadParaGuardar(Roles entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.RolId != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarCampos(entidad);
        }

        private static void ValidarEntidadParaModificar(Roles entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.RolId == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarCampos(entidad);
        }


        private static void ValidarCampos(Roles entidad)
        {
            // para que exista y tenga menos de 50
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
                throw new Exception("el nombre es obligatorio");
            if (entidad.Nombre.Length > 50)
                throw new Exception("demasiado largo tiene que ser menos de 50 caracteres");

            // para q la descripcion no supere 200
            if (!string.IsNullOrEmpty(entidad.Descripcion) && entidad.Descripcion.Length > 200)
                throw new Exception("la descripcion esta excediendo el limite de 200");
        }
    }
}
