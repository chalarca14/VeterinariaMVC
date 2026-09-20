using Microsoft.AspNetCore.Http;

namespace MVC.Data.DTO.Mascotas
{
    public class AddMascotaDto
    {
        public string Nombre { get; set; } = null!;
        public DateOnly? FechaNacimiento { get; set; }
        public decimal? Peso { get; set; }
        public int IdPropietario { get; set; }
        public int IdEspecie { get; set; }
        public int IdRaza { get; set; }
        public IFormFile? FotoFile { get; set; } // Representa el archivo de imagen adjunto
    }
}