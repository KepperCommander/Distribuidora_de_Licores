using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IClientesAplicacion
    {
        void Configurar(string StringConexion);
        List<Clientes> PorRazonSocialONit(Clientes? entidad); // filtra por RazonSocial o NIT
        List<Clientes> Listar();
        Clientes? Guardar(Clientes? entidad);
        Clientes? Modificar(Clientes? entidad);
        Clientes? Borrar(Clientes? entidad);
    }
}
