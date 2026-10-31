using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.categorias.crear_categoria
{
    public record CrearCategoriaDto(int id, string nombre, DateTime fecha_creacion);
}
