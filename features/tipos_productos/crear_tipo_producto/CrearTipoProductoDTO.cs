using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.tipos_productos.crear_tipo_producto
{
    public record CrearTipoProductoDTO(int id, string name, DateTime fecha_creacion);
}
