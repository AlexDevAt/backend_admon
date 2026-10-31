using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace backend_admon.features.companias.crear_compania
{
    public static class CrearCompaniaEndpoint
    {
        public static void MapCrearCompania(this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/companias",
                async (CrearCompaniaCommand command, IMediator mediator) =>
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
