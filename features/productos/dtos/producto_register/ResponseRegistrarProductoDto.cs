using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.productos.dtos.producto_register
{
    public record ResponseRegistrarProductoDto(
        Guid id_public,
        string? idEspecial,
        string nombre,
        string descripcion,
        double precio_unitario,
        double precio_iva,
        string categoria,
        string tipo,
        string compania,
        string rareza,
        string condicion,
        string expansion,
        DateTime fecha_creacion
    );
}
