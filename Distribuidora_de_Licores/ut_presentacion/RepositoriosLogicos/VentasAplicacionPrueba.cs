using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class VentasAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IVentasAplicacion ventasApp;
        private Ventas? venta;

        public VentasAplicacionPrueba()
        {
            iConexion = new Conexion();
            ventasApp = new VentasAplicacion(iConexion);
            ventasApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(GuardarPrueba(), "Fallo en GuardarPrueba");
            Assert.IsTrue(ModificarPrueba(), "Fallo en ModificarPrueba");
            Assert.IsTrue(ListarPrueba(), "Fallo en ListarPrueba");
            Assert.IsTrue(PorClientePrueba(), "Fallo en PorClientePrueba");
            Assert.IsTrue(BorrarPrueba(), "Fallo en BorrarPrueba");
        }

        public bool GuardarPrueba()
        {
            var clienteId = iConexion.Clientes!.Select(c => c.ClienteId).First();
            var sucursalId = iConexion.Sucursales!.Select(s => s.SucursalId).First();
            var empleadoId = iConexion.Empleados!.Select(e => e.EmpleadoId).First();

            venta = new Ventas
            {
                ClienteId = clienteId,
                SucursalId = sucursalId,
                EmpleadoId = empleadoId,
                Fecha = DateTime.Today,
                Total = 250000m
            };

            venta = ventasApp.Guardar(venta);   // EF asigna VentaId
            return venta!.VentaId > 0;
        }



        public bool ModificarPrueba()
        {
            venta!.Total += 100000m;
            ventasApp.Modificar(venta);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = ventasApp.Listar();
            return lista.Count > 0 && iConexion.Ventas!.Any(v => v.VentaId == venta!.VentaId);
        }

        public bool PorClientePrueba()
        {
            var encontrados = ventasApp.PorCliente(new Ventas { ClienteId = venta!.ClienteId });
            return encontrados.Any(v => v.VentaId == venta!.VentaId);
        }

        public bool BorrarPrueba()
        {
            ventasApp.Borrar(venta!);
            var existe = iConexion.Ventas!.Any(v => v.VentaId == venta!.VentaId);
            return !existe;
        }
    }
}
