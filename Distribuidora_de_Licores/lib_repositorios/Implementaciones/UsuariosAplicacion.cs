using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class UsuariosAplicacion : IUsuariosAplicacion
    {
        private IConexion? IConexion;

        public UsuariosAplicacion(IConexion iConexion)
        {
            IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            IConexion!.StringConexion = StringConexion;
        }

        public Usuarios? Guardar(Usuarios? entidad)
        {
            ValidarEntidadParaGuardar(entidad!);

            entidad!.Username = NormalizarUser(entidad.Username);
            entidad.HashPass = entidad.HashPass.Trim();

            // obliga que exista un rol
            if (!IConexion!.Roles!.Any(r => r.RolId == entidad.RolId))
                throw new Exception("rol inexistente");

            // username único
            if (IConexion!.Usuarios!.Any(u => u.Username.ToLower() == entidad.Username.ToLower()))
                throw new Exception("username duplicado");

            if (entidad.Activo == default) entidad.Activo = true;

            IConexion.Usuarios!.Add(entidad);
            IConexion.SaveChanges();
            return entidad;
        }

        public Usuarios? Modificar(Usuarios? entidad)
        {
            ValidarEntidadParaModificar(entidad!);

            entidad!.Username = NormalizarUser(entidad.Username);
            entidad.HashPass = entidad.HashPass.Trim();

            
            if (!IConexion!.Roles!.Any(r => r.RolId == entidad.RolId))
                throw new Exception("rol inexistente");

            // username único pero excluye el propio id
            if (IConexion!.Usuarios!.Any(u =>
                    u.UsuarioId != entidad.UsuarioId &&
                    u.Username.ToLower() == entidad.Username.ToLower()))
                throw new Exception("username duplicado.");

            var entry = IConexion!.Entry<Usuarios>(entidad);
            entry.State = EntityState.Modified;
            IConexion.SaveChanges();
            return entidad;
        }

        public Usuarios? Borrar(Usuarios? entidad)
        {
            if (entidad == null) throw new Exception("le falta info");
            if (entidad.UsuarioId == 0) throw new Exception("no se habia guardado");

            IConexion!.Usuarios!.Remove(entidad);
            IConexion.SaveChanges();
            return entidad;
        }

        public List<Usuarios> Listar()
        {
            return IConexion!.Usuarios!.OrderBy(u => u.UsuarioId).Take(20).ToList();
        }

        public List<Usuarios> PorUsername(Usuarios? entidad)
        {
            var filtro = entidad?.Username?.Trim();
            if (string.IsNullOrWhiteSpace(filtro))
                return Listar();

            return IConexion!.Usuarios!
                .Where(u => u.Username.Contains(filtro))
                .OrderBy(u => u.UsuarioId)
                .Take(20)
                .ToList();
        }

        

        private static void ValidarEntidadParaGuardar(Usuarios entidad)
        {
            if (entidad == null) throw new Exception("le falta info");
            if (entidad.UsuarioId != 0) throw new Exception("ya se habia guardado");
            ValidarCampos(entidad);
        }

        private static void ValidarEntidadParaModificar(Usuarios entidad)
        {
            if (entidad == null) throw new Exception("faltan datos basicos");
            if (entidad.UsuarioId <= 0) throw new Exception("no se pudo guardar no tiene id en la bd");
            ValidarCampos(entidad);
        }


        private static void ValidarCampos(Usuarios entidad)
        {
            // obliga a utilizar todo, que tenga menos de 50 y que no tenga espacios
            if (string.IsNullOrWhiteSpace(entidad.Username))
                throw new Exception("el username es obligatorio");
            var u = entidad.Username.Trim();
            if (u.Length > 50) throw new Exception("el username se excede de 50");
            if (u.Contains(' ')) throw new Exception("tiene espacios");

            // debe de tener menos de 200 caracteres
            if (string.IsNullOrWhiteSpace(entidad.HashPass))
                throw new Exception("el hashpass es obligatorio.");
            if (entidad.HashPass.Trim().Length > 200)
                throw new Exception("El hashpass excede 200 caracteres");

            // el id debe de ser positvo 
            if (entidad.RolId <= 0) throw new Exception("RolId inválido");
        }

        private static string NormalizarUser(string username)
            => username.Trim(); 
    }
}
