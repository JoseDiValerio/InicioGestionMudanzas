using InicioGestionMudanzas.Data;
using InicioGestionMudanzas.Models;
using Microsoft.AspNetCore.Mvc;

namespace InicioGestionMudanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MudanzasController : ControllerBase
    {
        // GET: api/Mudanzas
        // Devuelve todas las mudanzas registradas.
        [HttpGet]
        public IActionResult ObtenerMudanzas()
        {
            return Ok(AlmacenDatos.Mudanzas);
        }

        // GET: api/Mudanzas/1
        // Buscar mudanza por identificador.
        [HttpGet("{id}")]
        public IActionResult ObtenerMudanzaPorId(int id)
        {
            var mudanza = AlmacenDatos.Mudanzas
                .FirstOrDefault(m => m.Id == id);

            if (mudanza == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la mudanza."
                });
            }

            return Ok(mudanza);
        }

        // POST: api/Mudanzas
        // Registra una mudanza nueva.
        [HttpPost]
        public IActionResult CrearMudanza([FromBody] Mudanza nuevaMudanza)
        {
            string? error = ValidarMudanza(nuevaMudanza);

            if (error != null)
            {
                return BadRequest(new { mensaje = error });
            }

            int nuevoId = AlmacenDatos.Mudanzas.Count == 0
                ? 1
                : AlmacenDatos.Mudanzas.Max(m => m.Id) + 1;

            nuevaMudanza.Id = nuevoId;
            nuevaMudanza.DireccionOrigen = nuevaMudanza.DireccionOrigen.Trim();
            nuevaMudanza.DireccionDestino = nuevaMudanza.DireccionDestino.Trim();
            nuevaMudanza.Estado = NormalizarEstado(nuevaMudanza.Estado);

            AlmacenDatos.Mudanzas.Add(nuevaMudanza);

            return CreatedAtAction(
                nameof(ObtenerMudanzaPorId),
                new { id = nuevaMudanza.Id },
                nuevaMudanza);
        }

        // PUT: api/Mudanzas/1
        // Actualiza los datos de una mudanza.
        [HttpPut("{id}")]
        public IActionResult ActualizarMudanza(
            int id,
            [FromBody] Mudanza mudanzaActualizada)
        {
            var mudanzaExistente = AlmacenDatos.Mudanzas
                .FirstOrDefault(m => m.Id == id);

            if (mudanzaExistente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la mudanza que deseas actualizar."
                });
            }

            string? error = ValidarMudanza(mudanzaActualizada);

            if (error != null)
            {
                return BadRequest(new { mensaje = error });
            }

            mudanzaExistente.ClienteId = mudanzaActualizada.ClienteId;
            mudanzaExistente.VehiculoId = mudanzaActualizada.VehiculoId;
            mudanzaExistente.EmpleadoId = mudanzaActualizada.EmpleadoId;
            mudanzaExistente.Fecha = mudanzaActualizada.Fecha;
            mudanzaExistente.DireccionOrigen = mudanzaActualizada.DireccionOrigen.Trim();
            mudanzaExistente.DireccionDestino = mudanzaActualizada.DireccionDestino.Trim();
            mudanzaExistente.Costo = mudanzaActualizada.Costo;
            mudanzaExistente.Estado = NormalizarEstado(mudanzaActualizada.Estado);

            return NoContent();
        }

        // DELETE: api/Mudanzas/1
        // Eliminar mudanza por identificador.
        [HttpDelete("{id}")]
        public IActionResult EliminarMudanza(int id)
        {
            var mudanza = AlmacenDatos.Mudanzas
                .FirstOrDefault(m => m.Id == id);

            if (mudanza == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la mudanza que deseas eliminar."
                });
            }

            AlmacenDatos.Mudanzas.Remove(mudanza);

            return NoContent();
        }

        // Comprueba que los datos de la mudanza sean correctos.
        private static string? ValidarMudanza(Mudanza mudanza)
        {
            if (mudanza == null)
            {
                return "Debes enviar los datos de la mudanza.";
            }

            if (!AlmacenDatos.Clientes.Any(c => c.Id == mudanza.ClienteId))
            {
                return "El cliente indicado no existe.";
            }

            var vehiculo = AlmacenDatos.Vehiculos
                .FirstOrDefault(v => v.Id == mudanza.VehiculoId);

            if (vehiculo == null)
            {
                return "El vehículo indicado no existe.";
            }

            if (vehiculo.Estado.Equals(
                "Fuera de servicio",
                StringComparison.OrdinalIgnoreCase))
            {
                return "No se puede asignar un vehículo que está fuera de servicio.";
            }

            if (!AlmacenDatos.Empleados.Any(e => e.Id == mudanza.EmpleadoId))
            {
                return "El empleado indicado no existe.";
            }

            if (mudanza.Fecha == DateTime.MinValue)
            {
                return "Debes indicar una fecha válida para la mudanza.";
            }

            if (string.IsNullOrWhiteSpace(mudanza.DireccionOrigen))
            {
                return "La dirección de origen es obligatoria.";
            }

            if (string.IsNullOrWhiteSpace(mudanza.DireccionDestino))
            {
                return "La dirección de destino es obligatoria.";
            }

            if (mudanza.Costo < 0)
            {
                return "El costo no puede ser menor que cero.";
            }

            if (!EsEstadoValido(mudanza.Estado))
            {
                return "El estado debe ser Pendiente, En proceso o Completada.";
            }

            return null;
        }

        // Comprueba que el estado recibido esté permitido.
        private static bool EsEstadoValido(string? estado)
        {
            return !string.IsNullOrWhiteSpace(estado) &&
                (estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) ||
                 estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase) ||
                 estado.Equals("Completada", StringComparison.OrdinalIgnoreCase));
        }

        // Guarda el estado con el mismo formato en todos los registros.
        private static string NormalizarEstado(string? estado)
        {
            if (estado!.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                return "Pendiente";

            if (estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase))
                return "En proceso";

            return "Completada";
        }
    }
}
