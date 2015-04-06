// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class Columna
    {
        public String nombre = "";
        public String tipo = "";

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

        public void setTipo(String tipo)
        {
            this.tipo = tipo;
        }

    }
}
