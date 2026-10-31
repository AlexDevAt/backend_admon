using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.helpers;
using backend_admon.core.patterns;
using backend_admon.features.productos.entities;
using backend_admon.features.productos.obtener_productos.dtos;
using Dapper;
using MediatR;
using Npgsql.Internal;

namespace backend_admon.features.productos.obtener_productos
{
    public class ObtenerProductosHandle
        : IRequestHandler<ObtenerProductosQuery, Result<ObtenerProductosResponse<ProductoDto>>>
    {
        private readonly IDbConnection _db;

        public ObtenerProductosHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<ObtenerProductosResponse<ProductoDto>>> Handle(
            ObtenerProductosQuery request,
            CancellationToken cancellationToken
        )
        {
            DynamicParameters parameters = new();

            string queryObtenerTodos =
                @"SELECT 
                p.id_public_producto AS id_public,
                p.id_especial AS idEspecial,
                p.nombre_producto AS nombre,
                p.descripcion AS descripcion,
                p.costo_unitario AS precio_unitario,
                p.costo_unitario * 1.16 AS precio_iva,
                p.stock,
                p.fecha_creacion AS fecha_creacion,

                c.id_slug AS idSlugCategoria,
                c.nombre_categoria AS nombre,

                t.id_tipo AS idTipo,
                t.nombre_tipo AS nombre,

                co.id_compania AS idCompania,
                co.nombre_compania AS nombre,

                r.id_slug AS idSlugRareza,
                r.nombre AS nombre,

                con.id_slug AS idSlugCondicion,
                con.nombre AS nombre,
                
                ex.id_slug AS idSlugExpansion,
                ex.nombre AS nombre 

                FROM productos p

                INNER JOIN categorias_productos c
                    ON c.id_categoria = p.id_categoria_producto

                INNER JOIN tipos_productos t
                    ON t.id_tipo = p.id_tipo_producto

                INNER JOIN companias co
                    ON co.id_compania = p.id_compania_producto

                LEFT JOIN rarezas r 
                ON r.id_rareza = p.id_rareza

                LEFT JOIN condiciones_producto con
                ON con.id_condicion = p.id_condicion

                LEFT JOIN expansiones ex 
                ON ex.id_expansion = p.id_expansion

                WHERE 1=1

";
            ProductoFiltros filtros = request.Filtros;

            if (filtros.categoria != null)
            {
                queryObtenerTodos += @" AND c.id_slug = @idCategoriaSlug";

                parameters.Add("idCategoriaSlug", filtros.categoria);
            }

            if (filtros.tipo != null)
            {
                queryObtenerTodos += @" AND t.id_tipo = @idTipo";

                parameters.Add("idTipo", filtros.tipo);
            }

            if (filtros.compania != null)
            {
                queryObtenerTodos += @" AND co_id_compania = @idComp";

                parameters.Add("idComp", filtros.compania);
            }

            if (filtros.precioMin != null)
            {
                queryObtenerTodos += @" AND p.costo_unitario * 1.16 >= @precioMin";

                parameters.Add("precioMin", filtros.precioMin);
            }

            if (filtros.precioMax != null)
            {
                queryObtenerTodos += @" AND p.costo_unitario * 1.16 <= @precioMmax";

                parameters.Add("precioMmax", filtros.precioMax);
            }

            if (filtros.disponible != null && filtros.disponible == true)
            {
                queryObtenerTodos += @" AND p.stock > 0 ";
            }
            if (filtros.disponible != null && filtros.disponible == false)
            {
                queryObtenerTodos += @" AND p.stock = 0 ";
            }

            if (filtros.expansiones != null)
            {
                queryObtenerTodos += @" AND ex.id_slug = @expansionIDSlug ";
                parameters.Add("expansionIDSlug", filtros.expansiones);
            }

            if (filtros.rarezas != null)
            {
                queryObtenerTodos += @" AND r.id_slug = @rarezaidSlug";
                parameters.Add("rarezaidSlug", filtros.rarezas);
            }

            if (filtros.condiciones != null)
            {
                queryObtenerTodos += @" AND con.id_slug = @condicionSlug";
                parameters.Add("condicionSlug", filtros.condiciones);
            }

            if (filtros.orden != null)
            { //precio-asc
                queryObtenerTodos += filtros.orden.ToLower() switch
                {
                    "costo_unitario_desc" => " ORDER BY costo_unitario DESC",
                    "costo_unitario_asc" => " ORDER BY costo_unitario ASC",
                    "stock_asc" => " ORDER BY stock ASC",
                    "stock_desc" => " ORDER BY stock DESC",
                    _ => " ORDER BY fecha_creacion desc",
                };
            }

            if (filtros.limite != null)
            {
                Console.WriteLine(filtros.limite);
                queryObtenerTodos += @" LIMIT @limite ";
                parameters.Add("limite", filtros.limite);
            }

            var productos = await _db.QueryAsync<
                ProductoDto,
                CategoriaProductoDto,
                TipoProductoDto,
                CompaniaProductoDto,
                RarezaProductoDto,
                CondicionProductDto,
                ExpansionProductDto,
                ProductoDto
            >(
                queryObtenerTodos,
                (producto, categoria, tipo, compania, rareza, condiciones, expansiones) =>
                {
                    return producto with
                    {
                        categoria = categoria,
                        tipo = tipo,
                        compania = compania,
                        rareza = rareza,
                        condicion = condiciones,
                        expansion = expansiones,
                    };
                },
                param: parameters,
                splitOn: "idSlugCategoria,idTipo,idCompania,idSlugRareza,idSlugCondicion,idSlugExpansion"
            );

            if (!productos.Any())
            {
                return Result<ObtenerProductosResponse<ProductoDto>>.Failure(
                    "No hay productos",
                    200
                );
            }

            int totalProductos = productos.ToList().Count;

            return Result<ObtenerProductosResponse<ProductoDto>>.Success(
                new ObtenerProductosResponse<ProductoDto>(true, totalProductos, [.. productos]),
                200
            );
        }
    }
}
