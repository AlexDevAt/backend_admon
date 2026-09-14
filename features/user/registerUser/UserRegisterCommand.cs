using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.user.registerUser
{
    public record UserRegisterCommand(
        string Name,
        string ApellidoPaterno,
        string? ApellidoMaterno,
        string NombreUsuario,
        string Email,
        string password
    ):IRequest<Result<UserRegisterResponse>>;
}