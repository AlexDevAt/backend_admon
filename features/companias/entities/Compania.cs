using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.features.companias.entities
{
    public class Compania
    {
        public int Id;
        public Guid IdPublic;
        public required string Nombre;
        public required string Descripcion;
        public DateTime FechaCreacionCompania;
        public DateTime FechaCreacion;
    }
}
