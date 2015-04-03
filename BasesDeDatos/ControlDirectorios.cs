﻿using System;
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
        public void inicializar()
        {
            if (File.Exists("DataDB\\archivoM.dat") == false)
            {
                File.Create("DataDB\\archivoM.dat").Dispose();
            }
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
            BinaryFormatter formatter = new BinaryFormatter();
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
            //try
            //{
            //    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
            //    miContenido = DeSerializarContenido(contenido);
            //}
            //catch (Exception e)
            //{

            //}
            //try
            //{
            //    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
            //    tablasCreadas = DeSerializarTabla(contenido);
            //}
            //catch (Exception e)
            //{

            //}
            tablasCreadas.agregarRegistro(nombreTabla);
            miContenido.agregarFila(fila);
            contenido = SerializarTabla(tablasCreadas);
            File.WriteAllText("DataDB\\" + DBactual + "\\controlTablas.dat", contenido);
            contenido = SerializarContenido(miContenido);
            File.WriteAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat", contenido);

        }

        public Boolean existePrimaryKey(String nombreTabla, List<Object> fila)
        {
            //String contenido;
            //try
            //{
            //    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\" + nombreTabla + ".dat");
            //    miContenido = DeSerializarContenido(contenido);
            //}
            //catch (Exception e)
            //{

            //}
            //try
            //{
            //    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
            //    tablasCreadas = DeSerializarTabla(contenido);
            //}
            //catch (Exception e)
            //{

            //}
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
            //try
            //{
            //    contenido = File.ReadAllText("DataDB\\" + DBactual + "\\controlTablas.dat");
            //    tablasCreadas = DeSerializarTabla(contenido);
            //}
            //catch (Exception e)
            //{

            //}
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

        //Obtener lista con los nombres de las columnas


        //Muestra todo de las tablas indicadas
        public List<List<Object>> SelectFilas(List<String> nombreTablas, Boolean auxiliar)
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
        
        //Muestra lo seleccionado de las tablas indicadas
        public List<List<Object>> SelectFilas(List<String> nombreTablas,  List<String>columnasSeleccionadas)
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

        //Muestra todo de las tablas indicadas cuando se cumple where
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> expWhere, Boolean auxiliar)
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

        //Muestra lo seleccionado de las tablas indicadas cuando se cumple where
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String>columnasSeleccionadas, List<String> expWhere)
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

//
        //Muestra todo de las tablas indicadas cuando se cumple where en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> expWhere, List<String> columnasOrden, List<String> tipoOrden, Boolean auxiliar)
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

        //Muestra lo indicado de las tablas indicadas cuando se cumple where en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasSeleccionadas, List<String> expWhere, List<String> columnasOrden, List<String> tipoOrden)
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

/////
        //Muestra todo de las tablas indicadas en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasOrden, List<String> tipoOrden, Boolean auxiliar)
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

        //Muestra lo indicado de las tablas indicadas  en el orden especificado
        public List<List<Object>> SelectFilas(List<String> nombreTablas, List<String> columnasSeleccionadas, List<String> columnasOrden, List<String> tipoOrden)
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
                            tituloColumnas.Add(nombreColumnas[j] + "." + tablaColumnas[j]);
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