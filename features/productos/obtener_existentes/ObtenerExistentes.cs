using System.Data;
using backend_admon.core.patterns;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace backend_admon.features.productos.obtener_existentes
{
    public static class ObtenerExistentes
    {
        public static void MapObtenerProductosExistencia(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/productos/disponibilidad",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"SELECT 
                            (SELECT COUNT(*) AS existentes FROM productos WHERE productos.stock > 1) ,
                            COUNT(*) - (SELECT COUNT(*) FROM productos WHERE productos.stock > 1) AS Agotados
                        FROM productos p;";
                    try
                    {
                        var productos = await db.QueryFirstOrDefaultAsync<disponibilidad>(querySql);

                        if (productos == null)
                        {
                            var resBad = Result<ObtenerFiltrosResponse>.Failure(
                                "No hay productos disponibles",
                                404
                            );

                            return Results.Json(
                                data: new
                                {
                                    succes = resBad.IsSuccess,
                                    error_message = resBad.ErrorMessage,
                                },
                                statusCode: resBad.statusCode
                            );
                        }

                        FiltroDisponibilidad[] response =
                        [
                            new FiltroDisponibilidad
                            {
                                Nombre = "Disponibles",
                                IdSlug = "true",
                                Total = productos.Existentes,
                            },
                            new FiltroDisponibilidad
                            {
                                Nombre = "Agotados",
                                IdSlug = "false",
                                Total = productos.Agotados,
                            },
                        ];

                        var res = Result<ObtenerFiltrosResponse>.Success(
                            new ObtenerFiltrosResponse(
                                true,
                                "Productos en existencia cargados exitosamente",
                                [.. response]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerFiltrosResponse>.Failure(
                            ex.Message.ToString(),
                            500
                        );

                        return Results.Json(
                            data: new { succes = res.IsSuccess, error_message = res.ErrorMessage },
                            statusCode: res.statusCode
                        );
                    }
                }
            );
        }
    }
}

public record disponibilidad
{
    public int? Existentes { get; init; }
    public int? Agotados { get; init; }
}

public record FiltroDisponibilidad
{
    public string? Nombre { get; init; }
    public string? IdSlug { get; init; }
    public int? Total { get; init; }
}

public record ObtenerFiltrosResponse(
    bool status,
    string message,
    List<FiltroDisponibilidad> Filtros
);
