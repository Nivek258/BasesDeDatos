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
        public List<Columna> columnasTB = new List<Columna>();

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

        public int getNumRegistros()
        {
            return numRegistros;
        }

        public void setNumRegistros(int numRegistros)
        {
            this.numRegistros = numRegistros;
        }

        public Boolean existeColumna(String nombreCol)
        {
            for (int i = 0; i < columnasTB.Count(); i++)
            {
                if (columnasTB[i].getNombre().Equals(nombreCol))
                {
                    return true;
                }
            }
            return false;
        }

        public void agregarColumna(Columna nuevaCol)
        {
            columnasTB.Add(nuevaCol);
            numColumnas = numColumnas + 1;
        }
        public void removerColumna(String idColumna)
        {
            for (int i = 0; i < columnasTB.Count; i++)
            {
                if (columnasTB[i].getNombre().Equals(idColumna))
                {
                    columnasTB.RemoveAt(i);
                }
            }
            numColumnas = numColumnas - 1;
        }
        public void removerConstraint(String nombreConstraint)
        {
            for (int i = 0; i < pConstraint.Count(); i++)
            {
                if (pConstraint[i].getPkNombre().Equals(nombreConstraint))
                {
                    pConstraint.RemoveAt(i);
                }
            }

            for (int i = 0; i < fConstraint.Count(); i++)
            {
                if (fConstraint[i].getFkNombre().Equals(nombreConstraint))
                {
                    fConstraint.RemoveAt(i);
                }
            }
            for (int i = 0; i < chConstraint.Count(); i++)
            {
                if (chConstraint[i].getChNombre().Equals(nombreConstraint))
                {
                    chConstraint.RemoveAt(i); ;
                }
            }
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

        public String tipoColumna(String nombreCol)
        {
            for (int i = 0; i < columnasTB.Count(); i++)
            {
                if (columnasTB[i].getNombre().Equals(nombreCol))
                {
                    return columnasTB[i].getTipo();
                }
            }
            return "";
        }
        public int indiceColumna(String nombreCol)
        {
            for (int i = 0; i < columnasTB.Count(); i++)
            {
                if (columnasTB[i].getNombre().Equals(nombreCol))
                {
                    return i;
                }
            }
            return -1;
        }


    }
}
