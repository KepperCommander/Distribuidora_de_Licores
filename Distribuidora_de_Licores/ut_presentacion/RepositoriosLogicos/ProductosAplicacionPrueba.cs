using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class ProductosAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IProductosAplicacion prodApp;
        private Productos? prod;

        public ProductosAplicacionPrueba()
        {
            iConexion = new Conexion();
            prodApp = new ProductosAplicacion(iConexion);
            prodApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorNombrePrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            // Dato mínimo válido (asegúrate de tener registros con Id=1 en las FKs)
            prod = new Productos
            {
                ProductoId = 0,
                CategoriaId = 1,   // Debe existir en dbo.Categorias
                MarcaId = 1,   // Debe existir en dbo.Marcas
                ProveedorId = 1,   // Debe existir en dbo.Proveedores
                Nombre = "UT-Producto " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Unidad = "Botella",
                ContenidoML = 750,
                PrecioUnit = 12000m
            };

            prodApp.Guardar(prod);

            // Si el tracking no reflejó el Id, lo recupero
            /*if (prod.ProductoId == 0)
            {
                var rec = iConexion.Productos!.FirstOrDefault(x => x.Nombre == prod.Nombre);
                if (rec != null) prod.ProductoId = rec.ProductoId;
            }*/
            return prod.ProductoId > 0;
        }

        public bool ModificarPrueba()
        {
            prod!.PrecioUnit = prod.PrecioUnit + 500m;
            prodApp.Modificar(prod);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = prodApp.Listar();
            return lista.Count > 0 && iConexion.Productos!.Any(x => x.ProductoId == prod!.ProductoId);
        }

        public bool PorNombrePrueba()
        {
            var encontrados = prodApp.PorNombre(new Productos { Nombre = prod!.Nombre });
            return encontrados.Any(x => x.ProductoId == prod!.ProductoId);
        }

        public bool BorrarPrueba()
        {
            prodApp.Borrar(prod!);
            var existe = iConexion.Productos!.Any(x => x.ProductoId == prod!.ProductoId);
            return !existe;
        }
    }
}
