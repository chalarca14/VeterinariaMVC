using System;
using System.Collections.Generic;

namespace MVC.Data.Models;

public partial class Especy
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Raza> Razas { get; set; } = new List<Raza>();
}
