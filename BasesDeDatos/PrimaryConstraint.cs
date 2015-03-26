using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class PrimaryConstraint
    {
        public String pkNombre = "";
        public List<String> idCol = new List<String>();

        public void setPkNombre(String pkNombre)
        {
            this.pkNombre = pkNombre;
        }

        public String getPkNombre()
        {
            return pkNombre;
        }

        public Boolean existeIdCol(String nombreCol)
        {
            for (int i = 0; i < idCol.Count(); i++)
            {
                if (idCol[i].Equals(nombreCol))
                {
                    return true;
                }
            }
            return false;
        }

        public void agregarPK(String nombreCol)
        {
            idCol.Add(nombreCol);
        }

    }
}
