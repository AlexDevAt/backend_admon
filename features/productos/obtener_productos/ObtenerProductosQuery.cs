using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using backend_admon.features.productos.obtener_productos.dtos;
using MediatR;

namespace backend_admon.features.productos.obtener_productos
{
    public record ObtenerProductosQuery(ProductoFiltros Filtros)
        : IRequest<Result<ObtenerProductosResponse<ProductoDto>>>;
}
