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
    public class ControlTB
    {
        public List<Tabla> listaTB = new List<Tabla>();

        //Metodo que agrega una tabla a la base de datos.
        public void agregarTabla(Tabla nuevaTB)
        {
            listaTB.Add(nuevaTB);
        }
        //Metodo que devuelve un objeto tabla.
        public Tabla obtenerTabla(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    return listaTB[i];
                }
            }
            return null;
        }
        //Metodo que sustituye una tabla.
        public void sustituirTabla(String nombreTabla, Tabla nuevaTB)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    listaTB[i] = nuevaTB;  
                }
            }
        }
        //Metodo que cambia la referencia de una tabla a otra.
        public void cambiarRefTabla(String nombreTablaViejo, String nombreTablaNuevo)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                for (int j = 0; j < listaTB[i].fConstraint.Count; j++)
                {
                    if (listaTB[i].fConstraint[j].getTablaRefNombre().Equals(nombreTablaViejo))
                    {
                        listaTB[i].fConstraint[j].setTablaRefNombre(nombreTablaNuevo);
                    }
                }
            }
        }
        //Metodo que verifica la constraint que posee una columna.
        public Boolean columnaEnCostraint(String nombreTabla, String idCol)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                for (int j = 0; j < listaTB[i].fConstraint.Count; j++)
                {
                    if (listaTB[i].fConstraint[j].getTablaRefNombre().Equals(nombreTabla))
                    {
                        for (int k = 0; k < listaTB[i].fConstraint[j].refCol.Count; k++)
                        {
                            if (listaTB[i].fConstraint[j].refCol[k].Equals(idCol))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //Metodo que verifica que una tabla esta siendo referenciada en otra tabla.
        public Boolean tablaEnReferencia(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                for (int j = 0; j < listaTB[i].fConstraint.Count; j++)
                {
                    if (listaTB[i].fConstraint[j].getTablaRefNombre().Equals(nombreTabla))
                    {
                       return true;
                    }
                }
            }
            return false;
        }

        public Boolean columnaEnPrimary(String nombreTabla, String nombreColumna)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    
                    if (listaTB[i].pConstraint[0].existeIdCol(nombreColumna))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        public Boolean existeTabla(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    return true;
                }
            }
            return false;
        }
        //Metodo que elimina una tabla.
        public void removerTabla(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    listaTB.RemoveAt(i);
                }
            }
        }
        //Metodo que devuelve la cantidad de columnas en una tabla.
        public int numColumnas(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    return listaTB[i].getNumColumnas();
                }
            }
            return -1;
        }
        //Metodo que verifica la existencia de una columna en una tabla.
        public Boolean existeColumna(String nombreTabl, String nombreCol)
        {
            for (int i = 0; i < listaTB.Count(); i++)
            {
                if (nombreTabl.Equals(listaTB[i].getNombre()))
                {
                    return listaTB[i].existeColumna(nombreCol);
                }
            }
            return false;
        }
        //Metodo que obtiene el tipo de una columna.
        public String tipoColumna(String nombreTabl, String nombreCol)
        {
            for (int i = 0; i < listaTB.Count(); i++)
            {
                if (nombreTabl.Equals(listaTB[i].getNombre()))
                {
                    return listaTB[i].tipoColumna(nombreCol);
                }
            }
            return "";
        }
        //Metodo que devuelve el indice de una columna.
        public int indiceColumna(String nombreTabl, String nombreCol)
        {
            for (int i = 0; i < listaTB.Count(); i++)
            {
                if (nombreTabl.Equals(listaTB[i].getNombre()))
                {
                    return listaTB[i].indiceColumna(nombreCol);
                }
            }
            return -1;
        }
        //Metodo que cambia el nombre de una tabla.
        public void cambiarNombreTabla(String nombreViejo, String nombreNuevo)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreViejo))
                {
                    listaTB[i].setNombre(nombreNuevo);
                }
            }
        }
        public List<Tabla> getListaTB()
        {
            return listaTB;
        }
        //Metodo que agrega un registro a una tabla.
        public void agregarRegistro(String nombreTabla)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    listaTB[i].agregarRegistro();
                }
            }
        }
        //Metodo que elimna un registro de una tabla.
        public void removerRegistro(String nombreTabla, int registrosEliminados)
        {
            for (int i = 0; i < listaTB.Count; i++)
            {
                if (listaTB[i].getNombre().Equals(nombreTabla))
                {
                    listaTB[i].removerRegistro(registrosEliminados);
                }
            }
        }
    }
}
