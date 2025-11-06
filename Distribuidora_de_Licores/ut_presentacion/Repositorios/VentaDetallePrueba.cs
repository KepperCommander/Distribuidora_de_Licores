using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;
using lib_dominio.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class VentaDetallePrueba
    {
        private readonly IConexion? iConexion;
        private List<VentaDetalle>? lista;
        private VentaDetalle? entidad;

        public VentaDetallePrueba()
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
            this.lista = this.iConexion!.VentaDetalle!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.VentaDetalle()!;
            this.iConexion!.VentaDetalle!.Add(this.entidad);
            this.iConexion!.SaveChanges();

      
            return this.entidad.VentaDetId > 0;
        }

        public bool Modificar()
        {
           
            this.entidad!.Cantidad = this.entidad.Cantidad + 1; //se hace un cambio minimo porq las otras propiedades se calcelulan solas en el sql (el total)
            var entry = this.iConexion!.Entry<VentaDetalle>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.VentaDetalle!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
