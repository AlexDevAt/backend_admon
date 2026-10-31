using System.Data;
using backend_admon.core.patterns;
using backend_admon.features.categorias.entities;
using Dapper;
using MediatR;
using Npgsql;

namespace backend_admon.features.categorias.crear_categoria
{
    public class CrearCategoriaHandle
        : IRequestHandler<CrearCategoriaCommand, Result<CrearCategoriaResponse>>
    {
        private readonly IDbConnection _db;

        public CrearCategoriaHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<CrearCategoriaResponse>> Handle(
            CrearCategoriaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(request.name.Trim()))
            {
                return Result<CrearCategoriaResponse>.Failure("Tipo de nombre incorecto", 401);
            }

            string queryExist =
                @"SELECT EXISTS (SELECT 1 FROM categorias_productos WHERE nombre_categoria = @nombreCat)";

            bool existeCat = await _db.ExecuteScalarAsync<bool>(
                queryExist,
                new { nombreCat = request.name }
            );

            if (existeCat)
            {
                return Result<CrearCategoriaResponse>.Failure("La categoria ya existe", 409);
            }

            string queryCrear =
                @"INSERT INTO categorias_productos (nombre_categoria)
            VALUES (@nombreCat) 
            RETURNING id_categoria AS Id,nombre_categoria AS Nombre,fecha_creacion AS FechaCreacion;";

            try
            {
                Categoria categoria = await _db.QueryFirstAsync<Categoria>(
                    queryCrear,
                    new { nombreCat = request.name }
                );

                return Result<CrearCategoriaResponse>.Success(
                    new CrearCategoriaResponse(
                        true,
                        "Categoria creada con exito",
                        new CrearCategoriaDto(
                            id: categoria.Id,
                            nombre: categoria.Nombre,
                            fecha_creacion: categoria.FechaCreacion
                        )
                    ),
                    201
                );
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.MessageText);
                return Result<CrearCategoriaResponse>.Failure(ex.MessageText, 505);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
                return Result<CrearCategoriaResponse>.Failure("error", 505);
            }
        }
    }
}
