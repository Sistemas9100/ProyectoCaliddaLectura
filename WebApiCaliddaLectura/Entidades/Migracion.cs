using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Migracion
    {
        public int migrationId { get; set; }
        public List<Servicio> servicios { get; set; }
        public List<Parametro> parametros { get; set; }
        public List<Suministro> suministroLecturas { get; set; }
        public List<Suministro> suministroCortes { get; set; }
        public List<Suministro> suministroReconexiones { get; set; }
        public List<TipoLectura> tipoLecturas { get; set; }
        public List<DetalleGrupo> detalleGrupos { get; set; }
        public List<Reparto> repartoLectura { get; set; }
    }
}
