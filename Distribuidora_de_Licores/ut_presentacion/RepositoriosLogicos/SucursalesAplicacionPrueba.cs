using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class SucursalesAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly ISucursalesAplicacion sucApp;
        private Sucursales? suc;

        public SucursalesAplicacionPrueba()
        {
            iConexion = new Conexion();
            sucApp = new SucursalesAplicacion(iConexion);
            sucApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
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
            // Dato mínimo válido (la validación está en la implementación)
            suc = new Sucursales
            {
                SucursalId = 0,
                Nombre = "Sucursal UT " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Ciudad = "Medellín",
                Direccion = "Calle 10 # 20-30"
            };

            sucApp.Guardar(suc);

            // Si el tracking no reflejó el Id, lo recupero
            if (suc.SucursalId == 0)
            {
                var rec = iConexion.Sucursales!.FirstOrDefault(x => x.Nombre == suc.Nombre && x.Ciudad == suc.Ciudad);
                if (rec != null) suc.SucursalId = rec.SucursalId;
            }
            return suc.SucursalId > 0;
        }

        public bool ModificarPrueba()
        {
            suc!.Direccion = "Dirección actualizada";
            sucApp.Modificar(suc);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = sucApp.Listar();
            return lista.Count > 0 && iConexion.Sucursales!.Any(x => x.SucursalId == suc!.SucursalId);
        }

        public bool PorNombrePrueba()
        {
            var encontrados = sucApp.PorNombre(new Sucursales { Nombre = suc!.Nombre });
            return encontrados.Any(x => x.SucursalId == suc!.SucursalId);
        }

        public bool BorrarPrueba()
        {
            sucApp.Borrar(suc!);
            var existe = iConexion.Sucursales!.Any(x => x.SucursalId == suc!.SucursalId);
            return !existe;
        }
    }
}
