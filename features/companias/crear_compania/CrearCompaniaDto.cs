using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.companias.crear_compania
{
    public record CrearCompaniaDto(
        Guid idPublic,
        string nombre,
        DateTime fechaCreacionCompania,
        DateTime fechaCreacion
    );
}
