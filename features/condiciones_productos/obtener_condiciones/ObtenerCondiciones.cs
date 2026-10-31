using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using Dapper;

namespace backend_admon.features.condiciones_productos.obtener_condiciones
{
    public static class ObtenerCondiciones
    {
        public static void MapObtenerCondiciones(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/condiciones",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                        SELECT c.nombre AS Nombre,c.id_slug AS IdSlug, p.total as Total
                        FROM condiciones_producto c 
                        LEFT JOIN (
                          SELECT count(*) as total, p.id_condicion
                          FROM productos p
                          GROUP BY (p.id_condicion)
                        )  p ON c.id_condicion = p.id_condicion;
                        ";

                    try
                    {
                        var condiciones = await db.QueryAsync<CondicionDto>(querySql);

                        if (!condiciones.Any())
                        {
                            var resBad = Result<ObtenerCondicionesResponse>.Failure(
                                "No se encontraon condiciones",
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

                        var res = Result<ObtenerCondicionesResponse>.Success(
                            new ObtenerCondicionesResponse(
                                true,
                                "Condiciones cargadas exitosamente",
                                [.. condiciones]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerCondicionesResponse>.Failure(
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

public record CondicionDto
{
    public string? Nombre { get; init; }
    public string? IdSlug { get; init; }
    public int? Total { get; init; }
}

public record ObtenerCondicionesResponse(bool status, string message, List<CondicionDto> Filtros);
