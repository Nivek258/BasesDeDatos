﻿// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    public class PrimaryConstraint
    {
        public String pkNombre = "";
        public List<String> idCol = new List<String>();

        public void setPkNombre(String pkNombre)
        {
            this.pkNombre = pkNombre;
        }

        public String getPkNombre()
        {
            return pkNombre;
        }
        //Metodo que verifica la existencia de una columna.
        public Boolean existeIdCol(String nombreCol)
        {
            for (int i = 0; i < idCol.Count(); i++)
            {
                if (idCol[i].Equals(nombreCol))
                {
                    return true;
                }
            }
            return false;
        }
        //Metodo que agrega una columna a la llave primaria.
        public void agregarPK(String nombreCol)
        {
            idCol.Add(nombreCol);
        }

    }
}
