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
    public class DataBase
    {
        public String nombre;
        public int numTablas;
        

        //public DataBase(String nombre, int numTablas){
        //    this.nombre = nombre;
        //    this.numTablas = numTablas;
        //}

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
