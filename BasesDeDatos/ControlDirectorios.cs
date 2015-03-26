using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BasesDeDatos
{
    class ControlDirectorios
    {
        ControlDB basesCreadas = new ControlDB();
        String DBactual = "";
        ControlTB tablasCreadas = new ControlTB();
        public void inicializar()
        {
            if (File.Exists("DataDB\\archivoM.dat") == false)
            {
                File.Create("DataDB\\archivoM.dat").Dispose();
            }
            Console.WriteLine("paso1");
        }

        public Boolean existeDB(String nombreDB)
        {
            Boolean existe = basesCreadas.existeDataBase(nombreDB);
            if (existe)
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
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\archivoM.dat");
                basesCreadas = DeSerializarDB(contenido);
            }
            catch (Exception e)
            {

            }
            DataBase nuevaDB = new DataBase();
            nuevaDB.setNombre(nombreDB);
            nuevaDB.setNumTablas(0);
            basesCreadas.agregarDataBase(nuevaDB);
            //File.AppendAllText("DataDB\\archivoM.dat", "name: " + nombreDB + " int: 0" + Environment.NewLine);
            Directory.CreateDirectory("DataDB\\" + nombreDB);
            FileStream fs = File.Create("DataDB\\" + nombreDB + "\\controlTablas.dat");
            //Console.WriteLine("paso2");
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
        }

        public Boolean existeTabla(String nombreTabla)
        {
            //String contenido = "";
            //StreamReader myWriter = new StreamReader("DataDB\\" + DBactual + "\\controlTablas.dat");
            //contenido += myWriter.ReadToEnd();
            //myWriter.Close();
            //if (contenido.Contains(nombreTabla))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}   
            return tablasCreadas.existeTabla(nombreTabla);
         
        }
        public void agregarTabla(String nombreTabla, int cantidadRegs, List<Columna> datosColumnas)
        {
            String textoAppend = "name: " + nombreTabla + " registros: 0 columnas: [" ;
            for(int i=0; i<datosColumnas.Count;i++){
                textoAppend+= "[ " + datosColumnas[i].getNombre() + ", " + datosColumnas[i].getTipo() + ", ";
                for(int j=0; j<datosColumnas[i].getRestricciones().Count;j++){
                    textoAppend+= datosColumnas[j].getRestricciones();
                    if(j!=datosColumnas[i].getRestricciones().Count-1){
                        textoAppend+= ", ";
                    }
                }
                textoAppend += " ]";
                if(i!=datosColumnas.Count-1){
                    textoAppend+= ", ";
                }

            }
            textoAppend += "]";
            File.AppendAllText("DataDB\\" + DBactual + "\\controlTablas.dat", textoAppend + Environment.NewLine);
        }

        public String SerializarDB(ControlDB Obj)
        {
            XmlSerializer serializer = new XmlSerializer(Obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Obj);
                return writer.ToString();
            }
        }

        public ControlDB DeSerializarDB(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlDB));
            ControlDB miControlTemp = (ControlDB)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }
    }
}