using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class ClientesAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IClientesAplicacion cliApp;
        private Clientes? cli;

        public ClientesAplicacionPrueba()
        {
            iConexion = new Conexion();
            cliApp = new ClientesAplicacion(iConexion);
            cliApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorRazonSocialONitPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            
            cli = new Clientes
            {
                ClienteId = 0,
                Tipo = "Minorista",
                RazonSocial = "cleintealgo " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                NIT = "900123456",
                Telefono = "3001234567",
                Ciudad = "Medellín"
            };

            cliApp.Guardar(cli);

            // Si el tracking no reflejó el Id:
            /*if (cli.ClienteId == 0)
            {
                var rec = iConexion.Clientes!.FirstOrDefault(x => x.RazonSocial == cli.RazonSocial);
                if (rec != null) cli.ClienteId = rec.ClienteId;
            }*/
            return cli.ClienteId > 0;
        }

        public bool ModificarPrueba()
        {
            cli!.Telefono = "6041234567";
            cliApp.Modificar(cli);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = cliApp.Listar();
            return lista.Count > 0 && iConexion.Clientes!.Any(x => x.ClienteId == cli!.ClienteId);
        }

        public bool PorRazonSocialONitPrueba()
        {
            var encontrados = cliApp.PorRazonSocialONit(new Clientes { RazonSocial = cli!.RazonSocial, NIT = cli.NIT });
            return encontrados.Any(x => x.ClienteId == cli!.ClienteId);
        }

        public bool BorrarPrueba()
        {
            cliApp.Borrar(cli!);
            var existe = iConexion.Clientes!.Any(x => x.ClienteId == cli!.ClienteId);
            return !existe;
        }
    }
}
