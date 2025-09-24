using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IInventarioAplicacion
    {
        void Configurar(string StringConexion);
        List<Inventario> PorSucursalProducto(Inventario? entidad); // filtra por SucursalId/ProductoId
        List<Inventario> Listar();
        Inventario? Guardar(Inventario? entidad);
        Inventario? Modificar(Inventario? entidad);
        Inventario? Borrar(Inventario? entidad);
    }
}
