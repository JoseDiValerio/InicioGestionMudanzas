namespace InicioGestionMudanzas.Models
{
    public class Mudanza
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime Fecha {  get; set; }
        public string DireccionOrigen {  get; set; } = string.Empty;
        public string DireccionDestino { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
