using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using Dapper;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace backend_admon.features.categorias.obtener_categorias
{
    public static class ObtenerCategorias
    {
        public static void MapObtenerCategorias(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/categorias",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                        SELECT nombre_categoria AS Nombre,id_slug AS idSlug FROM Categorias_productos c;";

                    try
                    {
                        var categorias = await db.QueryAsync<CategoriaDto>(querySql);

                        if (!categorias.Any())
                        {
                            var resBad = Result<ObtenerCategoriasResponse>.Failure(
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

                        var res = Result<ObtenerCategoriasResponse>.Success(
                            new ObtenerCategoriasResponse(
                                true,
                                "Categorias cargadas exitosamente",
                                [.. categorias]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerCategoriasResponse>.Failure(
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

public record CategoriaDto
{
    public string Nombre { get; init; }
    public string idSlug { get; init; }
}

public record ObtenerCategoriasResponse(bool status, string message, List<CategoriaDto> Filtros);
