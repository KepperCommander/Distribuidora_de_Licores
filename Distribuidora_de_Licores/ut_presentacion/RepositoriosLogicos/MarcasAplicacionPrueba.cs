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
    public class MarcasAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IMarcasAplicacion marcasApp;
        private Marcas? marca;

        public MarcasAplicacionPrueba()
        {
            iConexion = new Conexion();
            marcasApp = new MarcasAplicacion(iConexion);
            marcasApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
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
            marca = new Marcas
            {
                MarcaId = 0,
                Nombre = "MArcaAlgo " + DateTime.Now.ToString("yyyyMMddHHmmssfff")
            };

            marcasApp.Guardar(marca);
            return marca.MarcaId > 0;
        }

        public bool ModificarPrueba()
        {
            marca!.Nombre = marca.Nombre + "marcanombre";
            marcasApp.Modificar(marca);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = marcasApp.Listar();
            return lista.Count > 0 && iConexion.Marcas!.Any(x => x.MarcaId == marca!.MarcaId);
        }

        public bool PorNombrePrueba()
        {
            var encontrados = marcasApp.PorNombre(new Marcas { Nombre = marca!.Nombre });
            return encontrados.Any(x => x.MarcaId == marca!.MarcaId);
        }

        public bool BorrarPrueba()
        {
            marcasApp.Borrar(marca!);
            var existe = iConexion.Marcas!.Any(x => x.MarcaId == marca!.MarcaId);
            return !existe;
        }
    }
}
