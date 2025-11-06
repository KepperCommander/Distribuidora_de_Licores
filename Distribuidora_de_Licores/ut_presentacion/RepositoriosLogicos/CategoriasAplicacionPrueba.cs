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
    public class CategoriasAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly ICategoriasAplicacion catApp;
        private Categorias? cat;

        public CategoriasAplicacionPrueba()
        {
            iConexion = new Conexion();
            catApp = new CategoriasAplicacion(iConexion);
            catApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
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
            cat = new Categorias
            {
                CategoriaId = 0,
                Nombre = "CategoriaAlgo " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Descripcion = "Categoría de prueba"
            };

            catApp.Guardar(cat);

            if (cat.CategoriaId == 0)
            {
                var rec = iConexion.Categorias!.FirstOrDefault(x => x.Nombre == cat.Nombre);
                if (rec != null) cat.CategoriaId = rec.CategoriaId;
            }
            return cat.CategoriaId > 0;
        }

        public bool ModificarPrueba()
        {
            cat!.Descripcion = "Categoría modificada en prueba";
            catApp.Modificar(cat);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = catApp.Listar();
            return lista.Count > 0 && iConexion.Categorias!.Any(x => x.CategoriaId == cat!.CategoriaId);
        }

        public bool PorNombrePrueba()
        {
            var encontrados = catApp.PorNombre(new Categorias { Nombre = cat!.Nombre });
            return encontrados.Any(x => x.CategoriaId == cat!.CategoriaId);
        }

        public bool BorrarPrueba()
        {
            catApp.Borrar(cat!);
            var existe = iConexion.Categorias!.Any(x => x.CategoriaId == cat!.CategoriaId);
            return !existe;
        }
    }
}
