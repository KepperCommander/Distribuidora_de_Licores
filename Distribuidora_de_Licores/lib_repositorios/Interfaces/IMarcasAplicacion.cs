using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IMarcasAplicacion
    {
        void Configurar(string StringConexion);
        List<Marcas> PorNombre(Marcas? entidad);
        List<Marcas> Listar();
        Marcas? Guardar(Marcas? entidad);
        Marcas? Modificar(Marcas? entidad);
        Marcas? Borrar(Marcas? entidad);
    }
}
