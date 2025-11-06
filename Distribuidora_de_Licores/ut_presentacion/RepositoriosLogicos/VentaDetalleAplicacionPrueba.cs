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
    public class VentaDetalleAplicacionPrueba
    {
        private readonly IConexion cx;
        private readonly IVentaDetalleAplicacion app;

        private Ventas? venta;              
        private VentaDetalle? det;          

        public VentaDetalleAplicacionPrueba()
        {
            cx = new Conexion();
            app = new VentaDetalleAplicacion(cx);
            app.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(PrepararVentaCabecera(), "Fallo creando cabecera de venta");
            Assert.IsTrue(Guardar(), "Fallo en Guardar");
            Assert.IsTrue(Modificar(), "Fallo en Modificar");
            Assert.IsTrue(Listar(), "Fallo en Listar");
            Assert.IsTrue(PorVenta(), "Fallo en PorVenta");
            Assert.IsTrue(Borrar(), "Fallo en Borrar");
        }

        
        private bool PrepararVentaCabecera()
        {
            // coge ids de los fK para que sean validos
            var clienteId = cx.Clientes!.Select(x => x.ClienteId).First();
            var sucursalId = cx.Sucursales!.Select(x => x.SucursalId).First();
            var empleadoId = cx.Empleados!.Select(x => x.EmpleadoId).First();

            venta = new Ventas
            {
                VentaId = 0,
                ClienteId = clienteId,
                SucursalId = sucursalId,
                EmpleadoId = empleadoId,
                Fecha = DateTime.Today,
                Total = 0m
            };

            cx.Ventas!.Add(venta);
            cx.SaveChanges();

            return venta.VentaId > 0;
        }

        private bool Guardar()
        {
            // se busca un producto que sea valido
            var productoId = cx.Productos!.Select(p => p.ProductoId).First();

            det = new VentaDetalle
            {
                VentaDetId = 0,
                VentaId = venta!.VentaId,
                ProductoId = productoId,
                Cantidad = 3,
                PrecioVenta = 12000m
                // Subtotal lo calcula sql
            };

            
            det = app.Guardar(det);
           
            return det.VentaDetId > 0;
        }

        private bool Modificar()
        {
            det!.Cantidad += 2;     // 5
            det!.PrecioVenta += 500m;  // 12500
            app.Modificar(det);
            return true;
        }

        private bool Listar()
        {
            var lista = app.Listar();
            return lista.Count > 0 && cx.VentaDetalle!.Any(x => x.VentaDetId == det!.VentaDetId);
        }

        private bool PorVenta()
        {
            var encontrados = app.PorVenta(new VentaDetalle { VentaId = venta!.VentaId });
            return encontrados.Any(x => x.VentaDetId == det!.VentaDetId);
        }

        private bool Borrar()
        {
            // Borrar detalle
            app.Borrar(det!);
            var existeDet = cx.VentaDetalle!.Any(x => x.VentaDetId == det!.VentaDetId);

            cx.Ventas!.Remove(venta!);
            cx.SaveChanges();

            return !existeDet;
        }
    }
}
