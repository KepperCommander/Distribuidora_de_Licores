using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;
using lib_dominio.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class LotesPrueba
    {
        private readonly IConexion? iConexion;
        private List<Lotes>? lista;
        private Lotes? entidad;

        public LotesPrueba()
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
            this.lista = this.iConexion!.Lotes!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
         
            this.entidad = EntidadesNucleo.Lotes()!;
            this.iConexion!.Lotes!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return this.entidad.LoteId > 0;
        }

        public bool Modificar()
        {
            // solo se ajusta cantidad y vencimiento
            this.entidad!.Cantidad += 5;
            this.entidad!.Vencimiento = (this.entidad!.Vencimiento ?? DateTime.Today).AddMonths(1);

            var entry = this.iConexion!.Entry<Lotes>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Lotes!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
