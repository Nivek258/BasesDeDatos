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
        public List<Columna> columasTB = new List<Columna>();

        public String getNombre()
        {
            return nombreTabla;
        }

        public void setNombre(String nombreTabla)
        {
            this.nombreTabla = nombreTabla;
        }

        public int getNumColumnas()
        {
            return numColumnas;
        }

        public void setNumColumnas(int numColumnas)
        {
            this.numColumnas = numColumnas;
        }

        public Boolean existeColumna(String nombreCol)
        {
            for (int i = 0; i < columasTB.Count(); i++)
            {
                if (columasTB[i].getNombre().Equals(nombreCol))
                {
                    return true;
                }
            }
            return false;
        }

        public void agregarColumna(Columna nuevaCol)
        {
            columasTB.Add(nuevaCol);
            numColumnas = numColumnas + 1;
        }

        public Boolean existeIdConstraint(String constraintId)
        {
            for (int i = 0; i < pConstraint.Count(); i++)
            {
                if (pConstraint[i].getPkNombre().Equals(constraintId))
                {
                    return true;
                }
            }

            for (int i = 0; i < fConstraint.Count(); i++)
            {
                if (fConstraint[i].getFkNombre().Equals(constraintId))
                {
                    return true;
                }
            }
            for (int i = 0; i < chConstraint.Count(); i++)
            {
                if (chConstraint[i].getChNombre().Equals(constraintId))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
