using System;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class RolesAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IRolesAplicacion rolesApp;
        private Roles? rol;

        public RolesAplicacionPrueba()
        {
            iConexion = new Conexion();
            rolesApp = new RolesAplicacion(iConexion);
            rolesApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorNombrePrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            
            rol = EntidadesNucleo.Roles() ?? new Roles();
            rol.RolId = 0;
            var stamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            rol.Nombre = $"estandarRol--{stamp}";     // < 50 chars y único
            rol.Descripcion ??= "Rol de prueba";

            rolesApp.Guardar(rol);

            //si no tiene rolid se busca por nombre
            if (rol.RolId == 0)
            {
                var rec = iConexion.Roles!.FirstOrDefault(r => r.Nombre == rol.Nombre);
                if (rec != null) rol.RolId = rec.RolId;
            }
            return rol.RolId > 0;
        }

        public bool ModificarPrueba()
        {
            rol!.Descripcion = "rol modificado en prueba";
            rolesApp.Modificar(rol);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = rolesApp.Listar();
            // que exista y que esté el que se acaba de crear
            return lista.Count > 0 && iConexion.Roles!.Any(r => r.RolId == rol!.RolId);
        }

        public bool PorNombrePrueba()
        {
            var encontrados = rolesApp.PorNombre(new Roles { Nombre = rol!.Nombre });
            return encontrados.Any(r => r.RolId == rol!.RolId);
        }

        public bool BorrarPrueba()
        {
            rolesApp.Borrar(rol!);
            var existe = iConexion.Roles!.Any(r => r.RolId == rol!.RolId);
            return !existe;
        }
    }
}
