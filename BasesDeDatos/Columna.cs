using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class Columna
    {
        String nombre = "";
        String tipo = "";
        List<String> restricciones = new List<String>();

        public String getNombre()
        {
            return this.nombre;
        }

        public void setNombre(String nombre)
        {
            this.nombre = nombre;
        }

        public String getTipo()
        {
            return this.tipo;
        }

        public void setNombre(String tipo)
        {
            this.tipo = tipo;
        }
        public List<String> getRestricciones()
        {
            return this.restricciones;
        }

        public void setNombre(List<String> restricciones)
        {
            this.restricciones = restricciones;
        }




    }
}
