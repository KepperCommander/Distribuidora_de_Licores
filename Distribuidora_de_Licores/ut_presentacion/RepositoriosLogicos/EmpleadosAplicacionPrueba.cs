using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.RepositoriosLogicos
{
    [TestClass]
    public class EmpleadosAplicacionPrueba
    {
        private readonly IConexion iConexion;
        private readonly IEmpleadosAplicacion empApp;
        private Empleados? emp;

        public EmpleadosAplicacionPrueba()
        {
            iConexion = new Conexion();
            empApp = new EmpleadosAplicacion(iConexion);
            empApp.Configurar(Configuracion.ObtenerValor("StringConexion"));
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, GuardarPrueba());
            Assert.AreEqual(true, ModificarPrueba());
            Assert.AreEqual(true, ListarPrueba());
            Assert.AreEqual(true, PorNombresPrueba());
            Assert.AreEqual(true, BorrarPrueba());
        }

        public bool GuardarPrueba()
        {
            
            emp = new Empleados
            {
                EmpleadoId = 0,
                SucursalId = 1,                        
                UsuarioId = null,                   
                Nombres = "nombrealgo " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Apellidos = "apellidoalgo",
                Cargo = "Vendedor"
            };

            empApp.Guardar(emp);

           
            if (emp.EmpleadoId == 0)
            {
                var rec = iConexion.Empleados!.FirstOrDefault(x => x.Nombres == emp.Nombres && x.Apellidos == emp.Apellidos);
                if (rec != null) emp.EmpleadoId = rec.EmpleadoId;
            }
            return emp.EmpleadoId > 0;
        }

        public bool ModificarPrueba()
        {
            emp!.Cargo = "Cajero";
            empApp.Modificar(emp);
            return true;
        }

        public bool ListarPrueba()
        {
            var lista = empApp.Listar();
            return lista.Count > 0 && iConexion.Empleados!.Any(x => x.EmpleadoId == emp!.EmpleadoId);
        }

        public bool PorNombresPrueba()
        {
            var encontrados = empApp.PorNombres(new Empleados { Nombres = emp!.Nombres });
            return encontrados.Any(x => x.EmpleadoId == emp!.EmpleadoId);
        }

        public bool BorrarPrueba()
        {
            empApp.Borrar(emp!);
            var existe = iConexion.Empleados!.Any(x => x.EmpleadoId == emp!.EmpleadoId);
            return !existe;
        }
    }
}
