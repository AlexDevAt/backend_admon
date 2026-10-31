using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace backend_admon.features.categorias.crear_categoria
{
    public static class CrearCategoriaEndpoint
    {
        public static void MapCrearCategoria(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/categorias",
                async (CrearCategoriaCommand command, IMediator mediator) =>
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
