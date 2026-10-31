using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.productos.entities
{
    public class Producto
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public required string NombreProducto { get; set; }
        public string? IdEspecial { get; set; }
        public required string Descripcion { get; set; }
        public double CostoUnitario { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public int IdTipo { get; set; }
        public int IdCompania { get; set; }
        public string? Rareza { get; set; }
        public required String Condicion { get; set; }
        public required string Expansion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
