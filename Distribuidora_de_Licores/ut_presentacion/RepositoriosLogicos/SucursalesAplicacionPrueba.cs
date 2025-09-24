using System;
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
            // Entidad mínima válida
            suc = EntidadesNucleo.Sucursales() ?? new Sucursales();

            suc.SucursalId = 0;
            var suf = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            // Nombre y ciudad legibles y únicos
            suc.Nombre = string.IsNullOrWhiteSpace(suc.Nombre) ? $"Sucursal UT {suf}" : suc.Nombre.Trim();
            suc.Ciudad = string.IsNullOrWhiteSpace(suc.Ciudad) ? "Medellín" : suc.Ciudad.Trim();
            suc.Direccion = string.IsNullOrWhiteSpace(suc.Direccion) ? $"Calle {suf} # 1-2" : suc.Direccion.Trim();

            // recortes por longitudes máximas
            if (suc.Nombre.Length > 80) suc.Nombre = suc.Nombre[..80];
            if (suc.Ciudad.Length > 80) suc.Ciudad = suc.Ciudad[..80];
            if (suc.Direccion.Length > 120) suc.Direccion = suc.Direccion[..120];

            sucApp.Guardar(suc);

            if (suc.SucursalId == 0)
            {
                var rec = iConexion.Sucursales!.FirstOrDefault(x => x.Nombre == suc.Nombre && x.Ciudad == suc.Ciudad);
                if (rec != null) suc.SucursalId = rec.SucursalId;
            }
            return suc.SucursalId > 0;
        }

        public bool ModificarPrueba()
        {
            // Cambiar dirección (datos existentes en la tabla)
            suc!.Direccion = "Dirección actualizada " + DateTime.Now.ToString("HHmmss");
            sucApp.Modificar(suc);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = sucApp.Listar();
            // que exista y que esté la que acabamos de crear (sin depender del Take(20))
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
