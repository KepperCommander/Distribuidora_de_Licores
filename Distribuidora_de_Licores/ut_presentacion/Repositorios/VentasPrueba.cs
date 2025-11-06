using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;
using lib_dominio.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class VentasPrueba
    {
        private readonly IConexion? iConexion;
        private List<Ventas>? lista;
        private Ventas? entidad;

        public VentasPrueba()
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
            this.lista = this.iConexion!.Ventas!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Ventas()!;
            this.iConexion!.Ventas!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return this.entidad.VentaId > 0;
        }

        public bool Modificar()
        {
            this.entidad!.Total = (this.entidad.Total == 0m ? 150000m : this.entidad.Total + 1000m); //para actualizar el total

            var entry = this.iConexion!.Entry<Ventas>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Ventas!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
