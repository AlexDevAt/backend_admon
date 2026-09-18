using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.core.helpers.dtos
{
    public record UserDto(
    int id,
    Guid public_id,
    string nombres,
    string apellido_paterno,
    string? apellido_materno,
    string nombre_usuario,
    string email,
    string password_hash,
    DateTime create_at


)
    {
        public UserDto() : this(0, Guid.Empty, string.Empty, string.Empty, null, string.Empty, string.Empty, string.Empty, default) { }
    };
}