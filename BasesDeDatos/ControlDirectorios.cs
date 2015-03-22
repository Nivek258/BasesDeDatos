using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class ControlDirectorios
    {

        public void inicializar()
        {
            if (File.Exists("..\\DataDB\\archivoM.dat") == false)
            {
                FileStream fs = File.Create("..\\DataDB\\archivoM.dat");
            }
        }

        public Boolean existeDB(String nombreDB)
        {
            String contenido = "";
            StreamReader myWriter = new StreamReader("..\\DataDB\\archivoM.dat");
            contenido += myWriter.ReadToEnd();
            myWriter.Close();

            if (contenido.Contains(nombreDB))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void agregarDB(String nombreDB)
        {
            File.AppendAllText("..\\DataDB\\archivoM.dat", "name: "+nombreDB +" int: 0"+Environment.NewLine);
            Directory.CreateDirectory("..\\DataDB\\"+nombreDB);
        }
        
    }
}
