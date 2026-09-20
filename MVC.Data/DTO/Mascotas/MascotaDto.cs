namespace MVC.Data.DTO.Mascotas
{
    public class MascotaDto
    {
        public int IdMascota { get; set; }
        public string Nombre { get; set; } = null!;
        public DateOnly? FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public string? RutaFoto { get; set; }

        public int IdPropietario { get; set; }
        public string NombrePropietario { get; set; } = null!;

        public int IdEspecie { get; set; }
        public string NombreEspecie { get; set; } = null!;

        public int IdRaza { get; set; }
        public string NombreRaza { get; set; } = null!;
    }
}