using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class ControlDB
    {
        public List<DataBase> listaDB = new List<DataBase>();

        public void agregarDataBase(DataBase nuevaDB)
        {
            listaDB.Add(nuevaDB);
        }

        public void cambiarNombreDB(String nombreViejo, String nombreNuevo)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreViejo))
                {
                    listaDB[i].setNombre(nombreNuevo);
                }
            }
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
        public void agregarCountTabla(String nombreDataBase)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreDataBase))
                {
                    int numTabla = listaDB[i].getNumTablas()+1;
                    listaDB[i].setNumTablas(numTabla);
                }
            }
        }
        public void restarCountTabla(String nombreDataBase)
        {
            for (int i = 0; i < listaDB.Count; i++)
            {
                if (listaDB[i].getNombre().Equals(nombreDataBase))
                {
                    int numTabla = listaDB[i].getNumTablas() - 1;
                    listaDB[i].setNumTablas(numTabla);
                }
            }
        }
        public List<DataBase> getListaDB()
        {
            return listaDB;
        }

    }
}
