using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class InventarioAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IInventarioAplicacion invApp;
        private Inventario? inv;

        public InventarioAplicacionPrueba()
        {
            iConexion = new Conexion();
            invApp = new InventarioAplicacion(iConexion);
            invApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorSucursalProductoPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            inv = new Inventario
            {
                InventarioId = 0,
                SucursalId = 1,
                ProductoId = 1,
                Stock = 10
            };

            
            inv = invApp.Guardar(inv);

            // posible error del entity framework no asig id, se recarga de la bd
            if (inv!.InventarioId == 0)
            {
                inv = iConexion.Inventario!
                       .FirstOrDefault(x => x.SucursalId == inv.SucursalId && x.ProductoId == inv.ProductoId);
            }
            return inv != null && inv.InventarioId > 0;
        }


        public bool ModificarPrueba()
        {
            inv!.Stock += 5;
            invApp.Modificar(inv);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = invApp.Listar();
            return lista.Count > 0 && iConexion.Inventario!.Any(x => x.InventarioId == inv!.InventarioId);
        }

        public bool PorSucursalProductoPrueba()
        {
            var encontrados = invApp.PorSucursalProducto(new Inventario
            {
                SucursalId = inv!.SucursalId,
                ProductoId = inv.ProductoId
            });
            return encontrados.Any(x => x.InventarioId == inv!.InventarioId);
        }

        public bool BorrarPrueba()
        {
            invApp.Borrar(inv!);
            var existe = iConexion.Inventario!.Any(x => x.InventarioId == inv!.InventarioId);
            return !existe;
        }
    }
}
