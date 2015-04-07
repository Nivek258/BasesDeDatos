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
    public class ControlContenido
    {
        public List<List<Object>> listObj = new List<List<Object>>();

        //Metodo que adjunta una fila a la tabla.
        public void agregarFila(List<Object> objetos)
        {
            listObj.Add(objetos);
        }
        public Boolean verificarIndices(int indice, Object elemento)
        {
            for (int i = 0; i < listObj.Count; i++)
            {
                if (listObj[i][indice] == elemento)
                {
                    return true;
                }
            }
            return false;
        }

        //Metodo que setea las filas a una tabla.
        public void setListObj(List<List<Object>> listObj)
        {
            this.listObj = listObj;
        }

    }
}
