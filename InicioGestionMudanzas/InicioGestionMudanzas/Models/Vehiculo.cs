namespace InicioGestionMudanzas.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public double CapacidadCarga { get; set; }
        public string Estado { get; set; } = "Disponible";

    }
}
