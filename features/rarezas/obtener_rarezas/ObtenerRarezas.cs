using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using Dapper;

namespace backend_admon.features.rarezas.obtener_rarezas
{
    public static class ObtenerRarezas
    {
        public static void MapObtenerRarezas(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/rarezas",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                        SELECT r.nombre AS Nombre,r.id_slug AS IdSlug,p.total as Total
                        FROM rarezas r 
                        LEFT JOIN (
                        SELECT count(*) as total, p.id_rareza
                        FROM productos p
                        GROUP BY (p.id_rareza)
                        )  p ON r.id_rareza = p.id_rareza;
                        ";

                    try
                    {
                        var rarezas = await db.QueryAsync<RarezaDto>(querySql);

                        if (!rarezas.Any())
                        {
                            var resBad = Result<ObtenerRarezasResponse>.Failure(
                                "No se encontraon categorias",
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

                        var res = Result<ObtenerRarezasResponse>.Success(
                            new ObtenerRarezasResponse(
                                true,
                                "expansiones cargadas exitosamente",
                                [.. rarezas]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerRarezasResponse>.Failure(
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

public record RarezaDto
{
    public string Nombre { get; init; }
    public string IdSlug { get; init; }
    public int? Total { get; init; }
}

public record ObtenerRarezasResponse(bool status, string message, List<RarezaDto> Filtros);
