using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.features.productos.obtener_productos.dtos;

namespace backend_admon.features.productos.dtos
{
    public record ProductoCardDTO
    {
        public Guid id_public { get; init; }
        public string? idEspecial { get; init; }
        public string? nombre { get; init; }
        public string? descripcion { get; init; }
        public decimal precio_unitario { get; init; }
        public decimal precio_iva { get; init; }
        public TipoProductoDto? TipoProducto { get; init; }
    }
}
