using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IProveedoresAplicacion
    {
        void Configurar(string StringConexion);
        List<Proveedores> PorNombreOCiudad(Proveedores? entidad); // filtra por Nombre o Ciudad
        List<Proveedores> Listar();
        Proveedores? Guardar(Proveedores? entidad);
        Proveedores? Modificar(Proveedores? entidad);
        Proveedores? Borrar(Proveedores? entidad);
    }
}
