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
        public ControlDB basesCreadas = new ControlDB();
        String DBactual = "";
        public ControlTB tablasCreadas = new ControlTB();
        ControlContenido miContenido = new ControlContenido();
        public void inicializar()
        {
            if (File.Exists("DataDB\\archivoM.dat") == false)
            {
                File.Create("DataDB\\archivoM.dat").Dispose();
            }
            Console.WriteLine("paso1");
        }
        public String obtenerTipoCol(String nombreTabla, String idCol)
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
            String tipoCol = tablasCreadas.tipoColumna(nombreTabla, idCol);
            return tipoCol;
        }
        public int obtenerIndiceCol(String nombreTabla, String idCol)
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
            int indiceCol = tablasCreadas.indiceColumna(nombreTabla, idCol);
            return indiceCol;
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
        public int obtenerNumColumnas(String nombreTabla)
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
            int numColumnas = tablasCreadas.numColumnas(nombreTabla);
            return numColumnas;
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
            File.Delete("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
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
            File.Create("DataDB\\" + DBactual + "\\" + nuevaTabla.getNombre()+".dat").Dispose();
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

        public ControlDB getBasesCreadas()
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
            return basesCreadas;
        }

        public ControlTB getTablasCreadas()
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
            return tablasCreadas;
        }
        public String SerializarContenido(ControlContenido contenidoObj)
        {
            XmlSerializer serializer = new XmlSerializer(contenidoObj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, contenidoObj);
                return writer.ToString();
            }
        }
        public ControlContenido DeSerializarContenido(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlContenido));
            ControlContenido miControlTemp = (ControlContenido)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }

        public Boolean revisarConstraint(List<Object> elementosIngreso, String nombreTabla)
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
            
            for (int i = 0; i < tablasCreadas.obtenerTabla(nombreTabla).chConstraint.Count; i++)
            {
                String check = tablasCreadas.obtenerTabla(nombreTabla).chConstraint[i].restriccionExp;
                List<string> checkElements = check.Split(new char[] { ' ' }).ToList();
                Boolean cumple = cumpleConstraint(elementosIngreso, checkElements, nombreTabla);
                if (!cumple)
                {
                    return false;
                }

            }
            return true;
        }

        public Boolean cumpleConstraint(List<Object> elementosIngreso, List<String> elementosCheck, String nombreTabla)
        {
            Stack<Boolean> stack = new Stack<Boolean>();
            List<String> nombresColumna= new List<String>();
            List<String> tiposColumna= new List<String>();
            Object elemento1 = null;
            Object elemento2 = null;
            String tipoElemento1 = "";
            String tipoElemento2 = "";
            Boolean op1 = false;
            Boolean op2 = false;
            Boolean elem1 = true;
            for (int i = 0; i < tablasCreadas.obtenerTabla(nombreTabla).columnasTB.Count; i++)
            {
                nombresColumna.Add(tablasCreadas.obtenerTabla(nombreTabla).columnasTB[i].getNombre());
                tiposColumna.Add(tablasCreadas.obtenerTabla(nombreTabla).columnasTB[i].getTipo());
            }
            for (int i = 0; i < elementosCheck.Count; i++)
            {
                if (nombresColumna.Contains(elementosCheck[i]))
                {
                    int indice = nombresColumna.IndexOf(elementosCheck[i]);
                    if (elem1)
                    {
                        elemento1 = elementosIngreso[indice];
                        if (elemento1 == null)
                        {
                            return false;
                        }
                        tipoElemento1 = tiposColumna[indice];
                        elem1 = false;
                    }
                    else
                    {
                        elemento2 = elementosIngreso[indice];
                        if (elemento2 == null)
                        {
                            return false;
                        }
                        tipoElemento2 = tiposColumna[indice];
                        elem1 = true;
                    }
                }
                else if (elementosCheck[i].Equals(">") || elementosCheck[i].Equals("<") || elementosCheck[i].Equals(">=") || elementosCheck[i].Equals("<=")) 
                {
                    if(elementosCheck[i].Equals(">"))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 > (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 > (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 > (Int32)elemento2;
                        }
                        else if(tipoElemento1.Equals("float") && tipoElemento2.Equals("float")){
                            op1 = (Single)elemento1 > (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 > (DateTime)elemento2;
                        }
                        stack.Push(op1);
                    }
                    else if (elementosCheck[i].Equals("<"))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 < (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 < (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 < (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 < (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 < (DateTime)elemento2;
                        }
                        stack.Push(op1);
                    }
                    else if (elementosCheck[i].Equals("<="))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 <= (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 <= (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 <= (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 <= (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 <= (DateTime)elemento2;
                        }
                        stack.Push(op1);
                    }
                    else if (elementosCheck[i].Equals(">="))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 >= (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 >= (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 >= (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 >= (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 >= (DateTime)elemento2;
                        }
                        stack.Push(op1);
                    }
                }
                else if (elementosCheck[i].Equals("=") || elementosCheck[i].Equals("<>"))
                {
                    if (elementosCheck[i].Equals("="))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 == (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 == (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 == (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 == (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 == (DateTime)elemento2;
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Equals("date"))
                        {
                            op1 = ((String)elemento1).Equals(((DateTime)elemento2).ToString());
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Contains("char"))
                        {
                            op1 = ((String)elemento1).Equals((String)elemento2);
                        }
                        stack.Push(op1);
                    }
                    else if (elementosCheck[i].Equals("<>"))
                    {
                        if (tipoElemento1.Equals("int") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Int32)elemento1 != (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("int") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Int32)elemento1 != (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("int"))
                        {
                            op1 = (Single)elemento1 != (Int32)elemento2;
                        }
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 != (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 != (DateTime)elemento2;
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Equals("date"))
                        {
                            op1 = !(((String)elemento1).Equals(((DateTime)elemento2).ToString()));
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Contains("char"))
                        {
                            op1 = !(((String)elemento1).Equals((String)elemento2));
                        }
                        stack.Push(op1);
                    }
                }
                else if (elementosCheck[i].ToLower().Equals("and"))
                {
                    op1 = stack.Pop();
                    op2 = stack.Pop();
                    stack.Push(op1 && op2);
                }
                else if (elementosCheck[i].ToLower().Equals("or"))
                {
                    op1 = stack.Pop();
                    op2 = stack.Pop();
                    stack.Push(op1 || op2);
                }
                else if (elementosCheck[i].ToLower().Equals("not"))
                {
                    op1 = stack.Pop();
                    stack.Push(!op1);
                }
                else
                {
                    if (elem1)
                    {
                        tipoElemento1 = tipoElemento(elementosCheck[i]);
                        if(tipoElemento1.Equals("int")){
                            elemento1 = Convert.ToInt32(elementosCheck[i]);
                        }
                        else if (tipoElemento1.Equals("float"))
                        {
                            elemento1 = Convert.ToSingle(elementosCheck[i]);
                        }
                        else if (tipoElemento1.Equals("date"))
                        {
                            String Fecha = elementosCheck[i].Replace("\'", "");
                            List<string> dateElements = Fecha.Split(new char[] { '-' }).ToList();
                            Fecha = dateElements[0] + "-";
                            if (dateElements[1].Length == 1)
                            {
                                Fecha += 0 + dateElements[1] + "-";
                            }
                            else
                            {
                                Fecha += dateElements[1] + "-";
                            }
                            if (dateElements[2].Length == 1)
                            {
                                Fecha += 0 + dateElements[2];
                            }
                            else
                            {
                                Fecha += dateElements[2];
                            }
                            DateTime date = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);
                            elemento1 = date;
                        }
                        else
                        {
                            elemento1 = elementosCheck[i];
                        }
                        elem1 = false;
                    }
                    else
                    {
                        tipoElemento2 = tipoElemento(elementosCheck[i]);
                        if (tipoElemento2.Equals("int"))
                        {
                            elemento2 = Convert.ToInt32(elementosCheck[i]);
                        }
                        else if (tipoElemento2.Equals("float"))
                        {
                            elemento2 = Convert.ToSingle(elementosCheck[i]);
                        }
                        else if (tipoElemento2.Equals("date"))
                        {
                            String Fecha = elementosCheck[i].Replace("\'", "");
                            List<string> dateElements = Fecha.Split(new char[] { '-' }).ToList();
                            Fecha = dateElements[0] + "-";
                            if (dateElements[1].Length == 1)
                            {
                                Fecha += 0 + dateElements[1] + "-";
                            }
                            else
                            {
                                Fecha += dateElements[1] + "-";
                            }
                            if (dateElements[2].Length == 1)
                            {
                                Fecha += 0 + dateElements[2];
                            }
                            else
                            {
                                Fecha += dateElements[2];
                            }
                            DateTime date = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);
                            elemento2 = date;
                        }
                        else
                        {
                            elemento2 = elementosCheck[i];
                        }
                        elem1 = true;
                    }
                    
                }
                
            }
            return stack.Pop();
        }

        public String tipoElemento(String elemento)
        {
            if (!(elemento[0].Equals('\'')))
            {
                if (elemento.Contains("."))
                {
                    return "float";
                }
                else
                {
                    return "int";
                }
            }
            else
            {
                try
                {
                    String Fecha = elemento.Replace("\'", "");
                    List<string> dateElements = Fecha.Split(new char[] { '-' }).ToList();
                    Fecha = dateElements[0] + "-";
                    if (dateElements[1].Length == 1)
                    {
                        Fecha += 0 + dateElements[1] + "-";
                    }
                    else
                    {
                        Fecha += dateElements[1] + "-";
                    }
                    if (dateElements[2].Length == 1)
                    {
                        Fecha += 0 + dateElements[2];
                    }
                    else
                    {
                        Fecha += dateElements[2];
                    }
                    DateTime date = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);
                    return "date";
                }
                catch (Exception e)
                {
                    return "char";
                }
            }
        }

        public void agregarFilaTabla(String nombreTabla, List<Object> fila)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            tablasCreadas.agregarRegistro(nombreTabla);
            miContenido.agregarFila(fila);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);

        }

        public Boolean existePrimaryKey(String nombreTabla, List<Object> fila)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
           List<PrimaryConstraint> tempPConstraint = tablasCreadas.obtenerTabla(nombreTabla).pConstraint;
            if (tempPConstraint.Count == 0)
            {
                return false;
            }
            else
            {
                List<String> columnasConstraint = tempPConstraint[0].idCol;
                List<int> indicesPConstraints = new List<int>();
                List<String> tiposPConstraints = new List<String>();
                for (int i = 0; i < columnasConstraint.Count; i++)
                {
                    int indice = tablasCreadas.indiceColumna(nombreTabla, columnasConstraint[i]);
                    String tipo = tablasCreadas.tipoColumna(nombreTabla, columnasConstraint[i]);
                    indicesPConstraints.Add(indice);
                    tiposPConstraints.Add(tipo);

                }
                String filaEntrante = "";
                for (int i = 0; i < indicesPConstraints.Count; i++)
                {
                    filaEntrante += fila[i].ToString()+" ";
                }
                
                List<List<Object>> contenidoFilas = miContenido.listObj;
                for (int i = 0; i < contenidoFilas.Count; i++)
                {
                    List<Object> filaTemp = contenidoFilas[i];
                    String FilaComparar = "";
                    for (int j = 0; j < indicesPConstraints.Count; j++)
                    {
                        FilaComparar += filaTemp[j].ToString()+" ";
                    }
                    if (filaEntrante.Equals(FilaComparar))
                    {
                        return true;
                    }

                }
                return false;
            }

        }

        public Boolean primaryNull(String nombreTabla, List<Object> fila)
        {
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            List<PrimaryConstraint> tempPConstraint = tablasCreadas.obtenerTabla(nombreTabla).pConstraint;
            if (tempPConstraint.Count == 0)
            {
                return false;
            }
            else
            {
                List<String> columnasConstraint = tempPConstraint[0].idCol;
                List<int> indicesPConstraints = new List<int>();
                for (int i = 0; i < columnasConstraint.Count; i++)
                {
                    int indice = tablasCreadas.indiceColumna(nombreTabla, columnasConstraint[i]);
                    indicesPConstraints.Add(indice);

                }
                for (int i = 0; i < indicesPConstraints.Count; i++)
                {
                    if (fila[i] == null)
                    {
                        return true;
                    }
                }

                return false;
            }

        }

        public int UpdateColumnas(List<Object> elementosIngreso, String nombreTabla, List<String> condicionWhere)
        {
            String contenido;
            int columnasUpdate = 0;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            List<int> indicesACambiar = new List<int>();
            //Guardar los indices a cambiar
            for (int i = 0; i < elementosIngreso.Count; i++)
            {
                if (elementosIngreso[i] != null)
                {
                    indicesACambiar.Add(i);
                }
            }
            //cambia elementos de la lista
            List<List<Object>> contenidoFilas = miContenido.listObj;
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = contenidoFilas[i];
                Boolean cumple = cumpleConstraint(filaTemp, condicionWhere, nombreTabla);
                if (cumple)
                {
                    for (int j = 0; j < indicesACambiar.Count; j++)
                    {
                        filaTemp[indicesACambiar[j]] = elementosIngreso[indicesACambiar[j]];
                    }

                    contenidoFilas[i] = filaTemp;
                    columnasUpdate = columnasUpdate + 1;
                }
                

            }
            //se guarda matriz
            miContenido.setListObj(contenidoFilas);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);
            return columnasUpdate;

            
        }

        public int UpdateColumnas(List<Object> elementosIngreso, String nombreTabla)
        {
            int columnasUpdate = 0;
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            List<int> indicesACambiar = new List<int>();
            //Guardar los indices a cambiar
            for (int i = 0; i < elementosIngreso.Count; i++)
            {
                if (elementosIngreso[i] != null)
                {
                    indicesACambiar.Add(i);
                }
            }
            //cambia elementos de la lista
            List<List<Object>> contenidoFilas = miContenido.listObj;
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = contenidoFilas[i];
                for (int j = 0; j < indicesACambiar.Count; j++)
                {
                    filaTemp[indicesACambiar[j]] = elementosIngreso[indicesACambiar[j]];
                }

                contenidoFilas[i] = filaTemp;
                columnasUpdate = columnasUpdate + 1;

            }
            //se guarda matriz
            miContenido.setListObj(contenidoFilas);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);
            return columnasUpdate;
        }

        public int DeleteFilas(String nombreTabla, List<String> condicionWhere)
        {
            String contenido;
            int columnasDelete = 0;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            
            //eliminar elementos de la lista
            List<List<Object>> contenidoFilas = miContenido.listObj;
            List<List<Object>> nuevoContenido = new List<List<Object>>();
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = contenidoFilas[i];
                Boolean cumple = cumpleConstraint(filaTemp, condicionWhere, nombreTabla);
                if (cumple)
                {
                    columnasDelete = columnasDelete + 1;
                }
                else
                {
                    nuevoContenido.Add(contenidoFilas[i]);
                }


            }
            //se guarda matriz
            miContenido.setListObj(nuevoContenido);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);
            tablasCreadas.removerRegistro(nombreTabla, columnasDelete);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            return columnasDelete;


        }

        public int DeleteFilas(String nombreTabla)
        {
            int columnasDelete = 0;
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                miContenido = DeSerializarContenido(contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            //elimina elementos de la lista
            List<List<Object>> contenidoFilas = miContenido.listObj;
            List<List<Object>> nuevoContenido = new List<List<Object>>();
            columnasDelete = contenidoFilas.Count;

            //se guarda matriz
            miContenido.setListObj(nuevoContenido);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);
            tablasCreadas.removerRegistro(nombreTabla, columnasDelete);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            return columnasDelete;
        }

        public List<List<Object>> SelectFilas(List<String> nombreTablas)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            //Lista de contenidos
            for (int i = 0; i < nombreTablas.Count; i++)
            {
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTablas[i] + ".dat");
                    miContenido = new ControlContenido();
                    miContenido = DeSerializarContenido(contenido);
                    coleccionContenido.Add(miContenido);
                }
                catch (Exception e)
                {
                    miContenido = new ControlContenido();
                    coleccionContenido.Add(miContenido);
                }
            }

            

            if (coleccionContenido.Count == 1)
            {
                productoCartesiano = coleccionContenido[0].listObj;
            }
            else
            {
                //Realizar producto cartesiano
                List<List<Object>> conjuntoTemp = new List<List<Object>>();
                productoCartesiano = coleccionContenido[0].listObj;
                for (int i = 1; i < coleccionContenido.Count; i++)
                {
                    conjuntoTemp = productoCartesiano;
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>;
                            fila1 = conjuntoTemp[j];
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[0]);
                            }
                            productoCartesiano.Add(fila1);
                                
                        }
                    }
                }
            }
            

            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
        }
        
    }
}