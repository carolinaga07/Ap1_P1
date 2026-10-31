using System;
using System.Collections.Generic;

namespace GestionLibros.Data.Models;

public partial class EntradasDetalle
{
    public int DetalleId { get; set; }

    public int EntradaId { get; set; }

    public int LibroId { get; set; }

    public int Cantidad { get; set; }

    public virtual Entrada Entrada { get; set; } = null!;

    public virtual Libro Libro { get; set; } = null!;
}
