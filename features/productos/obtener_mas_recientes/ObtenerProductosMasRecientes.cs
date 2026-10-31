using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using backend_admon.features.productos.dtos;
using backend_admon.features.productos.obtener_productos;
using backend_admon.features.productos.obtener_productos.dtos;
using Dapper;

namespace backend_admon.features.productos.obtener_mas_recientes
{
    public static class ObtenerProductosMasRecientes
    {
        public static void MapObtenerMasRecientess(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/productos/recientes",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"SELECT 
                        p.id_public_producto AS id_public,
                        p.id_especial AS idEspecial,
                        p.nombre_producto AS nombre,
                        p.costo_unitario * 1.16 AS precio_iva,
                        t.id_tipo AS idTipo,
                        t.nombre_tipo AS nombre
                        FROM Productos p
                        INNER JOIN Tipos_productos t 
                        ON t.id_tipo = p.id_tipo_producto

                        ORDER BY p.fecha_creacion DESC
                        LIMIT 5;";
                    try
                    {
                        var productos = await db.QueryAsync<
                            ProductoCardDTO,
                            TipoProductoDto,
                            ProductoCardDTO
                        >(
                            querySql,
                            (producto, tipo) =>
                            {
                                return producto with { TipoProducto = tipo };
                            },
                            splitOn: "idTipo"
                        );
                        if (!productos.Any())
                        {
                            var resBad = Result<ObtenerProductosResponse<ProductoCardDTO>>.Failure(
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

                        var res = Result<ObtenerProductosResponse<ProductoCardDTO>>.Success(
                            new ObtenerProductosResponse<ProductoCardDTO>(
                                true,
                                productos.ToList().Count,
                                [.. productos]
                            ),
                            200
                        );

                        return Results.Ok(res.Value);
                    }
                    catch (Exception ex)
                    {
                        var res = Result<ObtenerProductosResponse<ProductoCardDTO>>.Failure(
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
