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

        public void setListObj(List<List<Object>> listObj)
        {
            this.listObj = listObj;
        }

    }
}
