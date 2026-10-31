using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.tipos_productos.entities
{
    public class TipoProducto
    {
        public int Id;
        public required string Nombre;
        public DateTime FechaCreacion;
    }
}
