using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.helpers.hashear;
using backend_admon.core.patterns;
using Dapper;
using MediatR;
using Npgsql;

namespace backend_admon.features.user.registerUser
{
    public class UserRegisterHandler : IRequestHandler<UserRegisterCommand,Result<UserRegisterResponse>>
    {

        private readonly IDbConnection _db;
        public UserRegisterHandler(IDbConnection db)
        {
            _db = db;
        }


        async Task<Result<UserRegisterResponse>> IRequestHandler<UserRegisterCommand, Result<UserRegisterResponse>>.Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            string sql = $"SELECT EXISTS (SELECT 1 FROM Usuarios WHERE email = @Email)";

            bool res = await _db.ExecuteScalarAsync<bool>(sql, new {Email = request.Email});
            if(res)
            {
                return Result<UserRegisterResponse>.Failure("El usuario ya existe",409,false);
            }
            sql = @"INSERT INTO Usuarios (nombres,apellido_paterno,apellido_materno,nombre_usuario,email,password_hash) 
            Values (@Nombres,@ApellidoPaterno,@ApellidoMaterno,@NombreUsuario,@Email,@PasswordHash) RETURNING public_id";

            try
            {
                var nuevoUsuarioIdPublic = await _db.ExecuteScalarAsync(sql, 
                new {Nombres = request.Name,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                NombreUsuario=request.NombreUsuario,
                Email = request.Email,
                PasswordHash=new HashBCrypt().Hashear(request.password)});
                 return Result<UserRegisterResponse>
                .Success(new UserRegisterResponse(
                Guid.Parse(nuevoUsuarioIdPublic.ToString()),request.Name,request.Email),200);
            }
            catch (PostgresException ex) when (ex.SqlState== "23505")
            {
                return Result<UserRegisterResponse>.Failure("El nombre de usuario ya existe",409,false);
            }
            catch (Exception ex)
            {
                return Result<UserRegisterResponse>.Failure(ex.Data.ToString(),500,false);
                
            }


        }
    }
}