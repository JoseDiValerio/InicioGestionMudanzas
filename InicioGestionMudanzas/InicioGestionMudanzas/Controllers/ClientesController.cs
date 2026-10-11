using Microsoft.AspNetCore.Mvc;
using InicioGestionMudanzas.Data;
using InicioGestionMudanzas.Models;

namespace InicioGestionMudanzas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        // GET: api/clientes
        // Devuelve todos los clientes registrados.
        [HttpGet]
        public IActionResult ObtenerClientes()
        {
            return Ok(AlmacenDatos.Clientes);
        }

        // GET: api/clientes/1
        // Busca un cliente utilizando su identificador.
        [HttpGet("{id}")]
        public IActionResult ObtenerClientePorId(int id)
        {
            var cliente = AlmacenDatos.Clientes
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el cliente."
                });
            }

            return Ok(cliente);
        }

        // POST: api/clientes
        // Registra un cliente nuevo.
        [HttpPost]
        public IActionResult CrearCliente([FromBody] Cliente nuevoCliente)
        {
            if (string.IsNullOrWhiteSpace(nuevoCliente.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoCliente.Cedula) ||
                string.IsNullOrWhiteSpace(nuevoCliente.Telefono) ||
                string.IsNullOrWhiteSpace(nuevoCliente.Direccion))
            {
                return BadRequest(new
                {
                    mensaje = "Todos los campos son obligatorios."
                });
            }

            bool cedulaExiste = AlmacenDatos.Clientes.Any(
                c => c.Cedula.Equals(
                    nuevoCliente.Cedula.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            if (cedulaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un cliente con esa cédula."
                });
            }

            int nuevoId = AlmacenDatos.Clientes.Count == 0
                ? 1
                : AlmacenDatos.Clientes.Max(c => c.Id) + 1;

            nuevoCliente.Id = nuevoId;
            nuevoCliente.Nombre = nuevoCliente.Nombre.Trim();
            nuevoCliente.Cedula = nuevoCliente.Cedula.Trim();
            nuevoCliente.Telefono = nuevoCliente.Telefono.Trim();
            nuevoCliente.Direccion = nuevoCliente.Direccion.Trim();

            AlmacenDatos.Clientes.Add(nuevoCliente);

            return CreatedAtAction(
                nameof(ObtenerClientePorId),
                new { id = nuevoCliente.Id },
                nuevoCliente);
        }

        // PUT: api/clientes/1
        // Actualiza los datos de un cliente existente.
        [HttpPut("{id}")]
        public IActionResult ActualizarCliente(
            int id,
            [FromBody] Cliente clienteActualizado)
        {
            var clienteExistente = AlmacenDatos.Clientes
                .FirstOrDefault(c => c.Id == id);

            if (clienteExistente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el cliente que deseas actualizar."
                });
            }

            if (string.IsNullOrWhiteSpace(clienteActualizado.Nombre) ||
                string.IsNullOrWhiteSpace(clienteActualizado.Cedula) ||
                string.IsNullOrWhiteSpace(clienteActualizado.Telefono) ||
                string.IsNullOrWhiteSpace(clienteActualizado.Direccion))
            {
                return BadRequest(new
                {
                    mensaje = "Todos los campos son obligatorios."
                });
            }

            bool cedulaExiste = AlmacenDatos.Clientes.Any(
                c => c.Id != id &&
                     c.Cedula.Equals(
                         clienteActualizado.Cedula.Trim(),
                         StringComparison.OrdinalIgnoreCase));

            if (cedulaExiste)
            {
                return Conflict(new
                {
                    mensaje = "Otro cliente ya tiene esa cédula."
                });
            }

            clienteExistente.Nombre = clienteActualizado.Nombre.Trim();
            clienteExistente.Cedula = clienteActualizado.Cedula.Trim();
            clienteExistente.Telefono = clienteActualizado.Telefono.Trim();
            clienteExistente.Direccion = clienteActualizado.Direccion.Trim();

            return NoContent();
        }

        // DELETE: api/clientes/1
        // Elimina un cliente por su identificador.
        [HttpDelete("{id}")]
        public IActionResult EliminarCliente(int id)
        {
            var cliente = AlmacenDatos.Clientes
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el cliente que deseas eliminar."
                });
            }

            AlmacenDatos.Clientes.Remove(cliente);

            return NoContent();
        }
    }
}
