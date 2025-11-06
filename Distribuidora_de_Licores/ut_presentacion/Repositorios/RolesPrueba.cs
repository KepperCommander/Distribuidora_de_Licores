using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;
using lib_dominio.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class RolesPrueba
    {
        private readonly IConexion? iConexion;
        private List<Roles>? lista;
        private Roles? entidad;

        public RolesPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

       

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.Roles!.ToList();
            return this.lista.Any(r => r.RolId == this.entidad!.RolId);
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Roles()!;
            this.iConexion!.Roles!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return this.entidad.RolId > 0;
        }

        public bool Modificar()
        {
            this.entidad!.Descripcion = "Rol modificado en prueba";
            var entry = this.iConexion!.Entry<Roles>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Roles!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
