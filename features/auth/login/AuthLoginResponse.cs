using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.auth.login
{
    public record AuthLoginResponse
    (
        bool Status,
        string Token,
        string RefreshToken,
        UserDtoLogin User
    );

    public record UserDtoLogin
    (
        string Nombre_usuario,
        string Email,
        string Role,
        DateTime Create_at
    );
        
}