using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using Dapper;

namespace backend_admon.features.tipos_productos.obtener_tipos_destacados
{
    public static class ObtenerTiposDestacados
    {
        public static void MapObtenerTiposDestacados(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/tipos/destacados",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                        SELECT count(*) as Productos ,t.id_tipo AS Id, nombre_tipo AS Nombre 
                        FROM Tipos_productos t
                        LEFT JOIN Productos p ON p.id_tipo_producto = t.id_tipo 
                        WHERE t.es_destacado = true
                        GROUP BY (id_tipo,nombre_tipo);";

                    try
                    {
                        var categorias = await db.QueryAsync<TipoDto>(querySql);

                        if (!categorias.Any())
                        {
                            var resBad = Result<ObtenerTiposResponse>.Failure(
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

                        var res = Result<ObtenerTiposResponse>.Success(
                            new ObtenerTiposResponse(
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
