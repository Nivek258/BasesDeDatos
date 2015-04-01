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

        public void agregarTabla(Tabla nuevaTB)
        {
            listaTB.Add(nuevaTB);
        }
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
