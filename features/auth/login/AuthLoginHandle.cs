using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.helpers.dtos;
using backend_admon.core.helpers.hashear;
using backend_admon.core.patterns;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace backend_admon.features.auth.login
{

    public class AuthLoginHandle : IRequestHandler<AuthLoginCommand, Result<AuthLoginResponse>>
    {


        private readonly IDbConnection _db;

        public AuthLoginHandle(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Result<AuthLoginResponse>> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) )
            {
                return Result<AuthLoginResponse>.Failure("",400,false);
            }

            string sql = @"SELECT password_hash FROM Usuarios WHERE email = @email";

            string? passwordHasher = await _db.QueryFirstOrDefaultAsync<string>(sql, new
            {
                email = request.Email
            });
            if(passwordHasher == null)return Result<AuthLoginResponse>.Failure("Error en las credenciales",400,false);
            bool isCorrect = new HashBCrypt().VerifyHash(request.Password,passwordHasher);
            if (!isCorrect) return Result<AuthLoginResponse>.Failure("Error en las credenciales",400,false);

            sql = @"SELECT nombre_usuario,email,create_at FROM Usuarios WHERE email = @email;";

            UserDto? res = await  _db.QueryFirstOrDefaultAsync<UserDto>(sql, new
            {
                email = request.Email,
            });

            if(res == null)
            {
                return Result<AuthLoginResponse>.Failure("Error en las credenciales",400,false);
            }

            UserDtoLogin userDtoLogin = new UserDtoLogin(res.nombre_usuario,res.email,"usuario",res.create_at);

            return Result<AuthLoginResponse>.Success(new AuthLoginResponse(true,"","",userDtoLogin),200);
        }
    }
    
}



