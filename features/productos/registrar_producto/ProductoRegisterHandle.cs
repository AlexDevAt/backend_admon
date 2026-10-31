using System.Data;
using backend_admon.core.helpers;
using backend_admon.core.patterns;
using backend_admon.features.productos.dtos.producto_register;
using backend_admon.features.productos.entities;
using backend_admon.features.productos.registrar_producto.querys;
using Dapper;
using MediatR;
using Npgsql;

namespace backend_admon.features.productos.registrar_producto
{
    public class ProductoRegisterHandle
        : IRequestHandler<ProductoRegisterCommand, Result<ProductoRegisterResponse>>
    {
        private readonly IDbConnection _db;

        public ProductoRegisterHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<ProductoRegisterResponse>> Handle(
            ProductoRegisterCommand request,
            CancellationToken cancellationToken
        )
        {
            string sql = ProductoRegisterQuerys.ExisteProuctoQuery;

            bool existProducto = await _db.ExecuteScalarAsync<bool>(
                sql,
                new { nombre = request.Name, idCompania = request.Id_compania }
            );

            if (existProducto)
            {
                return Result<ProductoRegisterResponse>.Failure("El producto ya existe", 409);
            }

            string sqlCreate = ProductoRegisterQuerys.CrearProductoQuery;

            try
            {
                Producto producto = await _db.QueryFirstAsync<Producto>(
                    sqlCreate,
                    new
                    {
                        idPublic = Guid.CreateVersion7(),
                        nombreProducto = request.Name,
                        idEspecial = request.Id_especial ?? null,
                        descripcion = request.Descripcion,
                        costoUnitario = request.Costo_unitario,
                        stock = request.stock,
                        idCategoria = request.Id_categoria,
                        idTipo = request.Id_tipo,
                        idCompania = request.Id_compania,
                        rareza = request.Rareza,
                        condicion = request.Condicion,
                        expansion = request.Expansion,
                    }
                );

                //SI FALLA LOS JOINS HAY QUE HACER UN ROLLBACK NO SE TE OLVIDE CABRON TE AMO

                Console.WriteLine(producto.IdCategoria);

                string queryObtenerNombresDeFk =
                    @"SELECT 
                    c.nombre_categoria AS Categoria,
                    t.nombre_tipo AS TipoProducto,
                    co.nombre_compania AS Compania
                FROM Productos p
                INNER JOIN categorias_productos c 
                    ON c.id_categoria = p.id_categoria_producto
                INNER JOIN Tipos_productos t 
                    ON t.id_tipo = p.id_tipo_producto
                INNER JOIN Companias co 
                    ON co.id_compania = p.id_compania_producto;";

                NombresFkProductoDto nombresFkProductoDto =
                    await _db.QueryFirstAsync<NombresFkProductoDto>(
                        queryObtenerNombresDeFk,
                        new { productoId = producto.Id }
                    );

                ResponseRegistrarProductoDto productoDto = new ResponseRegistrarProductoDto(
                    id_public: producto.Uuid,
                    idEspecial: producto.IdEspecial,
                    nombre: producto.NombreProducto,
                    descripcion: producto.Descripcion,
                    precio_unitario: producto.CostoUnitario,
                    precio_iva: producto.CostoUnitario * Impuestos.IVA,
                    categoria: nombresFkProductoDto.Categoria,
                    tipo: nombresFkProductoDto.TipoProducto,
                    compania: nombresFkProductoDto.Compania,
                    rareza: producto.Rareza ?? null,
                    condicion: producto.Condicion,
                    expansion: producto.Expansion ?? null,
                    fecha_creacion: producto.FechaCreacion
                );

                return Result<ProductoRegisterResponse>.Success(
                    new ProductoRegisterResponse(
                        Status: true,
                        Message: "Producto creado con exito",
                        Producto: productoDto
                    ),
                    201
                );
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return Result<ProductoRegisterResponse>.Failure("El producto ya existe", 409);
            }
        }
    }
}
