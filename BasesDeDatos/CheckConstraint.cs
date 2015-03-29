using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class CheckConstraint
    {
        public String chNombre = "";
        public String restriccionExp = "";

        public void setChNombre(String chNombre)
        {
            this.chNombre = chNombre;
        }

        public String getChNombre()
        {
            return chNombre;
        }

        
        public void setRestriccionExp(String restExp)
        {
            restriccionExp = restExp;
        }



    }
}
