using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IVentasAplicacion
    {
        void Configurar(string StringConexion);
        List<Ventas> PorCliente(Ventas? entidad); // filtra por ClienteId
        List<Ventas> Listar();
        Ventas? Guardar(Ventas? entidad);
        Ventas? Modificar(Ventas? entidad);
        Ventas? Borrar(Ventas? entidad);
    }
}
