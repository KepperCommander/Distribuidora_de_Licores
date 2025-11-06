using System;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;
using lib_dominio.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class UsuariosAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IUsuariosAplicacion usuariosApp;
        private Usuarios? usu;

        public UsuariosAplicacionPrueba()
        {
            iConexion = new Conexion();
            usuariosApp = new UsuariosAplicacion(iConexion);
            usuariosApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorUsernamePrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
 
            usu = EntidadesNucleo.Usuarios() ?? new Usuarios();
            usu.UsuarioId = 0;
            if (usu.RolId <= 0) usu.RolId = 1;                         // con esto se asume que existe rol con id 1
            var uniq = Guid.NewGuid().ToString("N").Substring(0, 10);
            usu.Username = $"ut_user_{uniq}";
            if (string.IsNullOrWhiteSpace(usu.HashPass)) usu.HashPass = "hash_demo_123";
            usu.Activo = true;

            usuariosApp.Guardar(usu);

            if (usu.UsuarioId == 0)
            {
                var rec = iConexion.Usuarios!.FirstOrDefault(x => x.Username == usu.Username);
                if (rec != null) usu.UsuarioId = rec.UsuarioId;
            }
            return usu.UsuarioId > 0;
        }

        public bool ModificarPrueba()
        {
            // cambia un rol que exista 
            usu!.RolId = (usu.RolId == 1 ? 2 : 1); // alterna entre 1 y 2
            usu.Activo = false;
            usuariosApp.Modificar(usu);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = usuariosApp.Listar();
            return lista.Count > 0 && iConexion.Usuarios!.Any(x => x.UsuarioId == usu!.UsuarioId);
        }

        public bool PorUsernamePrueba()
        {
            var lista = usuariosApp.PorUsername(new Usuarios { Username = usu!.Username });
            return lista.Any(x => x.UsuarioId == usu!.UsuarioId);
        }

        public bool BorrarPrueba()
        {
            usuariosApp.Borrar(usu!);
            var existe = iConexion.Usuarios!.Any(x => x.UsuarioId == usu!.UsuarioId);
            return !existe;
        }
    }
}
