using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Roles? Roles()
        {
            var entidad = new Roles();
            entidad.Nombre = "Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Descripcion = "Rol de pruebas";
            return entidad;
        }

        public static Usuarios? Usuarios()
        {
            var entidad = new Usuarios();
            entidad.RolId = 1;
            entidad.Username = "Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.HashPass = "HashPrueba";
            entidad.Activo = true;
            return entidad;
        }

        public static Sucursales? Sucursales()
        {
            var entidad = new Sucursales();
            entidad.Nombre = "Sucursal Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Ciudad = "CiudadPrueba";
            entidad.Direccion = "Calle Falsa 123";
            return entidad;
        }

        public static Empleados? Empleados()
        {
            var entidad = new Empleados();
            entidad.SucursalId = 1;
            entidad.UsuarioId = 1;
            entidad.Nombres = "EmpleadoPrueba";
            entidad.Apellidos = "ApellidosPrueba";
            entidad.Cargo = "CargoPrueba";
            return entidad;
        }

        public static Proveedores? Proveedores()
        {
            var entidad = new Proveedores();
            entidad.Nombre = "Proveedor Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Ciudad = "CiudadPrueba";
            entidad.Telefono = "3000000000";
            return entidad;
        }

        public static Categorias? Categorias()
        {
            var entidad = new Categorias();
            entidad.Nombre = "CategoriaPrueba";
            entidad.Descripcion = "Descripcion de prueba";
            return entidad;
        }

        public static Marcas? Marcas()
        {
            var entidad = new Marcas();
            entidad.Nombre = "MarcaPrueba";
            return entidad;
        }

        public static Productos? Productos()
        {
            var entidad = new Productos();
            entidad.CategoriaId = 1;
            entidad.MarcaId = 1;
            entidad.ProveedorId = 1;
            entidad.Nombre = "Producto Prueba-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Unidad = "Botella";
            entidad.ContenidoML = 750;
            entidad.PrecioUnit = 10000;
            return entidad;
        }

        public static Clientes? Clientes()
        {
            var entidad = new Clientes();
            entidad.Tipo = "Bar";
            entidad.RazonSocial = "Cliente Prueba-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.NIT = "900999999-9";
            entidad.Telefono = "3111111111";
            entidad.Ciudad = "CiudadPrueba";
            return entidad;
        }

        public static Inventario? Inventario()
        {
            var entidad = new Inventario();
            entidad.SucursalId = 1;
            entidad.ProductoId = 1;
            entidad.Stock = 50;
            return entidad;
        }

        public static Lotes? Lotes()
        {
            var entidad = new Lotes();
            entidad.ProductoId = 1;
            entidad.ProveedorId = 1;
            entidad.CodigoLote = "LOTE-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Vencimiento = DateTime.Now.AddYears(2);
            entidad.Cantidad = 100;
            return entidad;
        }

        public static Compras? Compras()
        {
            var entidad = new Compras();
            entidad.ProveedorId = 1;
            entidad.SucursalId = 1;
            entidad.Fecha = DateTime.Now;
            entidad.Total = 0;
            return entidad;
        }

        public static CompraDetalle? CompraDetalle()
        {
            var entidad = new CompraDetalle();
            entidad.CompraId = 1;
            entidad.ProductoId = 1;
            entidad.Cantidad = 10;
            entidad.PrecioCompra = 20000;
            return entidad;
        }

        public static Ventas? Ventas()
        {
            var entidad = new Ventas();
            entidad.ClienteId = 1;
            entidad.SucursalId = 1;
            entidad.EmpleadoId = 1;
            entidad.Fecha = DateTime.Now;
            entidad.Total = 0;
            return entidad;
        }

        public static VentaDetalle? VentaDetalle()
        {
            var entidad = new VentaDetalle();
            entidad.VentaId = 1;
            entidad.ProductoId = 1;
            entidad.Cantidad = 2;
            entidad.PrecioVenta = 25000;
            return entidad;
        }
    }
}
