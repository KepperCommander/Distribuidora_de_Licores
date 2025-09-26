using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class ComprasAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IComprasAplicacion comprasApp;
        private Compras? compra;

        public ComprasAplicacionPrueba()
        {
            iConexion = new Conexion();
            comprasApp = new ComprasAplicacion(iConexion);
            comprasApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorProveedorPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
           
            compra = new Compras
            {
                CompraId = 0,
                ProveedorId = 1,
                SucursalId = 1,
                Fecha = DateTime.Today,
                Total = 150000m
            };

            compra = comprasApp.Guardar(compra);
            return compra.CompraId > 0;
        }

        public bool ModificarPrueba()
        {
            compra!.Total += 50000m;
            comprasApp.Modificar(compra);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = comprasApp.Listar();
            return lista.Count > 0 && iConexion.Compras!.Any(c => c.CompraId == compra!.CompraId);
        }

        public bool PorProveedorPrueba()
        {
            var encontrados = comprasApp.PorProveedor(new Compras
            {
                ProveedorId = compra!.ProveedorId
            });
            return encontrados.Any(c => c.CompraId == compra!.CompraId);
        }


        public bool BorrarPrueba()
        {
            comprasApp.Borrar(compra!);
            var existe = iConexion.Compras!.Any(c => c.CompraId == compra!.CompraId);
            return !existe;
        }
    }
}
