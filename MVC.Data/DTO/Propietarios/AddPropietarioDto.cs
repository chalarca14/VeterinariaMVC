namespace MVC.Data.DTO.Propietarios
{
    public class AddPropietarioDto
    {
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public string? Direccion { get; set; }
    }
}