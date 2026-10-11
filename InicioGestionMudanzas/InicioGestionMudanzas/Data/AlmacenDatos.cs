using InicioGestionMudanzas.Models;

namespace InicioGestionMudanzas.Data
{
    public class AlmacenDatos
    {
        public static List<Cliente> Clientes { get; set; } = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Nombre = "Carlos Pérez",
                Cedula = "00112345678",
                Telefono = "8095551234",
                Direccion = "Santo Domingo"
            },
            new Cliente
            {
                Id = 2,
                Nombre = "María Rodríguez",
                Cedula = "00212345678",
                Telefono = "8295551234",
                Direccion = "Santiago"
            }
        };
    }
}
