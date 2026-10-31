using System.Data;
using backend_admon.core.patterns;
using backend_admon.features.productos.dtos;
using backend_admon.features.productos.obtener_productos;
using backend_admon.features.productos.obtener_productos.dtos;
using Dapper;

namespace backend_admon.features.productos.obtener_mas_vendidos
{
    public static class ObtenerMasVendidos
    {
        public static void MapObtenerMasVendidos(this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/productos/mas-vendidos",
                async (IDbConnection db) =>
                {
                    string querySql =
                        @"
                    SELECT
                    p.id_public_producto AS id_public,
                    p.id_especial AS idEspecial,
                    p.nombre_producto AS nombre,
                    p.costo_unitario * 1.16 AS precio_iva,
                    d.id_producto,d.cantidad,
                    t.id_tipo AS idTipo,
                    t.nombre_tipo AS nombre
                    
                    FROM productos p
                    INNER JOIN (
                      SELECT 
                      d.id_producto,SUM(d.cantidad) as cantidad
                      FROM detallepedido d
                      GROUP BY (d.id_producto)
                    ) AS d ON d.id_producto = p.id_producto
                    INNER JOIN Tipos_productos t 
                    ON t.id_tipo = p.id_tipo_producto
                    ORDER BY d.cantidad DESC LIMIT 5;";
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
