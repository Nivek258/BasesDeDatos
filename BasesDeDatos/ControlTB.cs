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
                    return listaTB[i].getNumTablas();
                }
            }
            return -1;
        }
    }
}
