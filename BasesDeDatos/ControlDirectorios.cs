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
        String DBactual = "";
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
            FileStream fs = File.Create("..\\DataDB\\" + nombreDB+"\\controlTablas.dat");
        }

        public Boolean existeTabla(String nombreTabla)
        {
            String contenido = "";
            StreamReader myWriter = new StreamReader("..\\DataDB\\" + DBactual + "\\controlTablas.dat");
            contenido += myWriter.ReadToEnd();
            myWriter.Close();
            if (contenido.Contains(nombreTabla))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        public void agregarTabla(String nombreTabla, int cantidadRegs, List<Columna> datosColumnas)
        {
            String textoAppend = "name: " + nombreTabla + " registros: 0 columnas: [" ;
            for(int i=0; i<datosColumnas.Count;i++){
                textoAppend+= "[";
                for(int j=0; j<datosColumnas)
            }
            File.AppendAllText("..\\DataDB\\" + DBactual + "\\controlTablas.dat", ) ;
        }
    }
}
