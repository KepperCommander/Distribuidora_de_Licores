using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IComprasAplicacion
    {
        void Configurar(string StringConexion);
        List<Compras> PorProveedor(Compras? entidad); 
        List<Compras> Listar();
        Compras? Guardar(Compras? entidad);
        Compras? Modificar(Compras? entidad);
        Compras? Borrar(Compras? entidad);
    }
}
