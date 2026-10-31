using System.Data;
using backend_admon.core.patterns;
using backend_admon.features.tipos_productos.entities;
using Dapper;
using MediatR;
using Npgsql;

namespace backend_admon.features.tipos_productos.crear_tipo_producto
{
    public class CrearTipoProductoHandle
        : IRequestHandler<CrearTipoProductoCommand, Result<CrearTipoProductoResponse>>
    {
        private readonly IDbConnection _db;

        public CrearTipoProductoHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<CrearTipoProductoResponse>> Handle(
            CrearTipoProductoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(request.name.Trim()))
            {
                return Result<CrearTipoProductoResponse>.Failure("", 200);
            }

            string queryExist =
                @"SELECT EXISTS (SELECT 1 FROM Tipos_productos WHERE nombre_tipo = @nombreTipo)";

            bool existeCat = await _db.ExecuteScalarAsync<bool>(
                queryExist,
                new { nombreTipo = request.name }
            );

            if (existeCat)
            {
                return Result<CrearTipoProductoResponse>.Failure("La categoria ya existe", 409);
            }

            string queryCrear =
                @"INSERT INTO Tipos_productos (nombre_tipo)
            VALUES (@nombreTipo) 
            RETURNING id_tipo AS Id,nombre_tipo AS Nombre,fecha_creacion AS FechaCreacion;";

            try
            {
                TipoProducto tipoProducto = await _db.QueryFirstAsync<TipoProducto>(
                    queryCrear,
                    new { nombreTipo = request.name }
                );
                return Result<CrearTipoProductoResponse>.Success(
                    new CrearTipoProductoResponse(
                        true,
                        "Tipo de producto creado",
                        new CrearTipoProductoDTO(
                            tipoProducto.Id,
                            tipoProducto.Nombre,
                            tipoProducto.FechaCreacion
                        )
                    ),
                    201
                );
            }
            catch (PostgresException ex)
            {
                return Result<CrearTipoProductoResponse>.Failure(ex.MessageText, 409);
            }
            catch (Exception ex)
            {
                return Result<CrearTipoProductoResponse>.Failure(
                    $"error en el sistema {ex.Message}",
                    505
                );
            }
        }
    }
}
