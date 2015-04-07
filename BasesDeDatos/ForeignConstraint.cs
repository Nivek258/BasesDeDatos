// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

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

        public void setFkNombre(String fkNombre)
        {
            this.fkNombre = fkNombre;
        }

        public String getTablaRefNombre()
        {
            return nombreTabla ;
        }

        public void setTablaRefNombre(String nombreTabla)
        {
            this.nombreTabla = nombreTabla;
        }

        //Metodo que everifica la existencia de una columna en el constraint.
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
        //Metodo que agrega una columna al constraint.
        public void agregarFK(String nombreCol)
        {
            idCol.Add(nombreCol);
        }

        //Metodo que verifica la existencia de una columna referenciada.
        public Boolean existeRefCol(String nombreCol)
        {
            for (int i = 0; i < refCol.Count(); i++)
            {
                if (refCol[i].Equals(nombreCol))
                {
                    return true;
                }
            }
            return false;
        }
        //Metodo que agrega las columnas a la referencia.
        public void agregarRefCol(String nombreCol)
        {
            refCol.Add(nombreCol);
        }

    }
}
