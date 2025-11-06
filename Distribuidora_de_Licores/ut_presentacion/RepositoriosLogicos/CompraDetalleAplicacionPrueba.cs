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
    public class CompraDetalleAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly ICompraDetalleAplicacion detApp;

        private Compras? compra;              
        private CompraDetalle? det;           

        public CompraDetalleAplicacionPrueba()
        {
            iConexion = new Conexion();
            detApp = new CompraDetalleAplicacion(iConexion);
            detApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(PrepararCompraCabecera(), "Fallo creando compra cabecera");
            Assert.IsTrue(GuardarPrueba(), "Fallo en GuardarPrueba");
            Assert.IsTrue(ModificarPrueba(), "Fallo en ModificarPrueba");
            Assert.IsTrue(ListarPrueba(), "Fallo en ListarPrueba");
            Assert.IsTrue(PorCompraPrueba(), "Fallo en PorCompraPrueba");
            Assert.IsTrue(BorrarPrueba(), "Fallo en BorrarPrueba");
        }

        
        private bool PrepararCompraCabecera()
        {
            
            compra = new Compras
            {
                CompraId = 0,
                ProveedorId = 1,
                SucursalId = 1,
                Fecha = DateTime.Today,
                Total = 0m
            };
            iConexion.Compras!.Add(compra);
            iConexion.SaveChanges();
            return compra.CompraId > 0;
        }

        public bool GuardarPrueba()
        {
           
            det = new CompraDetalle
            {
                CompraDetId = 0,
                CompraId = compra!.CompraId,
                ProductoId = 1,
                Cantidad = 10,
                PrecioCompra = 8500m
                // Subtotal lo calcula sql
            };

            det = detApp.Guardar(det);

            if (det!.CompraDetId == 0)
            {
                var rec = iConexion.CompraDetalle!
                    .FirstOrDefault(x => x.CompraId == det.CompraId &&
                                         x.ProductoId == det.ProductoId);
                if (rec != null) det.CompraDetId = rec.CompraDetId;
            }
            return det.CompraDetId > 0;
        }

        public bool ModificarPrueba()
        {
            det!.Cantidad += 5;            // 15 unidades
            det!.PrecioCompra += 500m;     // 9000
            detApp.Modificar(det);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = detApp.Listar();
            return lista.Count > 0 && iConexion.CompraDetalle!.Any(x => x.CompraDetId == det!.CompraDetId);
        }

        public bool PorCompraPrueba()
        {
            var encontrados = detApp.PorCompra(new CompraDetalle { CompraId = compra!.CompraId });
            return encontrados.Any(x => x.CompraDetId == det!.CompraDetId);
        }

        public bool BorrarPrueba()
        {
            detApp.Borrar(det!);
            var existe = iConexion.CompraDetalle!.Any(x => x.CompraDetId == det!.CompraDetId);

            
            iConexion.Compras!.Remove(compra!);
            iConexion.SaveChanges();

            return !existe;
        }
    }
}
