using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class InventarioPrueba
    {
        private readonly IConexion? iConexion;
        private List<Inventario>? lista;
        private Inventario? entidad;

        public InventarioPrueba()
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
            this.lista = this.iConexion!.Inventario!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
     
            this.entidad = EntidadesNucleo.Inventario()!;

            var existente = this.iConexion!.Inventario! //guarda si existe esa comn¿binacion
                .FirstOrDefault(x => x.SucursalId == this.entidad.SucursalId &&
                                     x.ProductoId == this.entidad.ProductoId);

            if (existente != null)
            {
                this.entidad = existente;
                return true;
            }

        
            this.iConexion!.Inventario!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return this.entidad.InventarioId > 0;
        }

        public bool Modificar()
        {
            this.entidad!.Stock += 10; // aumentar stock como prueba
            var entry = this.iConexion!.Entry<Inventario>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Inventario!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
