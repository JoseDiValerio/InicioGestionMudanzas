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

        public static List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>
        {
            new Vehiculo
            {
                Id = 1,
                Matricula = "A123456",
                Marca = "Toyota",
                Modelo = "Hilux",
                CapacidadCarga = 1000,
                Estado = "Disponible"
            },
            new Vehiculo
            {
                Id = 2,
                Matricula = "B654321",
                Marca = "Nissan",
                Modelo = "Frontier",
                CapacidadCarga = 1200,
                Estado = "En mantenimiento"
            }
        };

        public static List<Empleado> Empleados { get; set; } = new List<Empleado>
        {
            new Empleado
            {
                Id = 1,
                Nombre = "Luis Martínez",
                Cedula = "00312345678",
                Telefono = "8091112233",
                Cargo = "Conductor"
            },
            new Empleado
            {
                Id = 2,
                Nombre = "Ana Gómez",
                Cedula = "00412345678",
                Telefono = "8291112233",
                Cargo = "Ayudante"
            }
        };
    }
}
