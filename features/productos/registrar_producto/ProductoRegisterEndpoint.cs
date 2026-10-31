using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace backend_admon.features.productos.registrar_producto
{
    public static class ProductoRegisterEndpoint
    {
        public static void MapCrearProducto(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/productos",
                async (ProductoRegisterCommand command, IMediator mediator) =>
                {
                    var response = await mediator.Send(command);
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
