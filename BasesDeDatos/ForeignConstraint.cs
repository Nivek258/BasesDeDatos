using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class ForeignConstraint
    {
        public String fkNombre = "";
        public List<String> idCol = new List<String>();
        public String nombreTabla = "";
        public List<String> refCol = new List<String>();

        public String getFkNombre()
        {
            return fkNombre;
        }
    }
}
