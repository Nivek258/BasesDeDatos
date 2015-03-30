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

        public int numRegistrosDB(String nombreDB)
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
            return basesCreadas.numRegistros(nombreDB);
        }
        public Boolean existeDB(String nombreDB)
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
        public void cambiarNombreDB(String nombreViejo, String nombreNuevo)
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
            basesCreadas.cambiarNombreDB(nombreViejo, nombreNuevo);
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
            Directory.Move("DataDB\\" + nombreViejo, "DataDB\\" + nombreNuevo);
        }
        public Tabla obtenerTabla(String nombreTabla)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            return tablasCreadas.obtenerTabla(nombreTabla);
        }
        public void sustituirTabla(String nombreTabla, Tabla nuevaTB) {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            tablasCreadas.sustituirTabla(nombreTabla, nuevaTB);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
        }
        public Boolean columnaEnConstraint(String nombreTabla, String idCol)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            Boolean existeColenConstraint = tablasCreadas.columnaEnCostraint(nombreTabla, idCol);
            return existeColenConstraint;

        }
        public Boolean tablaEnReferencia(String nombreTabla)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            Boolean existeColenConstraint = tablasCreadas.tablaEnReferencia(nombreTabla);
            return existeColenConstraint;

        }
        public void cambiarNombreTabla(String nombreViejo, String nombreNuevo)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            tablasCreadas.cambiarNombreTabla(nombreViejo, nombreNuevo);
            tablasCreadas.cambiarRefTabla(nombreViejo, nombreNuevo);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
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
            Directory.CreateDirectory("DataDB\\" + nombreDB);
            File.Create("DataDB\\" + nombreDB + "\\controlTablas.dat").Dispose();
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
        }
        public void removerDB(String nombreDB)
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
            basesCreadas.removerDataBase(nombreDB);
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("DataDB\\" + nombreDB);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            Directory.Delete("DataDB\\" + nombreDB);
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
        }
        public void removerTabla(String nombreTabla)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\archivoM.dat");
                basesCreadas = DeSerializarDB(contenido);
            }
            catch (Exception e)
            {

            }
            tablasCreadas.removerTabla(nombreTabla);
            basesCreadas.restarCountTabla(DBactual);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
        }
        public Boolean existeTabla(String nombreTabla)
        {  
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }

            return tablasCreadas.existeTabla(nombreTabla);
         
        }
        public void agregarTabla(Tabla nuevaTabla)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\archivoM.dat");
                basesCreadas = DeSerializarDB(contenido);
            }
            catch (Exception e)
            {

            }
            tablasCreadas.agregarTabla(nuevaTabla);
            basesCreadas.agregarCountTabla(DBactual);
            contenido = SerializarTabla (tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);

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
        public String SerializarTabla(ControlTB Obj)
        {
            XmlSerializer serializer = new XmlSerializer(Obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Obj);
                return writer.ToString();
            }
        }
        public ControlTB DeSerializarTabla(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlTB));
            ControlTB miControlTemp = (ControlTB)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }

        public Boolean existeColumna(String nombreTabl, String nombreCol)
        {
            Boolean respuesta = tablasCreadas.existeColumna(nombreTabl, nombreCol);
            return respuesta;
        }

        public void setDBActual(String nombreDB)
        {
            DBactual = nombreDB;
        }

        public String getDBActual()
        {
            return DBactual;
        }
    }
}