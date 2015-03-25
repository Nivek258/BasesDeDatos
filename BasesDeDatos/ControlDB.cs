using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class ControlDB
    {
        List<DataBase> listaDB;

        public void agregarDataBase(DataBase nuevaDB)
        {
            listaDB.Add(nuevaDB);
        }

        public Boolean existeDataBase(String nombreDataBase)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreDataBase))
                {
                    return true;
                }
            }
            return false;
        }

        public void removerDataBase(String nombreDataBase)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreDataBase))
                {
                    listaDB.RemoveAt(i);
                }
            }
        }

        public int numRegistros(String nombreDataBase)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreDataBase))
                {
                    return listaDB[i].getNumTablas();
                }
            }
            return -1;
        }
    }
}
