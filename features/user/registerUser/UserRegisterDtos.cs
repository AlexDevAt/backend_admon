using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.user.registerUser
{
    public record UserRegisterDto(
        Guid Id_public,
        string Nombre_usuario,
        string Email,
        DateTime CreateAt,
        bool Status
    );
    public record UserRegistroFromDB
    (
        Guid public_id,
        DateTime create_at
    );
}