using System;
using System.Collections.Generic;

namespace MVC.Data.Models;

public partial class Mascota
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdPropietario { get; set; }

    public int IdRaza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public decimal? Peso { get; set; }

    public string? RutaFoto { get; set; }

    public virtual Propietario IdPropietarioNavigation { get; set; } = null!;

    public virtual Raza IdRazaNavigation { get; set; } = null!;
}
