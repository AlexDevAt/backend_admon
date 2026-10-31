using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_admon.core.patterns;
using MediatR;

namespace backend_admon.features.companias.crear_compania
{
    public record CrearCompaniaCommand(
        string name,
        string descripcion,
        DateTime fecha_Creacion_Compania
    ) : IRequest<Result<CrearCompaniaResponse>>;
}
