using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using backend_admon.features.companias.entities;
using Dapper;
using MediatR;
using Npgsql;

namespace backend_admon.features.companias.crear_compania
{
    public class CrearCompaniaHandle
        : IRequestHandler<CrearCompaniaCommand, Result<CrearCompaniaResponse>>
    {
        private readonly IDbConnection _db;

        public CrearCompaniaHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<CrearCompaniaResponse>> Handle(
            CrearCompaniaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(request.name))
            {
                return Result<CrearCompaniaResponse>.Failure(
                    "Error al leer el campo nombre de la compañia",
                    401
                );
            }

            string queryExist =
                @"SELECT EXISTS (SELECT 1 FROM Companias WHERE nombre_compania = @nombreCompania)";

            bool existeCat = await _db.ExecuteScalarAsync<bool>(
                queryExist,
                new { nombreCompania = request.name }
            );

            if (existeCat)
            {
                return Result<CrearCompaniaResponse>.Failure("La compañia ya existe", 409);
            }

            string queryCrear =
                @"INSERT INTO Companias 
            (id_compania_public,nombre_compania,descripcion,fecha_creacion_compania)
            VALUES (@uuid,@nombreCompania,@descripcion,@fechaCreacionCompania)
            RETURNING id_compania AS Id, id_compania_public AS IdPublic, 
            nombre_compania AS Nombre, descripcion AS Descripcion,
            fecha_creacion_compania AS FechaCreacionCompania, 
            fecha_creacion AS FechaCreacion;";

            try
            {
                Compania compania = await _db.QueryFirstAsync<Compania>(
                    queryCrear,
                    new
                    {
                        uuid = Guid.CreateVersion7(),
                        nombreCompania = request.name,
                        descripcion = request.descripcion,
                        fechaCreacionCompania = request.fecha_Creacion_Compania,
                    }
                );

                CrearCompaniaDto crearCompaniaDto = new CrearCompaniaDto(
                    compania.IdPublic,
                    compania.Nombre,
                    compania.FechaCreacionCompania,
                    compania.FechaCreacion
                );

                return Result<CrearCompaniaResponse>.Success(
                    new CrearCompaniaResponse(true, "Compañia creada con exito", crearCompaniaDto),
                    201
                );
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.MessageText);
                return Result<CrearCompaniaResponse>.Failure(ex.MessageText, 505);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
                return Result<CrearCompaniaResponse>.Failure("error", 505);
            }
        }
    }
}
