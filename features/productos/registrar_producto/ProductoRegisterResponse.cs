using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.features.productos.dtos.producto_register;

namespace backend_admon.features.productos.registrar_producto
{
    public record ProductoRegisterResponse
    (
        bool Status,
        string Message,
        ResponseRegistrarProductoDto Producto
    );
}