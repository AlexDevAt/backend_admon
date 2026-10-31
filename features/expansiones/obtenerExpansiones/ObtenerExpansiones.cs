using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using Dapper;

namespace backend_admon.features.expansiones.obtenerExpansiones
{
    public static class ObtenerExpansiones
    {
        public static void MapObtenerExpansiones(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/expansiones",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                        SELECT e.nombre AS Nombre,e.id_slug AS IdSlug,p.total as Total
                        FROM expansiones e 
                        LEFT JOIN (
                          SELECT count(*) as total, p.id_expansion
                          FROM productos p
                          GROUP BY (p.id_expansion)
                        )  p ON e.id_expansion = p.id_expansion;
                        ";

                    try
                    {
                        var expansiones = await db.QueryAsync<ExpansionDto>(querySql);

                        if (!expansiones.Any())
                        {
                            var resBad = Result<ObtenerExpansionesResponse>.Failure(
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

                        var res = Result<ObtenerExpansionesResponse>.Success(
                            new ObtenerExpansionesResponse(
                                true,
                                "expansiones cargadas exitosamente",
                                [.. expansiones]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerExpansionesResponse>.Failure(
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

public record ExpansionDto
{
    public string Nombre { get; init; }
    public string IdSlug { get; init; }
    public int? Total { get; init; }
}

public record ObtenerExpansionesResponse(bool status, string message, List<ExpansionDto> Filtros);
