using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.companias.crear_compania
{
    public record CrearCompaniaResponse(bool status, string message, CrearCompaniaDto compania);
}
