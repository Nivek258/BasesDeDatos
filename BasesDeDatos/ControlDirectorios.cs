// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public List<String> tituloColumnas = new List<String>();
        String contenidoBase = "";
        String contenidoTabla = "";
        Boolean deUpdate = false;
        public Boolean falloEliminacion = false;
        public String mensajeFallo = "";
        public Boolean falloUpdate = false;
        public String mensajeFallo2 = "";


        //Metodo que revisa si existe el archivo maestro de bases de datos, si no existe lo crea, de lo contrario lee la informacion del archivo maestro.
        public void inicializar()
        {
            if (File.Exists("DataDB\\archivoM.dat") == false)
            {
                File.Create("DataDB\\archivoM.dat").Dispose();
            }
            String contenido;
            try
            {
                contenido = File.ReadAllText("DataDB\\archivoM.dat");
                basesCreadas = DeSerializarDB(contenido);
            }
            catch (Exception e)
            {

            }
        }

        //Metodo que almancena la informacion de bases de datos, datos de tablas e informacion de los registros al finalizar la ejecucion del programa.
        public void terminar()
        {
            String contenido;
            try
            {
                contenido = SerializarTabla(tablasCreadas);
                File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            }
            catch (Exception e)
            {

            }

            try
            {
                contenido = SerializarDB(basesCreadas);
                File.WriteAllText("DataDB\\archivoM.dat", contenido);
            }
            catch (Exception e)
            {

            }
            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            
        }


        //Metodo que obtiene el tipo de una columna.
        public String obtenerTipoCol(String nombreTabla, String idCol)
        {

            String tipoCol = tablasCreadas.tipoColumna(nombreTabla, idCol);
            return tipoCol;
        }
        //Metodo que obtiene el indice de una columna.
        public int obtenerIndiceCol(String nombreTabla, String idCol)
        {
            int indiceCol = tablasCreadas.indiceColumna(nombreTabla, idCol);
            return indiceCol;
        }
        //Metodo que obtiene la cantidad de registros que posee una base de datos.
        public int numRegistrosDB(String nombreDB)
        {
            return basesCreadas.numRegistros(nombreDB);
        }
        //Metodo que obtiene la cantidad de columnas que posee una tabla.
        public int obtenerNumColumnas(String nombreTabla)
        {
            int numColumnas = tablasCreadas.numColumnas(nombreTabla);
            return numColumnas;
        }
        //Metodo que revisa la existencia de una base de datos.
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
        //Metodo que cambia el nombre de una base de datos existente.
        public void cambiarNombreDB(String nombreViejo, String nombreNuevo)
        {
            String contenido;
            basesCreadas.cambiarNombreDB(nombreViejo, nombreNuevo);
            Directory.Move("DataDB\\" + nombreViejo, "DataDB\\" + nombreNuevo);
        }
        //Metodo que obtiene toda una tabla en la base de datos actual.
        public Tabla obtenerTabla(String nombreTabla)
        {
            return tablasCreadas.obtenerTabla(nombreTabla);
        }
        //Metodo que sustituye una tabla, es decir crear una nueva tabla con la informacion de la tabla anterior.
        public void sustituirTabla(String nombreTabla, Tabla nuevaTB) {
            String contenido;
            tablasCreadas.sustituirTabla(nombreTabla, nuevaTB);
        }
        //Metodo que revista la constraint que posee la columna especificada.
        public Boolean columnaEnConstraint(String nombreTabla, String idCol)
        {
            Boolean existeColenConstraint = tablasCreadas.columnaEnCostraint(nombreTabla, idCol);
            return existeColenConstraint;

        }
        //Metodo que revisa si la tabla esta siendo referenciada por otra tabla en una foreing key.
        public Boolean tablaEnReferencia(String nombreTabla)
        {
            Boolean existeColenConstraint = tablasCreadas.tablaEnReferencia(nombreTabla);
            return existeColenConstraint;

        }
        //Metodo que cambia el nombre de una tabla.
        public void cambiarNombreTabla(String nombreViejo, String nombreNuevo)
        {
            String contenido;
            tablasCreadas.cambiarNombreTabla(nombreViejo, nombreNuevo);
            tablasCreadas.cambiarRefTabla(nombreViejo, nombreNuevo);
        }
        //Metodo que crea una base de datos.
        public void agregarDB(String nombreDB)
        {
            String contenido;
            DataBase nuevaDB = new DataBase();
            nuevaDB.setNombre(nombreDB);
            nuevaDB.setNumTablas(0);
            basesCreadas.agregarDataBase(nuevaDB);
            Directory.CreateDirectory("DataDB\\" + nombreDB);
            File.Create("DataDB\\" + nombreDB + "\\controlTablas.dat").Dispose();
        }
        //Metodo que elimina una base de datos.
        public void removerDB(String nombreDB)
        {
            basesCreadas.removerDataBase(nombreDB);
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("DataDB\\" + nombreDB);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            Directory.Delete("DataDB\\" + nombreDB);
        }
        //Metodo que elimina una tabla.
        public void removerTabla(String nombreTabla)
        {
            File.Delete("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
            tablasCreadas.removerTabla(nombreTabla);
            basesCreadas.restarCountTabla(DBactual);
        }
        //Metodo que verifica la existencia de una tabla.
        public Boolean existeTabla(String nombreTabla)
        {  
            return tablasCreadas.existeTabla(nombreTabla);         
        }
        //Metodo que afrega una nueva tabla a la base de datos actual.
        public void agregarTabla(Tabla nuevaTabla)
        {
            File.Create("DataDB\\" + DBactual + "\\" + nuevaTabla.getNombre()+".dat").Dispose();
            tablasCreadas.agregarTabla(nuevaTabla);
            basesCreadas.agregarCountTabla(DBactual);
        }
        //Metodo que serializa las bases de datos en el archivo maestro.
        public String SerializarDB(ControlDB Obj)
        {
            XmlSerializer serializer = new XmlSerializer(Obj.GetType());
            BinaryFormatter formatter = new BinaryFormatter();
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Obj);
                return writer.ToString();
            }
        }
        //Metodo que deserializa las bases de datos en el archivo maestro.
        public ControlDB DeSerializarDB(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlDB));
            ControlDB miControlTemp = (ControlDB)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }
        //Metodo que serializa la informacion de una tabla.
        public String SerializarTabla(ControlTB Obj)
        {
            XmlSerializer serializer = new XmlSerializer(Obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, Obj);
                return writer.ToString();
            }
        }
        //Metodo que deserializa la informacion de una tabla.
        public ControlTB DeSerializarTabla(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlTB));
            ControlTB miControlTemp = (ControlTB)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }
        //Metodo que verifica la existencia de una columna en una tabla.
        public Boolean existeColumna(String nombreTabl, String nombreCol)
        {
            Boolean respuesta = tablasCreadas.existeColumna(nombreTabl, nombreCol);
            return respuesta;
        }
        //Metodo que verifica que una columna determinada sea primary key en la tabla determinada
        public Boolean columnaEnPrimaryK(String nombreTabl, String nombreCol)
        {
            Boolean respuesta = tablasCreadas.columnaEnPrimary(nombreTabl, nombreCol);
            return respuesta;
        }
        //Metodo que setea la base de datos en la cual se esta trabajando.
        public void setDBActual(String nombreDB)
        {
            String contenido = SerializarDB(basesCreadas);
            File.WriteAllText("DataDB\\archivoM.dat", contenido);
            DBactual = nombreDB;
            try
            {
                contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
                tablasCreadas = DeSerializarTabla(contenido);
            }
            catch (Exception e)
            {

            }
            contenidoBase = DBactual;
        }
        //Metodo que obtiene la base de datos actual.
        public String getDBActual()
        {
            return DBactual;
        }
        //Metodo que obtiene las bases de datos existentes en el sistema.
        public ControlDB getBasesCreadas()
        {
            String contenido;
            return basesCreadas;
        }
        //Metodo que obtiene las tablas de una base de datos.
        public ControlTB getTablasCreadas()
        {
            return tablasCreadas;
        }
        //Metodo que serializa la informacion (registros, valores, etc) de una tabla.
        public String SerializarContenido(ControlContenido contenidoObj)
        {
            XmlSerializer serializer = new XmlSerializer(contenidoObj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, contenidoObj);
                return writer.ToString();
            }
        }
        //Metodo que deserializa la informacion (registros, valores, etc) de una tabla.
        public ControlContenido DeSerializarContenido(String XmlText)
        {
            XmlSerializer x = new XmlSerializer(typeof(ControlContenido));
            ControlContenido miControlTemp = (ControlContenido)x.Deserialize(new StringReader(XmlText));
            return miControlTemp;
        }
        //Metodo que revisa las constraints de una tabla.
        public Boolean revisarConstraint(List<Object> elementosIngreso, String nombreTabla)
        {            
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

        //Metodo que determina si todos los elementos de la tabla cumplen con el check
        public Boolean tablaCumpleCheck(String nombreTabla, List<String> elementosCheck)
        {
            String contenido;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
            //recorrer lineas para verificar que cumpla check
            for (int i = 0; i < miContenido.listObj.Count; i++)
            {
                List<Object> filaTemp = miContenido.listObj[i];
                Boolean cumple = cumpleConstraint(filaTemp, elementosCheck, nombreTabla);
                if (!cumple)
                {
                    return false;
                }
                
            }
            return true;

        }

        //Metodo que verifica que los datos que se ingresen cumplan con las constraint especificadas en una tabla.
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
                            op1 = ((((String)elemento1).Replace("'", "")).Equals(((DateTime)elemento2).ToString()));
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
                            op1 = !((((String)elemento1).Replace("'","")).Equals(((DateTime)elemento2).ToString()));
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
        //Metodo que obtiene el tipo de un elemento a ingresar (e.g:  5 es int)
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
                    if (dateElements.Count == 3)
                    {
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
                    else
                    {
                        return "char";
                    }
                    
                }
                catch (Exception e)
                {
                    return "char";
                }
            }
        }
        //Metodo que agrega una fila a una tabla.
        public void agregarFilaTabla(String nombreTabla, List<Object> fila)
        {
            String contenido;
            tablasCreadas.agregarRegistro(nombreTabla);
            miContenido.agregarFila(fila);
        }
        //Metodo que verifica la unicidad de una primary key.
        public Boolean existePrimaryKey(String nombreTabla, List<Object> fila)
        {
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
                    filaEntrante += fila[indicesPConstraints[i]].ToString()+" ";
                }
                
                List<List<Object>> contenidoFilas = miContenido.listObj;
                for (int i = 0; i < contenidoFilas.Count; i++)
                {
                    List<Object> filaTemp = contenidoFilas[i];
                    String FilaComparar = "";
                    for (int j = 0; j < indicesPConstraints.Count; j++)
                    {
                        FilaComparar += filaTemp[indicesPConstraints[j]].ToString() + " ";
                    }
                    if (filaEntrante.Equals(FilaComparar))
                    {
                        return true;
                    }

                }
                return false;
            }

        }


        //Metodo que verifica que se cumpla la llave primaria en la tabla, ignorando la fila indicada
        public Boolean existePrimaryKey(String nombreTabla, List<Object> fila, int ignorarEn)
        {
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
                    filaEntrante += fila[indicesPConstraints[i]].ToString() + " ";
                }

                List<List<Object>> contenidoFilas = miContenido.listObj;
                for (int i = 0; i < contenidoFilas.Count; i++)
                {
                    if (i != ignorarEn)
                    {
                        List<Object> filaTemp = contenidoFilas[i];
                        String FilaComparar = "";
                        for (int j = 0; j < indicesPConstraints.Count; j++)
                        {
                            FilaComparar += filaTemp[indicesPConstraints[j]].ToString() + " ";
                        }
                        if (filaEntrante.Equals(FilaComparar))
                        {
                            return true;
                        }

                    }
                }
                return false;
            }

        }

        //Metodo que verifica que en las columnas dadas no se repitan elementos para determinar unicidad
        public Boolean unicidadEnTabla(String nombreTabla, List<String> columnasPKey)
        {
            String contenido;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;

            List<int> indicesPConstraints = new List<int>();
            List<String> tiposPConstraints = new List<String>();
            for (int i = 0; i < columnasPKey.Count; i++)
            {
                int indice = tablasCreadas.indiceColumna(nombreTabla, columnasPKey[i]);
                String tipo = tablasCreadas.tipoColumna(nombreTabla, columnasPKey[i]);
                indicesPConstraints.Add(indice);
                tiposPConstraints.Add(tipo);

            }
            String filaPrincipal = "";

            for (int i = 0; i < miContenido.listObj.Count; i++)
            {
                filaPrincipal = "";
                //armar linea a comparar
                for (int j = 0; j < indicesPConstraints.Count; j++)
                {
                    filaPrincipal += columnasPKey[indicesPConstraints[i]].ToString() + " ";
                }

                //recorrer lineas posteriores
                for (int j = i+1; j < miContenido.listObj.Count; j++)
                {

                    List<Object> filaTemp = miContenido.listObj[j];
                    String FilaComparar = "";
                    for (int k = 0; k < indicesPConstraints.Count; k++)
                    {
                        FilaComparar += filaTemp[indicesPConstraints[k]].ToString() + " ";
                    }
                    if (filaPrincipal.Equals(FilaComparar))
                    {
                        return false;
                    }

                }

            }

            return false;

        }

        //Metodo que verifica la existencia de una llave foranea en la fila.
        public Boolean existeForeignKey(String nombreTabla, List<Object> fila)
        {
            List<ForeignConstraint> tempFConstraint = tablasCreadas.obtenerTabla(nombreTabla).fConstraint;
            if (tempFConstraint.Count == 0)
            {
                return true;
            }
            else
            {
                Boolean bandera = true;
                for (int k = 0; k < tempFConstraint.Count; k++)
                {
                    if (bandera)
                    {
                        bandera = false;
                    }
                    else
                    {
                        return false;
                    }
                    List<String> columnasRefConstraint = tempFConstraint[k].refCol;
                    List<String> columnasConstraint = tempFConstraint[k].idCol;
                    String nombreTablaConstraint = tempFConstraint[k].getTablaRefNombre();
                    List<int> indicesKConstraints = new List<int>();
                    List<int> indicesTablaC = new List<int>();
                    List<String> tiposKConstraints = new List<String>();
                    List<String> tiposTablaC = new List<String>();
                    for (int i = 0; i < columnasRefConstraint.Count; i++)
                    {
                        int indice = tablasCreadas.indiceColumna(nombreTablaConstraint, columnasRefConstraint[i]);
                        String tipo = tablasCreadas.tipoColumna(nombreTablaConstraint, columnasRefConstraint[i]);
                        indicesKConstraints.Add(indice);
                        tiposKConstraints.Add(tipo);
                        indice = tablasCreadas.indiceColumna(nombreTabla, columnasConstraint[i]);
                        tipo = tablasCreadas.tipoColumna(nombreTabla, columnasConstraint[i]);
                        indicesTablaC.Add(indice);
                        tiposTablaC.Add(tipo);

                    }
                    String filaEntrante = "";
                    //Se podria ver que coincidan los tipos en caso se haya hecho referencia de una tabla int a una float
                    //
                    //
                    for (int i = 0; i < indicesTablaC.Count; i++)
                    {
                        filaEntrante += fila[indicesTablaC[i]].ToString() + " ";
                    }
                    ControlContenido contenidoTemp = new ControlContenido();
                    String contenido;
                    try
                    {
                        contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTablaConstraint + ".dat");
                        contenidoTemp = DeSerializarContenido(contenido);
                    }
                    catch (Exception e)
                    {

                    }

                    List<List<Object>> contenidoFilas = contenidoTemp.listObj;
                    for (int i = 0; i < contenidoFilas.Count; i++)
                    {
                        List<Object> filaTemp = contenidoFilas[i];
                        String FilaComparar = "";
                        for (int j = 0; j < indicesKConstraints.Count; j++)
                        {
                            FilaComparar += filaTemp[indicesTablaC[j]].ToString() + " ";
                        }
                        if (filaEntrante.Equals(FilaComparar))
                        {
                            bandera = true;
                        }

                    }
                }
                
                return bandera;
            }

        }
        //Metodo que verifica si se puede eliminar una fila de la tabla especificada.
        public Boolean permitirEliminacion(String nombreTabla, List<Object> fila)
        {
            List<String> nombresTablasRevisar = new List<String>();
            List<Tabla> tablasRevisar = new List<Tabla>();
            String tempNombre = "";
            String tempNombre2 = "";
            Boolean esForaneo = false;
            for (int i = 0; i < tablasCreadas.listaTB.Count; i++)
            {
                tempNombre2 = tablasCreadas.listaTB[i].getNombre();
                for (int j = 0; j < tablasCreadas.listaTB[i].fConstraint.Count; j++)
                {   
                    tempNombre = tablasCreadas.listaTB[i].fConstraint[j].nombreTabla;
                    
                    if (!nombresTablasRevisar.Contains(tempNombre2)&&tempNombre.Equals(nombreTabla))
                    {
                        nombresTablasRevisar.Add(tempNombre2);
                        esForaneo = esForeignKey(nombreTabla, tempNombre2, fila);
                        if (esForaneo)
                        {
                            mensajeFallo = "No se puede eliminar el elemento debido a que es referenciado por la tabla " + tempNombre2 + ".\n";
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        //Metodo que verifica si se puede hacer update a una fila de una tabla.
        public Boolean permitirUpdate(String nombreTabla, List<Object> fila, List<String> columnasCambiar)
        {
            List<String> nombresTablasRevisar = new List<String>();
            List<Tabla> tablasRevisar = new List<Tabla>();
            String tempNombre = "";
            String tempNombre2 = "";
            Boolean esForaneo = false;
            for (int i = 0; i < tablasCreadas.listaTB.Count; i++)
            {
                tempNombre2 = tablasCreadas.listaTB[i].getNombre();
                for (int j = 0; j < tablasCreadas.listaTB[i].fConstraint.Count; j++)
                {
                    tempNombre = tablasCreadas.listaTB[i].fConstraint[j].nombreTabla;

                    if (!nombresTablasRevisar.Contains(tempNombre2) && tempNombre.Equals(nombreTabla))
                    {
                        nombresTablasRevisar.Add(tempNombre2);
                        esForaneo = esForeignKey(nombreTabla, tempNombre2, fila, columnasCambiar);
                        if (esForaneo)
                        {
                            mensajeFallo = "No se puede eliminar el elemento debido a que es referenciado por la tabla " + tempNombre2 + ".\n";
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        //Metodo que verifica que el foreing key de una fila cumpla para las tablas especificadas
        public Boolean esForeignKey(String nombreTabla1, String nombreTabla2, List<Object> fila)
        {
            List<ForeignConstraint> tempFConstraint = tablasCreadas.obtenerTabla(nombreTabla2).fConstraint;
            for (int k = 0; k < tempFConstraint.Count; k++)
            {
                if (tempFConstraint[k].getTablaRefNombre().Equals(nombreTabla1))
                {
                    List<String> columnasRefConstraint = tempFConstraint[k].refCol;
                    List<String> columnasConstraint = tempFConstraint[k].idCol;
                    //String nombreTablaConstraint = tempFConstraint[k].getTablaRefNombre();
                    List<int> indicesKConstraints = new List<int>();
                    List<int> indicesTablaC = new List<int>();
                    List<String> tiposKConstraints = new List<String>();
                    List<String> tiposTablaC = new List<String>();
                    for (int i = 0; i < columnasRefConstraint.Count; i++)
                    {
                        int indice = tablasCreadas.indiceColumna(nombreTabla1, columnasRefConstraint[i]);
                        String tipo = tablasCreadas.tipoColumna(nombreTabla1, columnasRefConstraint[i]);
                        indicesKConstraints.Add(indice);
                        tiposKConstraints.Add(tipo);
                        indice = tablasCreadas.indiceColumna(nombreTabla2, columnasConstraint[i]);
                        tipo = tablasCreadas.tipoColumna(nombreTabla2, columnasConstraint[i]);
                        indicesTablaC.Add(indice);
                        tiposTablaC.Add(tipo);

                    }
                    String filaEntrante = "";
                    //Se podria ver que coincidan los tipos en caso se haya hecho referencia de una tabla int a una float
                    //
                    //
                    for (int i = 0; i < indicesKConstraints.Count; i++)
                    {
                        filaEntrante += fila[indicesKConstraints[i]].ToString() + " ";
                    }
                    ControlContenido contenidoTemp = new ControlContenido();
                    String contenido;
                    try
                    {
                        contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla2 + ".dat");
                        contenidoTemp = DeSerializarContenido(contenido);
                    }
                    catch (Exception e)
                    {

                    }

                    List<List<Object>> contenidoFilas = contenidoTemp.listObj;
                    for (int i = 0; i < contenidoFilas.Count; i++)
                    {
                        List<Object> filaTemp = contenidoFilas[i];
                        String FilaComparar = "";
                        for (int j = 0; j < indicesTablaC.Count; j++)
                        {
                            FilaComparar += filaTemp[indicesTablaC[j]].ToString() + " ";
                        }
                        if (filaEntrante.Equals(FilaComparar))
                        {
                            return true;
                        }

                    }
                }
                else
                {

                }
                
            }

            return false;
            
        }
        //Metodo que verifica si se puede updatear una fila cumpliendo la foreing key en ambas tablas.
        public Boolean esForeignKey(String nombreTabla1, String nombreTabla2, List<Object> fila, List<String> updateColumnas)
        {
            List<ForeignConstraint> tempFConstraint = tablasCreadas.obtenerTabla(nombreTabla2).fConstraint;
            for (int k = 0; k < tempFConstraint.Count; k++)
            {
                if (tempFConstraint[k].getTablaRefNombre().Equals(nombreTabla1))
                {
                    List<String> columnasRefConstraint = tempFConstraint[k].refCol;
                    List<String> columnasConstraint = tempFConstraint[k].idCol;
                    //String nombreTablaConstraint = tempFConstraint[k].getTablaRefNombre();
                    List<int> indicesKConstraints = new List<int>();
                    List<int> indicesTablaC = new List<int>();
                    List<String> tiposKConstraints = new List<String>();
                    List<String> tiposTablaC = new List<String>();
                    for (int i = 0; i < columnasRefConstraint.Count; i++)
                    {
                        if (updateColumnas.Contains(columnasRefConstraint[i]))
                        {
                            int indice = tablasCreadas.indiceColumna(nombreTabla1, columnasRefConstraint[i]);
                            String tipo = tablasCreadas.tipoColumna(nombreTabla1, columnasRefConstraint[i]);
                            indicesKConstraints.Add(indice);
                            tiposKConstraints.Add(tipo);
                            indice = tablasCreadas.indiceColumna(nombreTabla2, columnasConstraint[i]);
                            tipo = tablasCreadas.tipoColumna(nombreTabla2, columnasConstraint[i]);
                            indicesTablaC.Add(indice);
                            tiposTablaC.Add(tipo);
                        }
                    }
                    String filaEntrante = "";
                    //Se podria ver que coincidan los tipos en caso se haya hecho referencia de una tabla int a una float
                    //
                    //
                    if (indicesKConstraints.Count == 0)
                    {
                        return false;
                    }
                    for (int i = 0; i < indicesKConstraints.Count; i++)
                    {
                        filaEntrante += fila[indicesKConstraints[i]].ToString() + " ";
                    }
                    ControlContenido contenidoTemp = new ControlContenido();
                    String contenido;
                    try
                    {
                        contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla2 + ".dat");
                        contenidoTemp = DeSerializarContenido(contenido);
                    }
                    catch (Exception e)
                    {

                    }

                    List<List<Object>> contenidoFilas = contenidoTemp.listObj;
                    for (int i = 0; i < contenidoFilas.Count; i++)
                    {
                        List<Object> filaTemp = contenidoFilas[i];
                        String FilaComparar = "";
                        for (int j = 0; j < indicesTablaC.Count; j++)
                        {
                            FilaComparar += filaTemp[indicesTablaC[j]].ToString() + " ";
                        }
                        if (filaEntrante.Equals(FilaComparar))
                        {
                            return true;
                        }

                    }
                }
                else
                {

                }

            }

            return false;

        }
        //Metodo que verifica que la primary key no sea null.
        public Boolean primaryNull(String nombreTabla, List<Object> fila)
        {
            String contenido;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla+ ".dat", contenido);
                }
                catch (Exception e)
                {
                    
                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
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
        //Metodo que updatea la informacion de una columna cuando se utiliza una condicion.
        public int UpdateColumnas(List<Object> elementosIngreso, String nombreTabla, List<String> condicionWhere)
        {
            String contenido;
            int columnasUpdate = 0;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
            List<int> indicesACambiar = new List<int>();
            //Guardar los indices a cambiar
            for (int i = 0; i < elementosIngreso.Count; i++)
            {
                if (elementosIngreso[i] != null)
                {
                    indicesACambiar.Add(i);
                }
            }
            List<String> columnasAUpdatear = new List<String>();
            for(int i=0;i<indicesACambiar.Count;i++){
                columnasAUpdatear.Add(tablasCreadas.obtenerTabla(nombreTabla).columnasTB[indicesACambiar[i]].getNombre());
            }
            //cambia elementos de la lista
            Boolean cumpleFKey = false;
            Boolean noesFKey = false;
            Boolean nocumplePKey = false;
            falloUpdate = false;
            mensajeFallo2 = "";
            List<List<Object>> contenidoFilas = miContenido.listObj;
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = new List<Object>();
                for (int j = 0; j < contenidoFilas[i].Count; j++)
                {
                    filaTemp.Add(contenidoFilas[i][j]);
                }
                Boolean cumple = cumpleConstraint(filaTemp, condicionWhere, nombreTabla);
                if (cumple)
                {
                    noesFKey = permitirUpdate(nombreTabla, filaTemp, columnasAUpdatear);
                    if (!noesFKey)
                    {
                        falloUpdate = true;
                        deUpdate = false;
                        mensajeFallo2 = "No se puede continuar con el update ya que un elemento de la fila es referenciado por una foreign key. \n";
                        i = contenidoFilas.Count;
                    }
                    else
                    {
                        for (int j = 0; j < indicesACambiar.Count; j++)
                        {
                            filaTemp[indicesACambiar[j]] = elementosIngreso[indicesACambiar[j]];
                        }

                        //verificacion linea
                        deUpdate = true;
                        nocumplePKey = existePrimaryKey(nombreTabla, filaTemp, i);
                        if (nocumplePKey)
                        {
                            falloUpdate = true;
                            deUpdate = false;
                            mensajeFallo2 = "No se puede continuar con el update ya que se viola la llave primaria. \n";
                            i = contenidoFilas.Count;

                        }
                        else
                        {
                            cumpleFKey = existeForeignKey(nombreTabla, filaTemp);
                            if (!cumpleFKey)
                            {
                                falloUpdate = true;
                                deUpdate = false;
                                mensajeFallo2 = "No se puede continuar con el update ya que se viola la llave foranea. \n";
                                i = contenidoFilas.Count;
                            }
                            else
                            {
                                contenidoFilas[i] = filaTemp;
                                miContenido.setListObj(contenidoFilas);
                                columnasUpdate = columnasUpdate + 1;
                            }
                        }
                    }
                }
            }
            //se guarda matriz
            miContenido.setListObj(contenidoFilas);
            return columnasUpdate;

            
        }
        //Metodo que updatea la informacion de las columnas cuando no viene una condicion.
        public int UpdateColumnas(List<Object> elementosIngreso, String nombreTabla)
        {
            int columnasUpdate = 0;
            String contenido;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
            List<int> indicesACambiar = new List<int>();
            //Guardar los indices a cambiar
            for (int i = 0; i < elementosIngreso.Count; i++)
            {
                if (elementosIngreso[i] != null)
                {
                    indicesACambiar.Add(i);
                }
            }
            List<String> columnasAUpdatear = new List<String>();
            for (int i = 0; i < indicesACambiar.Count; i++)
            {
                columnasAUpdatear.Add(tablasCreadas.obtenerTabla(nombreTabla).columnasTB[indicesACambiar[i]].getNombre());
            }
            //cambia elementos de la lista
            Boolean cumpleFKey = false;
            Boolean noesFKey = false;
            Boolean nocumplePKey = false;
            falloUpdate = false;
            mensajeFallo2 = "";
            List<List<Object>> contenidoFilas = miContenido.listObj;
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = new List<Object>();
                for (int j = 0; j < contenidoFilas[i].Count; j++)
                {
                    filaTemp.Add(contenidoFilas[i][j]);
                }
                noesFKey = permitirUpdate(nombreTabla, filaTemp, columnasAUpdatear);
                if (!noesFKey)
                {
                    falloUpdate = true;
                    deUpdate = false;
                    mensajeFallo2 = "No se puede continuar con el update ya que un elemento de la fila es referenciado por una foreign key. \n";
                    i = contenidoFilas.Count;
                }
                else
                {
                    for (int j = 0; j < indicesACambiar.Count; j++)
                    {
                        filaTemp[indicesACambiar[j]] = elementosIngreso[indicesACambiar[j]];
                    }
                    //verificacion linea
                    deUpdate = true;
                    nocumplePKey = existePrimaryKey(nombreTabla, filaTemp, i);
                    if (nocumplePKey)
                    {
                        falloUpdate = true;
                        deUpdate = false;
                        mensajeFallo2 = "No se puede continuar con el update ya que se viola la llave primaria. \n";
                        i = contenidoFilas.Count;

                    }
                    else
                    {
                        cumpleFKey = existeForeignKey(nombreTabla, filaTemp);
                        if (!cumpleFKey)
                        {
                            falloUpdate = true;
                            deUpdate = false;
                            mensajeFallo2 = "No se puede continuar con el update ya que se viola la llave foranea. \n";
                            i = contenidoFilas.Count;
                        }
                        else
                        {

                            contenidoFilas[i] = filaTemp;
                            miContenido.setListObj(contenidoFilas);
                            columnasUpdate = columnasUpdate + 1;
                        }
                    }
                }
            }
            //se guarda matriz
            miContenido.setListObj(contenidoFilas);
            return columnasUpdate;
        }
        //Metodo que elimina filas de una tabla cuando cumplen la condicion.
        public int DeleteFilas(String nombreTabla, List<String> condicionWhere)
        {
            String contenido;
            int columnasDelete = 0;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
            //eliminar elementos de la lista
            mensajeFallo = "";
            falloEliminacion = false;
            int indiceFallo = 0;
            List<List<Object>> contenidoFilas = miContenido.listObj;
            List<List<Object>> nuevoContenido = new List<List<Object>>();
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = contenidoFilas[i];
                Boolean cumple = cumpleConstraint(filaTemp, condicionWhere, nombreTabla);
                
                if (cumple)
                {
                    Boolean hayReferencia = permitirEliminacion(nombreTabla, filaTemp);
                    if (hayReferencia)
                    {
                        columnasDelete = columnasDelete + 1;
                    }
                    else
                    {
                        falloEliminacion = true;
                        indiceFallo = i;
                        i = contenidoFilas.Count;
                    }
                    
                }
                else
                {
                    nuevoContenido.Add(contenidoFilas[i]);
                }
            }
            if (falloEliminacion)
            {
                for (int i = indiceFallo; i < contenidoFilas.Count; i++)
                {
                    nuevoContenido.Add(contenidoFilas[i]);
                }
            }


            //se guarda matriz
            miContenido.setListObj(nuevoContenido);
            tablasCreadas.removerRegistro(nombreTabla, columnasDelete);
            return columnasDelete;


        }
        //Metodo que elimina todas las filas de una tabla.
        public int DeleteFilas(String nombreTabla)
        {
            int columnasDelete = 0;
            String contenido;
            if (!(contenidoBase.Equals(DBactual) && contenidoTabla.Equals(nombreTabla)))
            {
                try
                {
                    contenido = SerializarContenido(miContenido);
                    File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
                }
                catch (Exception e)
                {

                }
                try
                {
                    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
                    miContenido = DeSerializarContenido(contenido);
                }
                catch (Exception e)
                {

                }
            }
            contenidoTabla = nombreTabla;
            //elimina elementos de la lista
            List<List<Object>> contenidoFilas = miContenido.listObj;
            List<List<Object>> nuevoContenido = new List<List<Object>>();

            mensajeFallo = "";
            //eliminar elementos de la lista
            falloEliminacion = false;
            int indiceProblema = 0;
            for (int i = 0; i < contenidoFilas.Count; i++)
            {
                List<Object> filaTemp = contenidoFilas[i];
                
                Boolean hayReferencia = permitirEliminacion(nombreTabla, filaTemp);
                if (hayReferencia)
                {
                    columnasDelete = columnasDelete + 1;
                }
                else
                {
                    falloEliminacion = true;
                    indiceProblema = i;
                    i = contenidoFilas.Count;
                }
                    
            }
            if (falloEliminacion)
            {
                for (int i = indiceProblema; i < contenidoFilas.Count; i++)
                {
                    nuevoContenido.Add(contenidoFilas[i]);
                }
            }
            //se guarda matriz
            miContenido.setListObj(nuevoContenido);
            tablasCreadas.removerRegistro(nombreTabla, columnasDelete);
            return columnasDelete;
        }

        //Obtener lista con los nombres de las columnas

        //Metodo que muestra todo de las tablas indicadas
        public List<List<Object>> SelectFilas(List<String> nombreTablas, Boolean auxiliar)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";
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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
                            }
                            productoCartesiano.Add(fila1);
                                
                        }
                    }
                }
            }
            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            tituloColumnas = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if(nombreTablas[k].Equals(tablaTemp.getNombre())){
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());
                        }
                    }
                }
            }


            return productoCartesiano;

        }
        
        //Metodo que muestra lo seleccionado de las tablas indicadas
        public List<List<Object>> SelectFilas(List<String> nombreTablas,  List<String>columnasSeleccionadas)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            tituloColumnas = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());
                        }
                    }
                    
                }
            }

            //Llamar metodo para seleccionar columnas
            return obtenerSelect(productoCartesiano, columnasSeleccionadas, nombresColumna, TablaColumna);

        }

        //Metodo que muestra todo de las tablas indicadas cuando se cumple la condicion
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> expWhere, Boolean auxiliar)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            tituloColumnas = new List<String>();

            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());

                        }
                    }
                }
            }

            //Reducir la tabla incluyendo solo donde se cumple el where

            List<List<Object>> cumplenWhere = new List<List<Object>>();
            Boolean cumple = false;
            for (int i = 0; i < productoCartesiano.Count; i++)
            {
                //cumple = (productoCartesiano[i], expWhere, nombreColumna, tipoColum, TablaColumna) metodo para determinar si cumple
                cumple = verificarWhere(productoCartesiano[i], expWhere, nombresColumna, tipoColum, TablaColumna);
                if (cumple)
                {
                    cumplenWhere.Add(productoCartesiano[i]);
                }
            }

            return cumplenWhere;
        }

        //Metodo que muestra lo seleccionado de las tablas indicadas cuando se cumple la condicion
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String>columnasSeleccionadas, List<String> expWhere)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            tituloColumnas = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());

                        }
                    }
                }
            }

            //Reducir la tabla incluyendo solo donde se cumple el where

            List<List<Object>> cumplenWhere = new List<List<Object>>();
            Boolean cumple = false;
            for (int i = 0; i < productoCartesiano.Count; i++)
            {
                //cumple = (productoCartesiano[i], expWhere, nombreColumna, tipoColum, TablaColumna) metodo para determinar si cumple
                cumple = verificarWhere(productoCartesiano[i], expWhere, nombresColumna, tipoColum, TablaColumna);
                if (cumple)
                {
                    cumplenWhere.Add(productoCartesiano[i]);
                }
            }
            return obtenerSelect(cumplenWhere, columnasSeleccionadas, nombresColumna, TablaColumna);
        }

        //Metodo que muestra todo de las tablas indicadas cuando se cumple where en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> expWhere, List<String> columnasOrden, List<String> tipoOrden, Boolean auxiliar)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            List<String> tituloColumna = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());

                        }
                    }
                }
            }
            //Reducir la tabla incluyendo solo donde se cumple el where

            List<List<Object>> cumplenWhere = new List<List<Object>>();
            Boolean cumple = false;
            for (int i = 0; i < productoCartesiano.Count; i++)
            {
                //cumple = (productoCartesiano[i], expWhere, nombreColumna, tipoColum, TablaColumna) metodo para determinar si cumple
                cumple = verificarWhere(productoCartesiano[i], expWhere, nombresColumna, tipoColum, TablaColumna);
                if (cumple)
                {
                    cumplenWhere.Add(productoCartesiano[i]);
                }
            }


            //generar el orden 
            return ordenarContenido(cumplenWhere, columnasOrden, tipoOrden, nombresColumna, tipoColum, TablaColumna);

        }

        //Metodo que muestra lo indicado de las tablas indicadas cuando se cumple la condicion en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasSeleccionadas, List<String> expWhere, List<String> columnasOrden, List<String> tipoOrden)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            List<String> tituloColumna = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());
                        }
                    }
                }
            }

            //Reducir la tabla incluyendo solo donde se cumple el where

            List<List<Object>> cumplenWhere = new List<List<Object>>();
            Boolean cumple = false;
            for (int i = 0; i < productoCartesiano.Count; i++)
            {
                //cumple = (productoCartesiano[i], expWhere, nombreColumna, tipoColum, TablaColumna) metodo para determinar si cumple
                cumple = verificarWhere(productoCartesiano[i], expWhere, nombresColumna, tipoColum, TablaColumna);
                if (cumple)
                {
                    cumplenWhere.Add(productoCartesiano[i]);
                }
            }


            //generar el orden 
            List<List<Object>> ordenado = ordenarContenido(cumplenWhere, columnasOrden, tipoOrden, nombresColumna, tipoColum, TablaColumna);
            
            //Seleccionar columnas
            return obtenerSelect(ordenado, columnasSeleccionadas, nombresColumna, TablaColumna);
        }

        //Metodo que muestra todo de las tablas indicadas en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasOrden, List<String> tipoOrden, Boolean auxiliar)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            List<String> tituloColum = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());
                        }
                    }
                }
            }



            //generar el orden 
            return ordenarContenido(productoCartesiano, columnasOrden, tipoOrden, nombresColumna, tipoColum, TablaColumna);

        }

        //Metodo que muestra lo indicado de las tablas indicadas  en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasSeleccionadas, List<String> columnasOrden, List<String> tipoOrden)
        {
            String contenido;
            List<ControlContenido> coleccionContenido = new List<ControlContenido>();
            List<List<Object>> productoCartesiano = new List<List<Object>>();

            try
            {
                contenido = SerializarContenido(miContenido);
                File.WriteAllText("DataDB\\" + contenidoBase + "\\" + contenidoTabla + ".dat", contenido);
            }
            catch (Exception e)
            {

            }
            contenidoTabla = "";

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
                    conjuntoTemp = new List<List<Object>>();
                    for (int j = 0; j < productoCartesiano.Count; j++)
                    {
                        conjuntoTemp.Add(productoCartesiano[j]);
                    }
                    productoCartesiano = new List<List<Object>>();
                    for (int j = 0; j < conjuntoTemp.Count; j++)
                    {
                        List<List<Object>> contenidoTabla2 = coleccionContenido[i].listObj;
                        for (int k = 0; k < contenidoTabla2.Count; k++)
                        {
                            List<Object> fila1 = new List<Object>();
                            for (int l = 0; l < conjuntoTemp[j].Count; l++)
                            {
                                fila1.Add(conjuntoTemp[j][l]);
                            }
                            List<Object> fila2 = contenidoTabla2[k];
                            for (int l = 0; l < fila2.Count; l++)
                            {
                                fila1.Add(fila2[l]);
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


            //Realizar listas para conocer nombre y tabla
            List<String> nombresColumna = new List<String>();
            List<String> TablaColumna = new List<String>();
            List<String> tipoColum = new List<string>();
            List<String> tituloColumna = new List<String>();
            for (int k = 0; k < nombreTablas.Count; k++)
            {
                for (int i = 0; i < tablasCreadas.getListaTB().Count; i++)
                {
                    Tabla tablaTemp = tablasCreadas.getListaTB()[i];
                    if (nombreTablas[k].Equals(tablaTemp.getNombre()))
                    {
                        for (int j = 0; j < tablaTemp.columnasTB.Count; j++)
                        {
                            nombresColumna.Add(tablaTemp.columnasTB[j].getNombre());
                            tipoColum.Add(tablaTemp.columnasTB[j].getTipo());
                            TablaColumna.Add(tablaTemp.getNombre());
                            tituloColumnas.Add(tablaTemp.getNombre() + "." + tablaTemp.columnasTB[j].getNombre());
                        }
                    }
                }
            }


            //generar el orden 
            List<List<Object>> ordenado = ordenarContenido(productoCartesiano, columnasOrden, tipoOrden, nombresColumna, tipoColum, TablaColumna);

            //Seleccionar columnas
            return obtenerSelect(ordenado, columnasSeleccionadas, nombresColumna, TablaColumna);
        }
        //Metodo que ordena el contenido de una seleccion.
        public List<List<Object>> ordenarContenido(List<List<Object>> tablaCompleta, List<String> columnasOrdenar, List<String> tipoOrdenamiento, List<String> nombresCol, List<String> tipoCol, List<String> TablaCol)
        {
            List<List<Object>> tablaOrdenada = new List<List<Object>>();
            List<List<Object>> conjuntoTemp= new List<List<Object>>();
            tablaOrdenada = tablaCompleta;
            int indiceColumna = 0;
            String tipoC = "";
            for (int i = columnasOrdenar.Count-1; i >= 0; i--)
            {
                //obtener indice
                if(columnasOrdenar[i].Contains(".")){
                    String nombreTabla = columnasOrdenar[i].Substring(0,columnasOrdenar[i].IndexOf("."));
                    String nombreColumna2 = columnasOrdenar[i].Substring(columnasOrdenar[i].IndexOf(".") + 1, columnasOrdenar[i].Length - (columnasOrdenar[i].IndexOf(".")+1));
                    for(int j = 0; j < nombresCol.Count; j++){
                        if(nombreColumna2.Equals(nombresCol[j])&&nombreTabla.Equals(TablaCol[j])){
                            indiceColumna = j;
                            tipoC = tipoCol[j];
                        }
                    }
                }
                else{
                    for(int j = 0; j < nombresCol.Count; j++){
                        if(columnasOrdenar[i].Equals(nombresCol[j])){
                            indiceColumna = j;
                            tipoC = tipoCol[j];
                        }
                    }
                }

                conjuntoTemp = new List<List<Object>>();
                for (int j = 0; j < tablaOrdenada.Count; j++)
                {
                    conjuntoTemp.Add(tablaOrdenada[j]);
                }
                tablaOrdenada = new List<List<Object>>();
                for (int j = 0; j < conjuntoTemp.Count; j++)
                {
                    if (tablaOrdenada.Count == 0)
                    {
                        tablaOrdenada.Add(conjuntoTemp[j]);
                    }
                    else
                    {
                        if(tipoC.Equals("int")){
                            if(tipoOrdenamiento[i].Equals("asc")){
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    tablaOrdenada.Add(conjuntoTemp[j]);
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]==null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                        else{
                                            int num1 = (Int32)tablaOrdenada[k][indiceColumna];
                                            int num2 = (Int32)conjuntoTemp[j][indiceColumna];
                                            if(num1>num2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                    }
                                }
                            }
                            else{
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            int num1 = (Int32)(tablaOrdenada[k][indiceColumna]);
                                            int num2 = (Int32)(conjuntoTemp[j][indiceColumna]);
                                            if(num1<num2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                        else if (k == tablaOrdenada.Count - 1)
                                        {
                                            tablaOrdenada.Add(conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }

                            }
                            
                        }
                        else if(tipoC.Equals("float")){
                            if(tipoOrdenamiento[i].Equals("asc")){
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    tablaOrdenada.Add(conjuntoTemp[j]);
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]==null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                        else{
                                            float num1 = (Single)(tablaOrdenada[k][indiceColumna]);
                                            float num2 = (Single)(conjuntoTemp[j][indiceColumna]);
                                            if(num1>num2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                    }
                                }
                            }
                            else{
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            float num1 = (Single)(tablaOrdenada[k][indiceColumna]);
                                            float num2 = (Single)(conjuntoTemp[j][indiceColumna]);
                                            if(num1<num2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                        else if (k == tablaOrdenada.Count - 1)
                                        {
                                            tablaOrdenada.Add(conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }

                            }
                            
                        }
                        else if(tipoC.Equals("date")){
                            if(tipoOrdenamiento[i].Equals("asc")){
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    tablaOrdenada.Add(conjuntoTemp[j]);
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]==null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                        else{
                                            DateTime fecha1 = (DateTime)(tablaOrdenada[k][indiceColumna]);
                                            DateTime fecha2 = (DateTime)(conjuntoTemp[j][indiceColumna]);
                                            if(fecha1>fecha2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                    }
                                }
                            }
                            else{
                                if(conjuntoTemp[j][indiceColumna]==null){
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }
                                else{
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if(tablaOrdenada[k][indiceColumna]!=null)
                                        {
                                            DateTime fecha1 = (DateTime)(tablaOrdenada[k][indiceColumna]);
                                            DateTime fecha2 = (DateTime)(conjuntoTemp[j][indiceColumna]);
                                            if(fecha1<fecha2)
                                            {
                                                tablaOrdenada.Insert(k,conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                        else if (k == tablaOrdenada.Count - 1)
                                        {
                                            tablaOrdenada.Add(conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }

                            }
                            
                        }
                        else
                        {
                            if (tipoOrdenamiento[i].Equals("asc"))
                            {
                                if (conjuntoTemp[j][indiceColumna] == null)
                                {
                                    tablaOrdenada.Add(conjuntoTemp[j]);
                                }
                                else
                                {
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if (tablaOrdenada[k][indiceColumna] == null)
                                        {
                                            tablaOrdenada.Insert(k, conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                        else
                                        {
                                            String varchar1 = ((String)tablaOrdenada[k][indiceColumna]).Replace("'", "");
                                            String varchar2 = ((String)conjuntoTemp[j][indiceColumna]).Replace("'", "");
                                            if (String.Compare(varchar1, varchar2) > 0)
                                            {
                                                tablaOrdenada.Insert(k, conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (conjuntoTemp[j][indiceColumna] == null)
                                {
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if (tablaOrdenada[k][indiceColumna] != null)
                                        {
                                            tablaOrdenada.Insert(k, conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int k = 0; k < tablaOrdenada.Count; k++)
                                    {
                                        if (tablaOrdenada[k][indiceColumna] != null)
                                        {
                                            String varchar1 = ((String)tablaOrdenada[k][indiceColumna]).Replace("'", "");
                                            String varchar2 = ((String)conjuntoTemp[j][indiceColumna]).Replace("'", "");
                                            if (String.Compare(varchar1, varchar2) < 0)
                                            {
                                                tablaOrdenada.Insert(k, conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                            else if (k == tablaOrdenada.Count - 1)
                                            {
                                                tablaOrdenada.Add(conjuntoTemp[j]);
                                                k = tablaOrdenada.Count;
                                            }
                                        }
                                        else if (k == tablaOrdenada.Count - 1)
                                        {
                                            tablaOrdenada.Add(conjuntoTemp[j]);
                                            k = tablaOrdenada.Count;
                                        }
                                    }
                                }

                            }

                        }
                        
                    }
                }
            }

            return tablaOrdenada;
        }
        //Metodo que obtiene lo especificado en el select.
        public List<List<Object>> obtenerSelect(List<List<Object>> tablaCompleta, List<String> columnasMostrar, List<String> nombreColumnas, List<String> tablaColumnas)
        {
            //Obtener indices
            tituloColumnas = new List<String>();
            List<int> indicesMostrar = new List<int>();
            for (int i = 0; i < columnasMostrar.Count; i++)
            {
                if (columnasMostrar[i].Contains("."))
                {
                    String nombreTabla = columnasMostrar[i].Substring(0, columnasMostrar[i].IndexOf("."));
                    String nombreColumna2 = columnasMostrar[i].Substring(columnasMostrar[i].IndexOf(".") + 1, columnasMostrar[i].Length - (columnasMostrar[i].IndexOf(".")+1));

                    for (int j = 0; j < nombreColumnas.Count; j++)
                    {
                        if (nombreColumna2.Equals(nombreColumnas[j])&&nombreTabla.Equals(tablaColumnas[j]))
                        {
                            indicesMostrar.Add(j);
                            tituloColumnas.Add(tablaColumnas[j] + "." + nombreColumnas[j]);
                        }
                    }
                }
                else
                {
                    String nombreColumna2 = columnasMostrar[i];
                    for (int j = 0; j < nombreColumnas.Count; j++)
                    {
                        if (nombreColumna2.Equals(nombreColumnas[j]))
                        {
                            indicesMostrar.Add(j);
                            tituloColumnas.Add(tablaColumnas[j] + "." + nombreColumnas[j]);
                        }
                    }
                }
            }

            //Generar nuevas filas con lo seleccionado
            List<List<Object>> tablaFinal = new List<List<Object>>();
            for (int i = 0; i < tablaCompleta.Count; i++)
            {
                List<Object> listaTemp = new List<Object>();
                for (int j = 0; j < indicesMostrar.Count; j++)
                {
                    listaTemp.Add(tablaCompleta[i][indicesMostrar[j]]);

                }
                tablaFinal.Add(listaTemp);
            }

            return tablaFinal;

        }

        //Metodo que verifica que se cumplan las condiciones del where.
        public Boolean verificarWhere(List<Object> elementosFila, List<String> expresionWhere, List<String> nombresCol, List<String> tipoCol, List<String>TablaCol){
            Stack<Boolean> stack = new Stack<Boolean>();
            Object elemento1 = null;
            Object elemento2 = null;
            String tipoElemento1 = "";
            String tipoElemento2 = "";
            Boolean op1 = false;
            Boolean op2 = false;
            Boolean elem1 = true;
            Boolean esfloat = false;
            float numeroF;
            for (int i = 0; i < expresionWhere.Count; i++)
            {
                if (expresionWhere[i].Contains("."))
                {
                    if(elem1){
                        esfloat = Single.TryParse(expresionWhere[i], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"),out numeroF);
                        if (esfloat)
                        {
                            elemento1 = numeroF;
                            tipoElemento1 = "float";
                            elem1 = false;
                        }
                        else
                        {
                            int indiceElemento = 0;
                            String nombreTabla = expresionWhere[i].Substring(0, expresionWhere[i].IndexOf("."));
                            String nombreColumna2 = expresionWhere[i].Substring(expresionWhere[i].IndexOf(".") + 1, expresionWhere[i].Length - (expresionWhere[i].IndexOf(".") + 1));
                            for (int j = 0; j < nombresCol.Count; j++)
                            {
                                if (nombreColumna2.Equals(nombresCol[j]) && nombreTabla.Equals(TablaCol[j]))
                                {
                                    indiceElemento = j;
                                }
                            }
                            elemento1 = elementosFila[indiceElemento];
                            if (elemento1 == null)
                            {
                                return false;
                            }
                            tipoElemento1 = tipoCol[indiceElemento];
                            elem1 = false;
                        }
                        
                    }
                    else{
                        esfloat = Single.TryParse(expresionWhere[i], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"), out numeroF);
                        if (esfloat)
                        {
                            elemento2 = numeroF;
                            tipoElemento2 = "float";
                            elem1 = true;
                        }
                        else
                        {
                            int indiceElemento = 0;
                            String nombreTabla = expresionWhere[i].Substring(0, expresionWhere[i].IndexOf("."));
                            String nombreColumna2 = expresionWhere[i].Substring(expresionWhere[i].IndexOf(".") + 1, expresionWhere[i].Length - (expresionWhere[i].IndexOf(".") + 1));
                            for (int j = 0; j < nombresCol.Count; j++)
                            {
                                if (nombreColumna2.Equals(nombresCol[j]) && nombreTabla.Equals(TablaCol[j]))
                                {
                                    indiceElemento = j;
                                }
                            }
                            elemento2 = elementosFila[indiceElemento];
                            if (elemento2 == null)
                            {
                                return false;
                            }
                            tipoElemento2 = tipoCol[indiceElemento];
                            elem1 = true;
                        }
                    } 
                }
                else if (nombresCol.Contains(expresionWhere[i]))
                {
                    int indiceElemento = 0;
                    for (int j = 0; j < nombresCol.Count; j++)
                    {
                        if (expresionWhere[i].Equals(nombresCol[j]))
                        {
                            indiceElemento = j;
                        }
                    }
                    if (elem1)
                    {
                        elemento1 = elementosFila[indiceElemento];
                        if (elemento1 == null)
                        {
                            return false;
                        }
                        tipoElemento1 = tipoCol[indiceElemento];
                        elem1 = false;
                    }
                    else
                    {
                        elemento2 = elementosFila[indiceElemento];
                        if (elemento2 == null)
                        {
                            return false;
                        }
                        tipoElemento2 = tipoCol[indiceElemento];
                        elem1 = true;
                    }
                }
                else if (expresionWhere[i].Equals(">") || expresionWhere[i].Equals("<") || expresionWhere[i].Equals(">=") || expresionWhere[i].Equals("<="))
                {
                    if (expresionWhere[i].Equals(">"))
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
                        else if (tipoElemento1.Equals("float") && tipoElemento2.Equals("float"))
                        {
                            op1 = (Single)elemento1 > (Single)elemento2;
                        }
                        else if (tipoElemento1.Equals("date") && tipoElemento2.Equals("date"))
                        {
                            op1 = (DateTime)elemento1 > (DateTime)elemento2;
                        }
                        stack.Push(op1);
                    }
                    else if (expresionWhere[i].Equals("<"))
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
                    else if (expresionWhere[i].Equals("<="))
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
                    else if (expresionWhere[i].Equals(">="))
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
                else if (expresionWhere[i].Equals("=") || expresionWhere[i].Equals("<>"))
                {
                    if (expresionWhere[i].Equals("="))
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
                            op1 = ((((String)elemento1).Replace("'","")).Equals(((DateTime)elemento2).ToString()));
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Contains("char"))
                        {
                            op1 = ((String)elemento1).Equals((String)elemento2);
                        }
                        stack.Push(op1);
                    }
                    else if (expresionWhere[i].Equals("<>"))
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
                            op1 = !((((String)elemento1).Replace("'", "")).Equals(((DateTime)elemento2).ToString()));
                        }
                        else if (tipoElemento1.Contains("char") && tipoElemento2.Contains("char"))
                        {
                            op1 = !(((String)elemento1).Equals((String)elemento2));
                        }
                        stack.Push(op1);
                    }
                }
                else if (expresionWhere[i].ToLower().Equals("and"))
                {
                    op1 = stack.Pop();
                    op2 = stack.Pop();
                    stack.Push(op1 && op2);
                }
                else if (expresionWhere[i].ToLower().Equals("or"))
                {
                    op1 = stack.Pop();
                    op2 = stack.Pop();
                    stack.Push(op1 || op2);
                }
                else if (expresionWhere[i].ToLower().Equals("not"))
                {
                    op1 = stack.Pop();
                    stack.Push(!op1);
                }
                else
                {
                    if (elem1)
                    {
                        tipoElemento1 = tipoElemento(expresionWhere[i]);
                        if (tipoElemento1.Equals("int"))
                        {
                            elemento1 = Convert.ToInt32(expresionWhere[i]);
                        }
                        else if (tipoElemento1.Equals("date"))
                        {
                            String Fecha = expresionWhere[i].Replace("\'", "");
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
                            elemento1 = expresionWhere[i];
                        }
                        elem1 = false;
                    }
                    else
                    {
                        tipoElemento2 = tipoElemento(expresionWhere[i]);
                        if (tipoElemento2.Equals("int"))
                        {
                            elemento2 = Convert.ToInt32(expresionWhere[i]);
                        }
                        else if (tipoElemento2.Equals("date"))
                        {
                            String Fecha = expresionWhere[i].Replace("\'", "");
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
                            elemento2 = expresionWhere[i];
                        }
                        elem1 = true;
                    }

                }
            }
            return stack.Pop();
        }

    }
}