using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class CompraDetallePrueba
    {
        private readonly IConexion? iConexion;
        private List<CompraDetalle>? lista;
        private CompraDetalle? entidad;

        public CompraDetallePrueba()
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
            //Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.CompraDetalle!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.CompraDetalle()!;
            this.iConexion!.CompraDetalle!.Add(this.entidad);
            this.iConexion!.SaveChanges();


            return this.entidad.CompraDetId > 0;
        }

        public bool Modificar()
        {
          
            this.entidad!.Cantidad = this.entidad.Cantidad + 1;

            var entry = this.iConexion!.Entry<CompraDetalle>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.CompraDetalle!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
