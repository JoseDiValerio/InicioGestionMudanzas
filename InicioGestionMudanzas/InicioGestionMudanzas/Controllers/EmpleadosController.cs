using InicioGestionMudanzas.Data;
using InicioGestionMudanzas.Models;
using Microsoft.AspNetCore.Mvc;

namespace InicioGestionMudanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        // GET: api/Empleados
        // Devuelve todos los empleados.
        [HttpGet]
        public IActionResult ObtenerEmpleados()
        {
            return Ok(AlmacenDatos.Empleados);
        }

        // GET: api/Empleados/1
        // Busca un empleado por su ID.
        [HttpGet("{id}")]
        public IActionResult ObtenerEmpleadoPorId(int id)
        {
            var empleado = AlmacenDatos.Empleados
                .FirstOrDefault(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el empleado."
                });
            }

            return Ok(empleado);
        }

        // POST: api/Empleados
        // Registra un empleado nuevo.
        [HttpPost]
        public IActionResult CrearEmpleado([FromBody] Empleado nuevoEmpleado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEmpleado.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoEmpleado.Cedula) ||
                string.IsNullOrWhiteSpace(nuevoEmpleado.Telefono) ||
                string.IsNullOrWhiteSpace(nuevoEmpleado.Cargo))
            {
                return BadRequest(new
                {
                    mensaje = "Todos los campos son obligatorios."
                });
            }

            string cedula = nuevoEmpleado.Cedula.Trim();

            bool cedulaExiste = AlmacenDatos.Empleados.Any(
                e => e.Cedula.Equals(
                    cedula,
                    StringComparison.OrdinalIgnoreCase));

            if (cedulaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un empleado con esa cédula."
                });
            }

            int nuevoId = AlmacenDatos.Empleados.Count == 0
                ? 1
                : AlmacenDatos.Empleados.Max(e => e.Id) + 1;

            nuevoEmpleado.Id = nuevoId;
            nuevoEmpleado.Nombre = nuevoEmpleado.Nombre.Trim();
            nuevoEmpleado.Cedula = cedula;
            nuevoEmpleado.Telefono = nuevoEmpleado.Telefono.Trim();
            nuevoEmpleado.Cargo = nuevoEmpleado.Cargo.Trim();

            AlmacenDatos.Empleados.Add(nuevoEmpleado);

            return CreatedAtAction(
                nameof(ObtenerEmpleadoPorId),
                new { id = nuevoEmpleado.Id },
                nuevoEmpleado);
        }

        // PUT: api/Empleados/1
        // Actualiza un empleado existente.
        [HttpPut("{id}")]
        public IActionResult ActualizarEmpleado(
            int id,
            [FromBody] Empleado empleadoActualizado)
        {
            var empleadoExistente = AlmacenDatos.Empleados
                .FirstOrDefault(e => e.Id == id);

            if (empleadoExistente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el empleado que deseas actualizar."
                });
            }

            if (string.IsNullOrWhiteSpace(empleadoActualizado.Nombre) ||
                string.IsNullOrWhiteSpace(empleadoActualizado.Cedula) ||
                string.IsNullOrWhiteSpace(empleadoActualizado.Telefono) ||
                string.IsNullOrWhiteSpace(empleadoActualizado.Cargo))
            {
                return BadRequest(new
                {
                    mensaje = "Todos los campos son obligatorios."
                });
            }

            string cedula = empleadoActualizado.Cedula.Trim();

            bool cedulaExiste = AlmacenDatos.Empleados.Any(
                e => e.Id != id &&
                     e.Cedula.Equals(
                         cedula,
                         StringComparison.OrdinalIgnoreCase));

            if (cedulaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Otro empleado ya tiene esa cédula."
                });
            }

            empleadoExistente.Nombre = empleadoActualizado.Nombre.Trim();
            empleadoExistente.Cedula = cedula;
            empleadoExistente.Telefono = empleadoActualizado.Telefono.Trim();
            empleadoExistente.Cargo = empleadoActualizado.Cargo.Trim();

            return NoContent();
        }

        // DELETE: api/Empleados/1
        // Elimina un empleado por su ID.
        [HttpDelete("{id}")]
        public IActionResult EliminarEmpleado(int id)
        {
            var empleado = AlmacenDatos.Empleados
                .FirstOrDefault(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el empleado que deseas eliminar."
                });
            }

            AlmacenDatos.Empleados.Remove(empleado);

            return NoContent();
        }
    }
}
