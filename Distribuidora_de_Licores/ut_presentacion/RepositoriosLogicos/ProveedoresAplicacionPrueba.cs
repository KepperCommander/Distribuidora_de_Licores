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
    public class ProveedoresAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IProveedoresAplicacion provApp;
        private Proveedores? prov;

        public ProveedoresAplicacionPrueba()
        {
            iConexion = new Conexion();
            provApp = new ProveedoresAplicacion(iConexion);
            provApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorNombreOCiudadPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            
            prov = new Proveedores
            {
                ProveedorId = 0,
                Nombre = "ProveedorAlgo " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Ciudad = "Medellín",
                Telefono = "3001234567"
            };

            provApp.Guardar(prov);

         
            if (prov.ProveedorId == 0)
            {
                var rec = iConexion.Proveedores!.FirstOrDefault(x => x.Nombre == prov.Nombre);
                if (rec != null) prov.ProveedorId = rec.ProveedorId;
            }
            return prov.ProveedorId > 0;
        }

        public bool ModificarPrueba()
        {
            prov!.Telefono = "6041234567";
            provApp.Modificar(prov);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = provApp.Listar();
            return lista.Count > 0 && iConexion.Proveedores!.Any(x => x.ProveedorId == prov!.ProveedorId);
        }

        public bool PorNombreOCiudadPrueba()
        {
            var encontrados = provApp.PorNombreOCiudad(new Proveedores { Nombre = prov!.Nombre, Ciudad = prov.Ciudad });
            return encontrados.Any(x => x.ProveedorId == prov!.ProveedorId);
        }

        public bool BorrarPrueba()
        {
            provApp.Borrar(prov!);
            var existe = iConexion.Proveedores!.Any(x => x.ProveedorId == prov!.ProveedorId);
            return !existe;
        }
    }
}
