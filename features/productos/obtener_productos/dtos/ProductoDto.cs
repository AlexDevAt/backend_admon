using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.productos.obtener_productos.dtos
{
    public record ProductoDto
    {
        public Guid id_public { get; init; }
        public string? idEspecial { get; init; }
        public string nombre { get; init; } = null!;
        public string descripcion { get; init; } = null!;

        public decimal precio_unitario { get; init; }
        public decimal precio_iva { get; init; }
        public decimal stock { get; init; }
        public DateTime fecha_creacion { get; init; }

        public CategoriaProductoDto? categoria { get; init; }
        public TipoProductoDto? tipo { get; init; }
        public CompaniaProductoDto? compania { get; init; }

        public RarezaProductoDto? rareza { get; init; }
        public CondicionProductDto? condicion { get; init; }

        public ExpansionProductDto expansion { get; init; }
    }

    public record ExpansionProductDto
    {
        public string IdSlugExpansion { get; init; }
        public string nombre { get; init; }
    }

    public record RarezaProductoDto
    {
        public string IdSlugRareza { get; init; }
        public string nombre { get; init; }
    }

    public record CondicionProductDto
    {
        public string IdSlugCondicion { get; init; }
        public string nombre { get; init; }
    }
}
