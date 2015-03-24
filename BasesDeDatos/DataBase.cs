using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class DataBase
    {
        int numTablas;
        String nombre;
        
        DataBase(String nombre, int numTablas){
            this.nombre = nombre;
            this.numTablas = numTablas;
        }

        public int getNumTablas()
        {
            return this.numTablas;
        }

        public String getNombre()
        {
            return this.nombre;
        }

        public void setNumTablas(int numTablas)
        {
            this.numTablas = numTablas;
        }

        public void setNombre(String nombre)
        {
            this.nombre = nombre;
        }
    }
}
