using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.categorias.entities
{
    public class Categoria
    {
        public int Id;
        public required string Nombre;
        public DateTime FechaCreacion;
    }
}
