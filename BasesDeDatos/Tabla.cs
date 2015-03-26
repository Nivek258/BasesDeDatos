using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class Tabla
    {
        public String nombreTabla = "";
        public int numRegistros = 0;
        public List<PrimaryConstraint> pConstraint = new List<PrimaryConstraint>();
        public List<ForeignConstraint> fConstraint = new List<ForeignConstraint>();
        public List<CheckConstraint> chConstraint = new List<CheckConstraint>();
        public int numColumnas = 0;
        // public List<Columna> columasTB = new List<Columna>();
    }
}
