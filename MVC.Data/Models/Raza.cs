using System;
using System.Collections.Generic;

namespace MVC.Data.Models;

public partial class Raza
{
    public int Id { get; set; }

    public int IdEspecie { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual Especy IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
