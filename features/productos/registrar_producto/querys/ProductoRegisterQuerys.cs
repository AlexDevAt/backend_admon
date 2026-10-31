using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.productos.registrar_producto.querys
{
    public static class ProductoRegisterQuerys
    {
        public static readonly string CrearProductoQuery =
            @"
    INSERT INTO Productos (
        id_public_producto,
        nombre_producto,
        id_especial,
        descripcion,
        costo_unitario,
        stock,
        id_categoria_producto,
        id_tipo_producto,
        id_compania_producto,
        rareza,
        condicion,
        expansion
    )
    VALUES (
        @idPublic,
        @nombreProducto,
        @idEspecial,
        @descripcion,
        @costoUnitario,
        @stock,
        @idCategoria,
        @idTipo,
        @idCompania,
        @rareza,
        @condicion,
        @expansion
    )
    RETURNING
        id_producto AS Id,
        id_public_producto AS Uuid,
        nombre_producto AS NombreProducto,
        id_especial AS IdEspecial,
        descripcion AS Descripcion,
        costo_unitario AS CostoUnitario,
        stock AS Stock,
        id_categoria_producto AS IdCategoria,
        id_tipo_producto AS IdTipo,
        id_compania_producto AS IdCompania,
        rareza AS Rareza,
        condicion AS Condicion,
        expansion AS Expansion;
";

        public static readonly string ExisteProuctoQuery =
            @"
        SELECT EXISTS (
        SELECT 1 
        FROM Productos 
        WHERE nombre_producto = @nombre 
          AND id_compania_producto = @idCompania
    );";
    }
}
