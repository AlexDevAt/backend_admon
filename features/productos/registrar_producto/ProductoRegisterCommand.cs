using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.productos.registrar_producto
{
    public record ProductoRegisterCommand
    (
        string Name,
        string Descripcion,
        string Id_especial,
        double Costo_unitario,
        int stock,
        int Id_categoria,
        int Id_tipo,
        int Id_compania,
        string Rareza,
        string Condicion,
        string Expansion
    ):IRequest<Result<ProductoRegisterResponse>>;
}