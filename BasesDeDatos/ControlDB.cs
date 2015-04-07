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
    public class ControlDB
    {
        public List<DataBase> listaDB = new List<DataBase>();

        //Metodo que agrega una base de datos.
        public void agregarDataBase(DataBase nuevaDB)
        {
            listaDB.Add(nuevaDB);
        }
        //Metodo que cambia el nombre de una base de datos.
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
        //Metodo que verifica la existencia de una base de datos.
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
        //Metodo que elimina una base de datos.
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
        //Metodo que devuelve la cantidad de registros de una base de datos.
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
        //Metodo que suma 1 a la cantidad de registros de una base de datos.
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
        //Metodo que resta 1 a la cantidad de registros de una base de datos.
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
