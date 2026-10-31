using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.features.productos.obtener_productos.dtos;
using MediatR;

namespace backend_admon.features.productos.obtener_productos
{
    public static class ObtenerProductosEndpoint
    {
        public static void MapObtenerProductos(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/productos",
                async ([AsParameters] ProductoFiltros filtros, ISender sender) =>
                {
                    var query = new ObtenerProductosQuery(filtros);

                    var response = await sender.Send(query);

                    if (!response.IsSuccess)
                    {
                        return Results.Json(
                            data: new
                            {
                                succes = response.IsSuccess,
                                error_message = response.ErrorMessage,
                            },
                            statusCode: response.statusCode
                        );
                    }
                    return Results.Ok(response.Value);
                }
            );
        }
    }
}
