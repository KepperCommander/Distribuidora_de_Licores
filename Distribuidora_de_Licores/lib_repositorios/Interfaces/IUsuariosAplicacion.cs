using System;
using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IUsuariosAplicacion
    {
        void Configurar(string StringConexion);

        List<Usuarios> PorUsername(Usuarios? entidad);
        List<Usuarios> Listar();

        Usuarios? Guardar(Usuarios? entidad);
        Usuarios? Modificar(Usuarios? entidad);
        Usuarios? Borrar(Usuarios? entidad);
    }
}
