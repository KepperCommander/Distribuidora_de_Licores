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
    public class LotesAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly ILotesAplicacion lotesApp;
        private Lotes? lote;

        public LotesAplicacionPrueba()
        {
            iConexion = new Conexion();
            lotesApp = new LotesAplicacion(iConexion);
            lotesApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorProductoOCodigoPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            
            lote = new Lotes
            {
                LoteId = 0,
                ProductoId = 1,  
                ProveedorId = 1,  
                CodigoLote = "codigo-lotealgo" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Vencimiento = DateTime.Today.AddMonths(12),
                Cantidad = 100
            };

            
            lote = lotesApp.Guardar(lote);
            return lote.LoteId > 0;
        }

        public bool ModificarPrueba()
        {
            lote!.Cantidad += 50;  // nuevo total
            lotesApp.Modificar(lote);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = lotesApp.Listar();
            return lista.Count > 0 && iConexion.Lotes!.Any(x => x.LoteId == lote!.LoteId);
        }

        public bool PorProductoOCodigoPrueba()
        {
            var encontrados = lotesApp.PorCodigo(new Lotes
            {
                ProductoId = lote!.ProductoId,
                CodigoLote = lote.CodigoLote
            });
            return encontrados.Any(x => x.LoteId == lote!.LoteId);
        }

        public bool BorrarPrueba()
        {
            lotesApp.Borrar(lote!);
            var existe = iConexion.Lotes!.Any(x => x.LoteId == lote!.LoteId);
            return !existe;
        }
    }
}
