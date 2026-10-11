using InicioGestionMudanzas.Data;
using InicioGestionMudanzas.Models;
using Microsoft.AspNetCore.Mvc;

namespace InicioGestionMudanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        // GET: api/Vehiculos
        // Devuelve todos los vehículos.
        [HttpGet]
        public IActionResult ObtenerVehiculos()
        {
            return Ok(AlmacenDatos.Vehiculos);
        }

        // GET: api/Vehiculos/1
        // Busca un vehículo por su ID.
        [HttpGet("{id}")]
        public IActionResult ObtenerVehiculoPorId(int id)
        {
            var vehiculo = AlmacenDatos.Vehiculos
                .FirstOrDefault(v => v.Id == id);

            if (vehiculo == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el vehículo."
                });
            }

            return Ok(vehiculo);
        }

        // GET: api/Vehiculos/disponibles
        // Devuelve solamente los vehículos disponibles.
        [HttpGet("disponibles")]
        public IActionResult ObtenerVehiculosDisponibles()
        {
            var vehiculos = AlmacenDatos.Vehiculos
                .Where(v => v.Estado == "Disponible")
                .ToList();

            return Ok(vehiculos);
        }

        // POST: api/Vehiculos
        // Registra un vehículo nuevo.
        [HttpPost]
        public IActionResult CrearVehiculo([FromBody] Vehiculo nuevoVehiculo)
        {
            if (string.IsNullOrWhiteSpace(nuevoVehiculo.Matricula) ||
                string.IsNullOrWhiteSpace(nuevoVehiculo.Marca) ||
                string.IsNullOrWhiteSpace(nuevoVehiculo.Modelo) ||
                nuevoVehiculo.CapacidadCarga <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "Completa los campos obligatorios y utiliza una capacidad de carga mayor que cero."
                });
            }

            string matricula = nuevoVehiculo.Matricula.Trim();

            bool matriculaExiste = AlmacenDatos.Vehiculos.Any(
                v => v.Matricula.Equals(
                    matricula,
                    StringComparison.OrdinalIgnoreCase));

            if (matriculaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un vehículo con esa matrícula."
                });
            }

            string estado = string.IsNullOrWhiteSpace(nuevoVehiculo.Estado)
                ? "Disponible"
                : nuevoVehiculo.Estado.Trim();

            if (!EsEstadoValido(estado))
            {
                return BadRequest(new
                {
                    mensaje = "El estado debe ser Disponible, En mantenimiento o Fuera de servicio."
                });
            }

            int nuevoId = AlmacenDatos.Vehiculos.Count == 0
                ? 1
                : AlmacenDatos.Vehiculos.Max(v => v.Id) + 1;

            nuevoVehiculo.Id = nuevoId;
            nuevoVehiculo.Matricula = matricula.ToUpperInvariant();
            nuevoVehiculo.Marca = nuevoVehiculo.Marca.Trim();
            nuevoVehiculo.Modelo = nuevoVehiculo.Modelo.Trim();
            nuevoVehiculo.Estado = estado;

            AlmacenDatos.Vehiculos.Add(nuevoVehiculo);

            return CreatedAtAction(
                nameof(ObtenerVehiculoPorId),
                new { id = nuevoVehiculo.Id },
                nuevoVehiculo);
        }

        // PUT: api/Vehiculos/1
        // Actualiza los datos de un vehículo.
        [HttpPut("{id}")]
        public IActionResult ActualizarVehiculo(
            int id,
            [FromBody] Vehiculo vehiculoActualizado)
        {
            var vehiculoExistente = AlmacenDatos.Vehiculos
                .FirstOrDefault(v => v.Id == id);

            if (vehiculoExistente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el vehículo que deseas actualizar."
                });
            }

            if (string.IsNullOrWhiteSpace(vehiculoActualizado.Matricula) ||
                string.IsNullOrWhiteSpace(vehiculoActualizado.Marca) ||
                string.IsNullOrWhiteSpace(vehiculoActualizado.Modelo) ||
                vehiculoActualizado.CapacidadCarga <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "Completa los campos obligatorios y utiliza una capacidad de carga mayor que cero."
                });
            }

            string matricula = vehiculoActualizado.Matricula.Trim();

            bool matriculaExiste = AlmacenDatos.Vehiculos.Any(
                v => v.Id != id &&
                     v.Matricula.Equals(
                         matricula,
                         StringComparison.OrdinalIgnoreCase));

            if (matriculaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Otro vehículo ya tiene esa matrícula."
                });
            }

            if (!EsEstadoValido(vehiculoActualizado.Estado))
            {
                return BadRequest(new
                {
                    mensaje = "El estado debe ser Disponible, En mantenimiento o Fuera de servicio."
                });
            }

            vehiculoExistente.Matricula = matricula.ToUpperInvariant();
            vehiculoExistente.Marca = vehiculoActualizado.Marca.Trim();
            vehiculoExistente.Modelo = vehiculoActualizado.Modelo.Trim();
            vehiculoExistente.CapacidadCarga = vehiculoActualizado.CapacidadCarga;
            vehiculoExistente.Estado = vehiculoActualizado.Estado.Trim();

            return NoContent();
        }

        // DELETE: api/Vehiculos/1
        // Elimina un vehículo por su ID.
        [HttpDelete("{id}")]
        public IActionResult EliminarVehiculo(int id)
        {
            var vehiculo = AlmacenDatos.Vehiculos
                .FirstOrDefault(v => v.Id == id);

            if (vehiculo == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el vehículo que deseas eliminar."
                });
            }

            AlmacenDatos.Vehiculos.Remove(vehiculo);

            return NoContent();
        }

        private static bool EsEstadoValido(string? estado)
        {
            return estado == "Disponible"
                || estado == "En mantenimiento"
                || estado == "Fuera de servicio";
        }
    }
}
