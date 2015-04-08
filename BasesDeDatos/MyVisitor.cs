// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasesDeDatos
{
    public class MyVisitor : gramSQLBaseVisitor<String>
    {
        //Variables
        String error = "Fate Stay Night";
        public String mensajeError = "";
        public String mensajeInsert = "";
        public String mensajeUpdate = "";
        public String mensajeDelete = "";
        public String mensajeSelect = "";
        int numeroInsert = 0;
        int numeroUpdate = 0;
        int numeroDelete = 0;
        int numeroSelect = 0;
        ControlDirectorios miControl = new ControlDirectorios();
        Tabla tablaNueva = new Tabla();
        PrimaryConstraint tempPC = new PrimaryConstraint();
        ForeignConstraint tempFC = new ForeignConstraint();
        CheckConstraint tempCC = new CheckConstraint();
        String refNombreTabla = "";
        int TipoConstraint = 0;
        Boolean expConstraint = false;
        Boolean expWhere = false;
        Boolean conTabla = false;
        Boolean constraintContenido = false;
        public DataGridView aMostrar = new DataGridView();
        List<String> nombresCol = new List<String>();
        List<String> valuesCol = new List<String>();
        List<String> valuesTipo = new List<String>();
        List<String> nombresTabla = new List<String>();
        List<String> tipoSort = new List<String>();
        List<String> columnaSort = new List<String>();
        List<Object> filaObjetos;
        //public List<DataBase> mostrarDB = new List<DataBase>();

        public override string VisitAlterExpression_database(gramSQLParser.AlterExpression_databaseContext context)
        {
            String nombreViejo = context.GetChild(2).GetText();
            String nombreNuevo = context.GetChild(5).GetText();
            Boolean existeDB = miControl.existeDB(nombreViejo);
            if (!existeDB)
            {
                mensajeError += "No existe la base de datos " + nombreViejo + " a la cual se le desea cambiar nombre. \n";
                return error;
            }
            existeDB = miControl.existeDB(nombreNuevo);
            if (existeDB)
            {
                mensajeError += "Ya existe una base de datos con el nombre " + nombreNuevo + ".\n";
                return error;
            }
            miControl.cambiarNombreDB(nombreViejo, nombreNuevo);
            if (miControl.getDBActual().Equals(nombreViejo)) {
                miControl.setDBActual(nombreNuevo); 
            }   
            return "void";
        }

        public override string VisitDropExpression_database(gramSQLParser.DropExpression_databaseContext context)
        {
            String nombreDatabase = context.GetChild(2).GetText();
            Boolean existeDB = miControl.existeDB(nombreDatabase);
            if (!existeDB)
            {
                mensajeError += "No existe una base de datos con el nombre: " +nombreDatabase+".\n";
            }
            int numRegistros = miControl.numRegistrosDB(nombreDatabase);
            DialogResult resultado = MessageBox.Show("Borrar la base de datos: " + nombreDatabase + " con: " + numRegistros + " registros", "Eliminar Base de Datos", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (resultado == DialogResult.Yes)
            {
                miControl.removerDB(nombreDatabase);
            }
            return "void";

        }

        public override string VisitAccionTabla_DropColumn(gramSQLParser.AccionTabla_DropColumnContext context)
        {
            String idCol = context.GetChild(2).GetText();
            String nombre = tablaNueva.getNombre();
            Boolean existeCol = tablaNueva.existeColumna(idCol);
            if (existeCol)
            {
                mensajeError += "No se puede borrar la columna: "+idCol+ " ya que no existe en la tabla. \n";
                return error;
            }
            Boolean existeColEnConstraint = miControl.columnaEnConstraint(nombre, idCol);
            if (existeColEnConstraint)
            {
                mensajeError += "No se puede borrar la columna: " + idCol + " ya que esta siendo referenciada en otra tabla.\n";
            }
            tablaNueva.removerColumna(idCol);
            return "void";

        }

        public override string VisitTipo(gramSQLParser.TipoContext context)
        {
            if (context.ChildCount == 1)
            {
                if (context.GetChild(0).GetText().Equals("int") || context.GetChild(0).GetText().Equals("Int") || context.GetChild(0).GetText().Equals("INT"))
                {
                    return "int";
                }
                if (context.GetChild(0).GetText().Equals("float") || context.GetChild(0).GetText().Equals("Float") || context.GetChild(0).GetText().Equals("FLOAT"))
                {
                    return "float";
                }
                else
                {
                    return "date";
                }
            }
            else
            {
                return "char "+context.GetChild(2).GetText();
            }
        }

        public override string VisitDeclaracionColumnas(gramSQLParser.DeclaracionColumnasContext context)
        {
            return Visit(context.GetChild(1));
        }

        public override string VisitExpresionOrden2_comita(gramSQLParser.ExpresionOrden2_comitaContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            retorno = Visit(context.GetChild(2));
            if (retorno.Equals(error))
            {
                return error;
            }

            return "void"; 
        }

        public override string VisitListaUpdate2_comita(gramSQLParser.ListaUpdate2_comitaContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            String retorno2 = Visit(context.GetChild(2));
            if (retorno2.Equals(error))
            {
                return error;
            }
            return "void";
        }

        public override string VisitProgram(gramSQLParser.ProgramContext context)
        {
            miControl.inicializar();
            Boolean terminar = false;
            int hijo = 0;
            String retorno = "";
            while (!terminar)
            {
                retorno = Visit(context.GetChild(hijo));
                if (retorno.Equals(error))
                {
                    miControl.terminar();
                    return error;
                }
               
                if ((hijo + 2) == context.ChildCount)
                {
                    terminar = true;
                }
                else
                {
                    hijo = hijo + 2;
                }
            }
            miControl.terminar();
            return "void";
        }

        public override string VisitExpBooleana_expBooleana2(gramSQLParser.ExpBooleana_expBooleana2Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitDeclaracionConstraint1(gramSQLParser.DeclaracionConstraint1Context context)
        {
            return Visit(context.GetChild(0));
            
        }

        public override string VisitCConstraint_check(gramSQLParser.CConstraint_checkContext context)
        {
            String nombreCC = context.GetChild(0).GetText();
            Boolean existeConstraint = tablaNueva.existeIdConstraint(nombreCC);
            if (existeConstraint)
            {
                mensajeError += "La restriccion " + nombreCC + " ya existe.\n";
                return error;
            }
            tempCC = new CheckConstraint();
            tempCC.setChNombre(nombreCC);
            expConstraint = true;

            String retorno = Visit(context.GetChild(3));
            expConstraint =  false;
            if (retorno.Equals(error))
            {
                return error;
            }
            if (!constraintContenido)
            {
                tempCC.setRestriccionExp(retorno);
                tablaNueva.chConstraint.Add(tempCC);
            }
            else
            {
                List<string> whereElements = retorno.Split(new char[] { ' ' }).ToList();
                Boolean cumplenCheck = miControl.tablaCumpleCheck(tablaNueva.getNombre(), whereElements);
                if (cumplenCheck)
                {
                    tempCC.setRestriccionExp(retorno);
                    tablaNueva.chConstraint.Add(tempCC);
                }
                else
                {
                    mensajeError += "El check no se cumple con todos los elementos de la tabla. \n";
                    return error;
                }
            }
            
            
            return "void";


            //return "void";


        }

        public override string VisitInt_literal(gramSQLParser.Int_literalContext context)
        {
            return "int";
        }

        public override string VisitCreate_Table(gramSQLParser.Create_TableContext context)
        {
            if (context.ChildCount == 7)
            {
                if (miControl.getDBActual().Equals(""))
                {
                    mensajeError += "No se ha especificado la base de datos a usar. \n";
                    return error;
                }
                String nombreTabla = context.GetChild(2).GetText();
                Boolean existeTabla = miControl.existeTabla(nombreTabla);
                if (existeTabla)
                {
                    mensajeError += "Ya existe la tabla " + nombreTabla + " en la base de datos actual.\n";
                    return error;
                }
                tablaNueva = new Tabla();
                tablaNueva.setNombre(nombreTabla);
                String retorno = Visit(context.GetChild(4));
                String retorno2 = Visit(context.GetChild(5));
                if (retorno.Equals(error) || retorno2.Equals(error))
                {
                    return error;
                }
                miControl.agregarTabla(tablaNueva);
                return "";
            }
            else
            {
                if (miControl.getDBActual().Equals(""))
                {
                    mensajeError += "No se ha especificado la base de datos a usar. \n";
                    return error;
                }
                String nombreTabla = context.GetChild(2).GetText();
                Boolean existeTabla = miControl.existeTabla(nombreTabla);
                if (existeTabla)
                {
                    mensajeError += "Ya existe la tabla " + nombreTabla + " en la base de datos actual.\n";
                    return error;
                }
                tablaNueva = new Tabla();
                tablaNueva.setNombre(nombreTabla);
                String retorno = Visit(context.GetChild(4));
                if (retorno.Equals(error))
                {
                    return error;
                }
                miControl.agregarTabla(tablaNueva);
                return "";
            }

        }

        public override string VisitDeclaracionConstraint2_comita(gramSQLParser.DeclaracionConstraint2_comitaContext context)
        {
            String retorno1 = Visit(context.GetChild(0));
            if (retorno1.Equals(error))
            {
                return error;
            }
            String retorno2 = Visit(context.GetChild(2));
            if (retorno2.Equals(error))
            {
                return error;
            }
            return "void";
        }

        public override string VisitAccionTabla_AddColumn(gramSQLParser.AccionTabla_AddColumnContext context)
        {
            String idCol = context.GetChild(2).GetText();
            String colTipo = context.GetChild(3).GetText();
            Boolean existeCol = tablaNueva.existeColumna(idCol);
            if (existeCol)
            {
                mensajeError += "La columna: " + idCol + "ya existe en la tabla. \n";
                return error;
            }
            Columna colTemp = new Columna();
            colTemp.setNombre(idCol);
            colTemp.setTipo(colTipo);
            tablaNueva.agregarColumna(colTemp);
            if (context.ChildCount == 5)
            {
                //Boolean para determinar que hay que revisar tabla existente
                constraintContenido = true;
                String retorno = Visit(context.GetChild(4));
                if (retorno.Equals(error))
                {
                    return error;
                }
                constraintContenido = false;
            }
            
            return "void";
        }

        public override string VisitIdComa2_idComa(gramSQLParser.IdComa2_idComaContext context)
        {
            String idNombre = context.GetChild(0).GetText();
            Boolean idRepetido = false;
            Boolean columnaExiste = false;
            Boolean esPrimary = false;
            if (TipoConstraint == 1)
            {
                idRepetido = tempPC.existeIdCol(idNombre);
                if (!idRepetido)
                {
                    columnaExiste = tablaNueva.existeColumna(idNombre);
                    if (columnaExiste)
                    {
                        tempPC.agregarPK(idNombre);
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la tabla actual.\n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en la llave primaria.\n";
                    return error;
                }

            }
            //Mas else if
            else if (TipoConstraint == 2)
            {
                idRepetido = tempFC.existeIdCol(idNombre);
                if (!idRepetido)
                {
                    columnaExiste = tablaNueva.existeColumna(idNombre);
                    if (columnaExiste)
                    {
                        tempFC.agregarFK(idNombre);
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la tabla actual.\n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en la llave foranea.\n";
                    return error;
                }

            }

            else if (TipoConstraint == 3)
            {
                idRepetido = tempFC.existeRefCol(idNombre);
                if (!idRepetido)
                {

                    columnaExiste = miControl.existeColumna(refNombreTabla, idNombre);
                    if (columnaExiste)
                    {
                        esPrimary = miControl.columnaEnPrimaryK(refNombreTabla, idNombre);
                        if (esPrimary)
                        {
                            tempFC.agregarFK(idNombre);
                        }
                        else
                        {
                            mensajeError += "La columna " + idNombre + " debe ser primaria.\n";
                            return error;
                        }
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la tabla " + refNombreTabla + ".\n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en las referencias.\n";
                    return error;
                }

            }

            return "void";
        }

        public override string VisitCConstraint_foreign(gramSQLParser.CConstraint_foreignContext context)
        {
            String nombreFK = context.GetChild(0).GetText();
            Boolean existeConstraint = tablaNueva.existeIdConstraint(nombreFK);
            if (existeConstraint)
            {
                mensajeError += "La restriccion " + nombreFK + " ya existe.\n";
                return error;
            }
            tempFC = new ForeignConstraint();
            tempFC.setFkNombre(nombreFK);
            TipoConstraint = 2;

            String retorno = Visit(context.GetChild(4));
            TipoConstraint = 0;
            if (retorno.Equals(error))
            {
                return error;
            }

            String nombreTabla = context.GetChild(7).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla " + nombreTabla + ".\n";
                return error;
            }
            refNombreTabla = nombreTabla;
            tempFC.setTablaRefNombre(refNombreTabla);
            TipoConstraint = 3;
            String retorno2 = Visit(context.GetChild(9));
            TipoConstraint = 0;
            if (retorno2.Equals(error))
            {
                return error;
            }

            if (tempFC.idCol.Count() != tempFC.refCol.Count())
            {
                mensajeError += "No coincide el numero de columnas referenciadas en las llaves foraneas con el numero de las referencias. \n";
                return error;
            }

            tablaNueva.fConstraint.Add(tempFC);
            return "void";
        }

        public override string VisitDeclaracionColumnas2_declaracion(gramSQLParser.DeclaracionColumnas2_declaracionContext context)
        {
            
            String nombreCol = context.GetChild(0).GetChild(0).GetText();
            String tipoCol = Visit(context.GetChild(0));
            Columna colTemp = new Columna();
            colTemp.setNombre(nombreCol);
            colTemp.setTipo(tipoCol);

            Boolean existeCol = tablaNueva.existeColumna(nombreCol);
            if (existeCol)
            {
                mensajeError += "La columna " + nombreCol + " ya ha sido especificada.\n";
                return error;
            }

            tablaNueva.agregarColumna(colTemp);
            return "void";

            
        }

        public override string VisitExpresionOrden(gramSQLParser.ExpresionOrdenContext context)
        {
            String nombreColumna = Visit(context.GetChild(0));
            if (nombreColumna.Equals(error))
            {
                return error;
            }
            columnaSort.Add(nombreColumna);
            if (context.ChildCount == 2)
            {
                String typeSort = context.GetChild(1).GetText().ToLower();

                tipoSort.Add(typeSort);
                
            }
            else
            {
                tipoSort.Add("asc");
            }
            return "void";
            

        }

        public override string VisitIgualacion(gramSQLParser.IgualacionContext context)
        {
            String nombreColumna = context.GetChild(0).GetText();
            Boolean existeColumna = miControl.existeColumna(refNombreTabla, nombreColumna);
            if(!existeColumna)
            {
                mensajeError += "no existe la columna "+nombreColumna+" en la tabla "+refNombreTabla+". \n";
                return error;
            }
            String tipoColumna = miControl.obtenerTipoCol(refNombreTabla,nombreColumna);

            String retorno = Visit(context.GetChild(2));
            if(retorno.Equals(error)){
                return error;
            }
            if(!(tipoColumna.Equals(retorno)))
            {
                 if (!(((tipoColumna.Equals("int") || tipoColumna.Equals("float")) && (retorno.Equals("int") || retorno.Equals("float"))) || (tipoColumna.Contains("char") && retorno.Equals("date")) || (tipoColumna.Contains("char") && retorno.Contains("char"))))
                {
                    mensajeError += "No se puede guardar un " + retorno + " como un " + tipoColumna + ".\n";
                    return error;
                }
            }
            nombresCol.Add(nombreColumna);
            valuesCol.Add(context.GetChild(2).GetText());
            valuesTipo.Add(retorno);

            return "void";

        }

        public override string VisitIdComa1(gramSQLParser.IdComa1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitColumnaDatos_id(gramSQLParser.ColumnaDatos_idContext context)
        {
            String nombreCol = context.GetChild(0).GetText();
            Boolean existeCol;
            String tipoCol = "";
            if (expWhere)
            {
                if (expConstraint)
                {
                    existeCol = miControl.existeColumna(refNombreTabla, nombreCol);
                    if (!existeCol)
                    {
                        mensajeError += "no existe la columna " + nombreCol + ". \n";
                        return error;
                    }
                    tipoCol = miControl.obtenerTipoCol(refNombreTabla, nombreCol);
                }
                else
                {
                    for (int i = 0; i < nombresTabla.Count; i++)
                    {
                        existeCol = miControl.existeColumna(nombresTabla[i], nombreCol);
                        if (existeCol)
                        {
                            return miControl.obtenerTipoCol(nombresTabla[i], nombreCol);
                        }
                        
                    }
                    mensajeError += "no existe la columna " + nombreCol + ". \n";
                    return error;
                }
                
            }
            else
            {
                existeCol = tablaNueva.existeColumna(nombreCol);
                if (!existeCol)
                {
                    mensajeError += "no existe la columna " + nombreCol + ". \n";
                    return error;
                }
                tipoCol = tablaNueva.tipoColumna(nombreCol);
            }
            
            return tipoCol;
        }

        public override string VisitColumnaDatos_null(gramSQLParser.ColumnaDatos_nullContext context)
        {
            return "null";
        }

        public override string VisitNombreTablaColumna_id(gramSQLParser.NombreTablaColumna_idContext context)
        {
            //columna
            if (conTabla)
            {
                String nombreColumna = context.GetChild(0).GetText();
                int coincidencias = 0;
                Boolean existe = false;
                for (int i = 0; i < nombresTabla.Count; i++)
                {
                    existe = miControl.existeColumna(nombresTabla[i], nombreColumna);
                    if (existe)
                    {
                        coincidencias++;
                    }
                }
                if (coincidencias == 0)
                {
                    mensajeError += "las tablas referenciadas no poseen una tabla de nombre " + nombreColumna + ". \n";
                    return error;
                }
                else if (coincidencias > 1)
                {
                    mensajeError += "existe ambiguedad al hacer uso de la columna " + nombreColumna + ". \n";
                    return error;
                }

                return nombreColumna;
            }
            //tabla
            else
            {
                String nombreTabla = context.GetChild(0).GetText();
                Boolean existeTabla = miControl.existeTabla(nombreTabla);
                if (existeTabla)
                {
                    return nombreTabla;
                }
                else
                {
                    mensajeError += "no existe la tabla de nombre " + nombreTabla + ". \n";
                    return error;
                }

            }
        }


        public override string VisitDropExpression_table(gramSQLParser.DropExpression_tableContext context)
        {
            String nombreTabla = context.GetChild(2).GetText();
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla " + nombreTabla + " en la base de datos. \n";
                return error;
            }
            Boolean tablaReferenciada = miControl.tablaEnReferencia(nombreTabla);
            if (tablaReferenciada)
            {
                mensajeError += "La tabla " + nombreTabla + " no puede ser borrada ya que es referenciada en otra tabla. \n";
                return error;
            }
            miControl.removerTabla(nombreTabla);
            return "void";
           
        }

        public override string VisitListaValores2_comita(gramSQLParser.ListaValores2_comitaContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            String valueLiteral = context.GetChild(0).GetText();
            valuesCol.Add(valueLiteral);
            String retorno2 = Visit(context.GetChild(2));
            if (retorno2.Equals(error))
            {
                return error;
            }
            return "void";

        }

        public override string VisitShowColumnsExpression(gramSQLParser.ShowColumnsExpressionContext context)
        {
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No se ha especificado la base de datos a usar. \n";
                return error;
            }
            String nombreTabla = context.GetChild(3).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla " + nombreTabla + " en esta base de datos. \n";
            }
            aMostrar.ColumnCount = 2;
            aMostrar.RowCount = miControl.getTablasCreadas().getListaTB().Count + 1;

            aMostrar.Rows[0].Cells[0].Value = "Nombre Columna";
            aMostrar.Rows[0].Cells[1].Value = "Tipo Columna";
            for (int i = 1; i < aMostrar.RowCount; i++)
            {
                aMostrar.Rows[i].Cells[0].Value = miControl.getTablasCreadas().obtenerTabla(nombreTabla).columnasTB[i-1].getNombre();
                aMostrar.Rows[i].Cells[1].Value = miControl.getTablasCreadas().obtenerTabla(nombreTabla).columnasTB[i - 1].getTipo() + "";
            }
            //mostrarDB = miControl.getBasesCreadas().listaDB;
            return "void";
        }

        public override string VisitCConstraint_primary(gramSQLParser.CConstraint_primaryContext context)
        {
            if (tablaNueva.pConstraint.Count() == 1)
            {
                mensajeError += "No se permiten multiples llaves primarias.\n";
                return error;
            }
            
            String nombrePK = context.GetChild(0).GetText();
            Boolean existeConstraint = tablaNueva.existeIdConstraint(nombrePK);
            if (existeConstraint)
            {
                mensajeError += "La restriccion " + nombrePK + " ya existe.\n";
                return error;
            } 
            tempPC = new PrimaryConstraint();
            tempPC.setPkNombre(nombrePK);
            TipoConstraint = 1;
            
            String retorno = Visit(context.GetChild(4));
            TipoConstraint = 0;
            if (retorno.Equals(error))
            {
                return error;
            }
            if (!constraintContenido)
            {
                tablaNueva.pConstraint.Add(tempPC);
            }
            else
            {
                Boolean unique = miControl.unicidadEnTabla(tablaNueva.getNombre(), tempPC.idCol);
                if(unique)
                {
                    tablaNueva.pConstraint.Add(tempPC);
                }
                else
                {
                    mensajeError += "Se viola la unicidad de la Primary Key en la tabla. \n";
                    return error;
                }
            }
            return "void";
        }

        public override string VisitShowExpression_Tables(gramSQLParser.ShowExpression_TablesContext context)
        {
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No se ha especificado la base de datos a usar. \n";
                return error;
            }
            aMostrar.ColumnCount = 3;
            aMostrar.RowCount = miControl.getTablasCreadas().getListaTB().Count + 1;

            aMostrar.Rows[0].Cells[0].Value = "Nombre Tabla";
            aMostrar.Rows[0].Cells[1].Value = "Numero de registros";
            aMostrar.Rows[0].Cells[2].Value = "Numero de Columnas";
            for (int i = 1; i < aMostrar.RowCount; i++)
            {
                aMostrar.Rows[i].Cells[0].Value = miControl.getTablasCreadas().getListaTB()[i - 1].getNombre();
                aMostrar.Rows[i].Cells[1].Value = miControl.getTablasCreadas().getListaTB()[i - 1].getNumRegistros() +"";
                aMostrar.Rows[i].Cells[2].Value = miControl.getTablasCreadas().getListaTB()[i - 1].getNumColumnas() + "";
            }
            //mostrarDB = miControl.getBasesCreadas().listaDB;
            return "void";
        }

        public override string VisitDeclaracionConstraint(gramSQLParser.DeclaracionConstraintContext context)
        {
            return Visit(context.GetChild(1));
            
        }

        public override string VisitListaValores2_valores(gramSQLParser.ListaValores2_valoresContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            String valueLiteral = context.GetChild(0).GetText();
            valuesCol.Add(valueLiteral);
            return "void";
        }

        public override string VisitExpresionOrden2_expresionOrden(gramSQLParser.ExpresionOrden2_expresionOrdenContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            return "void";
        }

        public override string VisitListaUpdate2_igualacion(gramSQLParser.ListaUpdate2_igualacionContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            return "void";
        }

        public override string VisitListaColumna1(gramSQLParser.ListaColumna1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitListaTablaColumna1(gramSQLParser.ListaTablaColumna1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitVarchar_literal(gramSQLParser.Varchar_literalContext context)
        {
            return "char";
        }

        public override string VisitListaValores1(gramSQLParser.ListaValores1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitExpBooleana3_not(gramSQLParser.ExpBooleana3_notContext context)
        {
            String retorno = Visit(context.GetChild(0));
            if (retorno.Equals(error))
            {
                return error;
            }
            return  retorno + " NOT";
        }

        public override string VisitAccionTabla_DropConstraint(gramSQLParser.AccionTabla_DropConstraintContext context)
        {
            String nombreConstraint = context.GetChild(2).GetText();
            Boolean existeConstraint = tablaNueva.existeIdConstraint(nombreConstraint);
            if (!existeConstraint)
            {
                mensajeError += "El constraint: " + nombreConstraint + " no existe en la tabla.\n";
                return error;
            }
            tablaNueva.removerConstraint(nombreConstraint);
            return "void";
        }

        public override string VisitExpRelacion(gramSQLParser.ExpRelacionContext context)
        {
            String retorno1 = context.GetChild(0).GetText();
            String retorno2 = context.GetChild(2).GetText();
            if (retorno1.ToLower().Equals("null"))
            {
                retorno1 = null;
            }
            if (retorno2.ToLower().Equals("null"))
            {
                retorno2 = null;
            }
            String opSimb = context.GetChild(1).GetText();
            String type1 = Visit(context.GetChild(0));
            String type2 = Visit(context.GetChild(2));
            if (type1.Equals(error) || type2.Equals(error))
            {
                return error;
            }
            if (opSimb.Equals("<") || opSimb.Equals(">") || opSimb.Equals("<=") || opSimb.Equals(">="))
            {
                if (!(((type1.Equals("int") || type1.Equals("float")) && (type2.Equals("int") || type2.Equals("float")))||(type1.Equals("date")&&type2.Equals("date"))))
                {
                    mensajeError += "No se puede comparar un " + type1 + " con un " + type2 + " usando el operador " + opSimb;
                    return error;
                }
            }
            else if (!(type1.Equals(type2)))
            {
                if (!(((type1.Equals("int") || type1.Equals("float")) && (type2.Equals("int") || type2.Equals("float"))) || (type1.Contains("char") && type2.Equals("date")) || (type1.Contains("char") && type2.Contains("char"))||(type1.Equals("null")||type2.Equals("null"))))
                {
                    mensajeError += "No se puede relacionar un " + type1 + " con un " + type2 + ".\n";
                    return error;
                }
            }
            String nuevoRetorno = retorno1 + " " + retorno2 + " " + opSimb;
            return nuevoRetorno;
        }

        public override string VisitShowExpression_Databases(gramSQLParser.ShowExpression_DatabasesContext context)
        {
            aMostrar.ColumnCount = 2;
            aMostrar.RowCount = miControl.getBasesCreadas().getListaDB().Count + 1;

            aMostrar.Rows[0].Cells[0].Value = "Nombre Data Base";
            aMostrar.Rows[0].Cells[1].Value = "Numero de tablas";
            for (int i = 1; i < aMostrar.RowCount; i++)
            {
                aMostrar.Rows[i].Cells[0].Value = miControl.getBasesCreadas().getListaDB()[i-1].getNombre();
                aMostrar.Rows[i].Cells[1].Value = miControl.getBasesCreadas().getListaDB()[i - 1].getNumTablas() + "";
            }
            //mostrarDB = miControl.getBasesCreadas().listaDB;
            return "void";
        }

        public override string VisitNombreColumna(gramSQLParser.NombreColumnaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitValores(gramSQLParser.ValoresContext context)
        {
            if (context.GetChild(0).GetText().Equals("NULL") || context.GetChild(0).GetText().Equals("Null") || context.GetChild(0).GetText().Equals("null"))
            {
                valuesTipo.Add("null");
                return "null";
            }
            else
            {
                String tipoLiteral = Visit(context.GetChild(0));


                if (tipoLiteral.Equals(error))
                {
                    return error;
                }
                valuesTipo.Add(tipoLiteral);
                return tipoLiteral;
            }
        }

        public override string VisitUpdateExpression(gramSQLParser.UpdateExpressionContext context)
        {
            String nombreTabla = context.GetChild(1).GetText();
            
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla: " + nombreTabla + ".\n";
                return error;
            }


            nombresCol = new List<String>();
            refNombreTabla = nombreTabla;
            valuesCol = new List<String>();
            valuesTipo = new List<String>();

            String retorno = Visit(context.GetChild(3));
            if (retorno.Equals(error))
            {
                return error;
            }

            //Hacer lista de objetos y revisar especificaciones por tipo
            //Hacer lista de objetos
            filaObjetos = new List<Object>();
            for (int i = 0; i < miControl.obtenerNumColumnas(nombreTabla); i++)
            {
                filaObjetos.Add(null);
            }
            String tipoColumna;
            int indiceColumna;
            String tipoValues;
            for (int i = 0; i < nombresCol.Count; i++)
            {
                tipoColumna = miControl.obtenerTipoCol(nombreTabla, nombresCol[i]);
                indiceColumna = miControl.obtenerIndiceCol(nombreTabla, nombresCol[i]);
                tipoValues = valuesTipo[i];
                if (tipoColumna.Equals("int"))
                {
                    if (tipoValues.Equals("int"))
                    {
                        int numero = Convert.ToInt32(valuesCol[i]);
                        filaObjetos[indiceColumna] = numero;
                    }
                    else if (tipoValues.Equals("float"))
                    {
                        float numero = Convert.ToSingle(valuesCol[i]);
                        int numero2 = (int)Math.Floor(numero);
                        filaObjetos[indiceColumna] = numero2;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un int.";
                        return error;
                    }

                }
                else if (tipoColumna.Equals("float"))
                {
                    if (tipoValues.Equals("float"))
                    {
                        float numero = Convert.ToSingle(valuesCol[i]);
                        filaObjetos[indiceColumna] = numero;
                    }
                    else if (tipoValues.Equals("int"))
                    {
                        int numero = Convert.ToInt32(valuesCol[i]);
                        float numero2 = (float)numero;
                        filaObjetos[indiceColumna] = numero2;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un float.";
                        return error;
                    }
                }
                else if (tipoColumna.Equals("date"))
                {
                    if (tipoValues.Equals("date"))
                    {
                        String Fecha = valuesCol[i].Replace("\'", "");
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
                        filaObjetos[indiceColumna] = date;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un date.";
                        return error;
                    }
                }
                else
                {
                    if (tipoValues.Equals("date"))
                    {
                        filaObjetos[indiceColumna] = valuesCol[i];
                    }
                    else if (tipoValues.Contains("char"))
                    {
                        String sizeChar = tipoColumna.Replace("char ", "");
                        int size = Convert.ToInt32(sizeChar);
                        int realSize = valuesCol[i].Length - 2;
                        if (realSize <= size)
                        {
                            filaObjetos[indiceColumna] = valuesCol[i].Replace("\' ", "");
                        }
                        else
                        {
                            int indice = i + 1;
                            mensajeError += "El varchar en el valor " + indice + " supera el tamaño especificado. \n";
                            return error;
                        }

                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un varchar.";
                        return error;
                    }
                }
            }

            //Llamar metodo update
            int filasUpdate;
            if (context.ChildCount == 4)
            {
                filasUpdate = miControl.UpdateColumnas(filaObjetos, nombreTabla);
                numeroUpdate = numeroUpdate + filasUpdate;
                mensajeUpdate = "Update " + numeroUpdate + " con exito. \n";
                if (miControl.falloUpdate)
                {
                    mensajeError += miControl.mensajeFallo2;
                    return error;
                }
            }
            else
            {
                expConstraint = true;
                expWhere = true;
                retorno = Visit(context.GetChild(5));
                expConstraint = false;
                expWhere = false;
                if (retorno.Equals(error))
                {
                    return error;
                }
                List<string> whereElements = retorno.Split(new char[] { ' ' }).ToList();
                filasUpdate = miControl.UpdateColumnas(filaObjetos, nombreTabla, whereElements);
                numeroUpdate = numeroUpdate + filasUpdate;
                mensajeUpdate = "Update " + numeroUpdate + " con exito. \n";
                if (miControl.falloUpdate)
                {
                    mensajeError += miControl.mensajeFallo2;
                    return error;
                }
                
            }
            return "void";
        }

        public override string VisitUseExpression(gramSQLParser.UseExpressionContext context)
        {
            String nombreDataBase = context.GetChild(2).GetText();
            Boolean existeDB = miControl.existeDB(nombreDataBase);
            if (!existeDB)
            {
                mensajeError += "No existe la base de datos " + nombreDataBase + ". \n";
                return error;
            }
            miControl.setDBActual(nombreDataBase);
            return "void";

        }

        public override string VisitInsertExpression(gramSQLParser.InsertExpressionContext context)
        {
            String nombreTabla = context.GetChild(2).GetText();
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }

            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla: " + nombreTabla + ".\n";
                return error;
            }
            nombresCol = new List<String>();
            refNombreTabla = nombreTabla;
            //Recorrer nombres columna
            String retorno1 = Visit(context.GetChild(4));
            if (retorno1.Equals(error))
            {
                return error;
            }
            //Recorrer values
            //nombresCol = new List<String>();
            valuesTipo = new List<String>();
            valuesCol = new List<String>();
            String retorno2 = Visit(context.GetChild(8));
            if (retorno2.Equals(error))
            {
                return error;
            }
            //Error 1 listas tamaños diferentes
            if (valuesCol.Count != nombresCol.Count)
            {
                mensajeError += "El numero de los valores a ingresar no coincide con las columnas especificadas. \n";
                return error;
            }
            //Error 2 coincidencia de tipos

            //Hacer lista de objetos
            filaObjetos = new List<Object>();
            for (int i = 0; i < miControl.obtenerNumColumnas(nombreTabla); i++)
            {
                filaObjetos.Add(null);
            }
            String tipoColumna;
            int indiceColumna;
            String tipoValues;
            for (int i = 0; i < nombresCol.Count; i++)
            {
                tipoColumna = miControl.obtenerTipoCol(nombreTabla, nombresCol[i]);
                indiceColumna = miControl.obtenerIndiceCol(nombreTabla, nombresCol[i]);
                tipoValues = valuesTipo[i];
                if (tipoColumna.Equals("int"))
                {
                    if (tipoValues.Equals("int"))
                    {
                        int numero = Convert.ToInt32(valuesCol[i]);
                        filaObjetos[indiceColumna] = numero;
                    }
                    else if (tipoValues.Equals("float"))
                    {
                        float numero = Convert.ToSingle(valuesCol[i]);
                        int numero2 = (int)Math.Floor(numero);
                        filaObjetos[indiceColumna] = numero2;
                    }
                    else if (tipoValues.Equals("null"))
                    {
                        filaObjetos[indiceColumna] = null;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un int.";
                        return error;
                    }
                    
                }
                else if (tipoColumna.Equals("float"))
                {
                    if (tipoValues.Equals("float"))
                    {
                        float numero = Convert.ToSingle(valuesCol[i]);
                        filaObjetos[indiceColumna] = numero;
                    }
                    else if (tipoValues.Equals("int"))
                    {
                        int numero = Convert.ToInt32(valuesCol[i]);
                        float numero2 = (float)numero;
                        filaObjetos[indiceColumna] = numero2;
                    }
                    else if (tipoValues.Equals("null"))
                    {
                        filaObjetos[indiceColumna] = null;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un float.";
                        return error;
                    }
                }
                else if (tipoColumna.Equals("date"))
                {
                    if (tipoValues.Equals("date"))
                    {
                        String Fecha = valuesCol[i].Replace("\'", "");
                        List<string> dateElements = Fecha.Split(new char[] { '-' }).ToList();
                        Fecha = dateElements[0]+"-";
                        if (dateElements[1].Length == 1)
                        {
                            Fecha+=0+dateElements[1]+"-";
                        }
                        else{
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
                        filaObjetos[indiceColumna] = date;
                    }
                    else if (tipoValues.Equals("null"))
                    {
                        filaObjetos[indiceColumna] = null;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un date.";
                        return error;
                    }
                }
                else
                {
                    if (tipoValues.Equals("date"))
                    {
                        filaObjetos[indiceColumna] = valuesCol[i];
                    }
                    else if (tipoValues.Contains("char"))
                    {
                        String sizeChar = tipoColumna.Replace("char ", "");
                        int size = Convert.ToInt32(sizeChar);
                        int realSize = valuesCol[i].Length - 2;
                        if (realSize <= size)
                        {
                            filaObjetos[indiceColumna] = valuesCol[i].Replace("\' ", "");
                        }
                        else
                        {
                            int indice = i + 1;
                            mensajeError += "El varchar en el valor " + indice + " supera el tamaño especificado. \n";
                            return error;
                        }

                    }
                    else if (tipoValues.Equals("null"))
                    {
                        filaObjetos[indiceColumna] = null;
                    }
                    else
                    {
                        int indice = i + 1;
                        mensajeError += "En el valor numero " + indice + " se esperaba un varchar.";
                        return error;
                    }
                }
            }
            //Revisar el check para poder adjuntar la fila
            //Aqui va el check :D
            Boolean cumplen = miControl.revisarConstraint(filaObjetos, nombreTabla);
            if (cumplen)
            {
                Boolean pKeyNulo = miControl.primaryNull(nombreTabla, filaObjetos);
                if (!pKeyNulo)
                {
                    Boolean existePKey = miControl.existePrimaryKey(nombreTabla, filaObjetos);
                    
                    if (!existePKey)
                    {
                        Boolean ExisteFKey = miControl.existeForeignKey(nombreTabla, filaObjetos);
                        if (ExisteFKey)
                        {
                            miControl.agregarFilaTabla(nombreTabla, filaObjetos);
                            numeroInsert = numeroInsert + 1;
                            mensajeInsert = "Insert " + numeroInsert + " con exito. \n";
                        }
                        else
                        {
                            mensajeError += "Se viola la primary key ya que no existe el elemento que se esta referenciando. \n";
                            return error; 
                        }
                       
                    }
                    else
                    {
                        mensajeError += "Ya existe la llave primaria que se trata de agregar. \n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "Una de las llaves primarias no puede ser null. \n";
                    return error;
                }
                
            }
            else
            {
                mensajeError += "No se pudo agregar la fila en "+nombreTabla+". \n";
                return error;
            }

            return "void";
        }

        public override string VisitColumnaDatos_referencia(gramSQLParser.ColumnaDatos_referenciaContext context)
        {
            if (expConstraint)
            {
                mensajeError += "No se puede referenciar columnas de otras tablas en esta condicion. \n";
                return error;
            }
            //codigo del WHERE
            String nombreTabla = context.GetChild(0).GetText();
            String nombreCol = context.GetChild(2).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "no existe una tabla de nombre " + nombreTabla + ". \n";
                return error;
            }

            if (!nombresTabla.Contains(nombreTabla))
            {
                mensajeError += "no se especifico hacer uso de la tabla " + nombreTabla + ". \n";
                return error;
            }

            Boolean existeCol = miControl.existeColumna(nombreTabla, nombreCol);
            if (!existeCol)
            {
                mensajeError += "no existe la columna " + nombreCol + "en la tabla "+nombreTabla+". \n";
                return error;
            }
            String tipoCol = miControl.obtenerTipoCol(nombreTabla, nombreCol);
            return tipoCol;

        }

        public override string VisitNombreTablaColumna_referencia(gramSQLParser.NombreTablaColumna_referenciaContext context)
        {
            //columna
            if (conTabla)
            {
                String nombreColumna = context.GetChild(2).GetText();
                String nombreTabla = context.GetChild(0).GetText();
                Boolean existe = miControl.existeTabla(nombreTabla);
                if (!existe)
                {
                    mensajeError += "No existe la tabla de nombre " + nombreTabla + ". \n";
                    return error;
                }
                existe = miControl.existeColumna(nombreTabla, nombreColumna);
                if (!existe)
                {
                    mensajeError += "No existe la columna de nombre " + nombreColumna + "en la tabla "+nombreTabla+". \n";
                    return error;
                }
                return nombreTabla + "." + nombreColumna;

            }
            //tabla
            else
            {
                mensajeError += "se requiere el nombre de una tabla, no una referencia .\n";
                return error;
            }
        }

        public override string VisitLiteral(gramSQLParser.LiteralContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitSelectExpression(gramSQLParser.SelectExpressionContext context)
        {
            Boolean seleccionarTodo = false;
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }

            String retorno = Visit(context.GetChild(3));

            if (retorno.Equals(error))
            {
                return error;
            }

            if (context.GetChild(1).GetText().Equals("*"))
            {
                seleccionarTodo = true;
            }
            else
            {
                nombresCol = new List<String>();
                conTabla = true;
                retorno = Visit(context.GetChild(1));
                conTabla = false;
                if (retorno.Equals(error))
                {
                    return error;
                }
            }

            if (context.ChildCount == 4)
            {
                if (seleccionarTodo)
                {
                    //llamar metodo 1 parametros (nombreTablas)
                    //aMostrar = new DataGridView();
                    List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, true);
                    List<String> titulos = miControl.tituloColumnas;
                    
                    aMostrar.ColumnCount = titulos.Count;
                    aMostrar.RowCount = tablaParaMostrar.Count+1;

                    for (int i = 0; i < titulos.Count; i++)
                    {
                        aMostrar.Rows[0].Cells[i].Value = titulos[i];
                    }
                    for (int i = 1; i < aMostrar.RowCount; i++)
                    {
                        for (int j = 0; j < aMostrar.ColumnCount; j++)
                        {
                            aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i-1][j];
                            
                        }
                        
                    }
                    numeroSelect = tablaParaMostrar.Count;
                    mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                    
                }
                else
                {
                    //llamar metodo 2 parametros (nombreTablas, select)
                    List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla,nombresCol);
                    List<String> titulos = miControl.tituloColumnas;

                    aMostrar.ColumnCount = titulos.Count;
                    aMostrar.RowCount = tablaParaMostrar.Count + 1;

                    for (int i = 0; i < titulos.Count; i++)
                    {
                        aMostrar.Rows[0].Cells[i].Value = titulos[i];
                    }
                    for (int i = 1; i < aMostrar.RowCount; i++)
                    {
                        for (int j = 0; j < aMostrar.ColumnCount; j++)
                        {
                            aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                        }

                    }
                    numeroSelect = tablaParaMostrar.Count;
                    mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                }
                
            }
            else
            {
                String palabra = context.GetChild(4).GetText().ToLower();
                if (palabra.Equals("where"))
                {
                    
                    expWhere = true;
                    retorno = Visit(context.GetChild(5));
                    expWhere = false;
                    if (retorno.Equals(error))
                    {
                        return error;
                    }
                    List<string> whereElements = retorno.Split(new char[] { ' ' }).ToList();
                    if (context.ChildCount == 6)
                    {
                        if (seleccionarTodo)
                        {
                            //llamar a metodo que usa 2 parametros (nombreTablas y where)
                            List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, whereElements, true);
                            List<String> titulos = miControl.tituloColumnas;

                            aMostrar.ColumnCount = titulos.Count;
                            aMostrar.RowCount = tablaParaMostrar.Count + 1;

                            for (int i = 0; i < titulos.Count; i++)
                            {
                                aMostrar.Rows[0].Cells[i].Value = titulos[i];
                            }
                            for (int i = 1; i < aMostrar.RowCount; i++)
                            {
                                for (int j = 0; j < aMostrar.ColumnCount; j++)
                                {
                                    aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                                }

                            }
                            numeroSelect = tablaParaMostrar.Count;
                            mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                        }
                        else
                        {
                            //llamar a metodo que usa 3 parametros (nombreTablas, select y where)
                            List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, nombresCol, whereElements);
                            List<String> titulos = miControl.tituloColumnas;

                            aMostrar.ColumnCount = titulos.Count;
                            aMostrar.RowCount = tablaParaMostrar.Count + 1;

                            for (int i = 0; i < titulos.Count; i++)
                            {
                                aMostrar.Rows[0].Cells[i].Value = titulos[i];
                            }
                            for (int i = 1; i < aMostrar.RowCount; i++)
                            {
                                for (int j = 0; j < aMostrar.ColumnCount; j++)
                                {
                                    aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                                }

                            }
                            numeroSelect = tablaParaMostrar.Count;
                            mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                        }
                        
                    }
                    else
                    {
                        tipoSort = new List<String>();
                        columnaSort = new List<String>();
                        conTabla = true;
                        retorno = Visit(context.GetChild(9));
                        conTabla = false;
                        if (retorno.Equals(error))
                        {
                            return error;
                        }

                        if (seleccionarTodo)
                        {
                            //llamar metodo que usa 4 parametros (nombre, where, tipoSort, columnaSort)
                            List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, whereElements, columnaSort, tipoSort, true);
                            List<String> titulos = miControl.tituloColumnas;

                            aMostrar.ColumnCount = titulos.Count;
                            aMostrar.RowCount = tablaParaMostrar.Count + 1;

                            for (int i = 0; i < titulos.Count; i++)
                            {
                                aMostrar.Rows[0].Cells[i].Value = titulos[i];
                            }
                            for (int i = 1; i < aMostrar.RowCount; i++)
                            {
                                for (int j = 0; j < aMostrar.ColumnCount; j++)
                                {
                                    aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                                }

                            }
                            numeroSelect = tablaParaMostrar.Count;
                            mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                        }
                        else
                        {
                            //llamar metodo que usa 5 parametros (nombre, select, where, tipoSort, columnaSort)
                            List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, nombresCol, whereElements, columnaSort, tipoSort);
                            List<String> titulos = miControl.tituloColumnas;

                            aMostrar.ColumnCount = titulos.Count;
                            aMostrar.RowCount = tablaParaMostrar.Count + 1;

                            for (int i = 0; i < titulos.Count; i++)
                            {
                                aMostrar.Rows[0].Cells[i].Value = titulos[i];
                            }
                            for (int i = 1; i < aMostrar.RowCount; i++)
                            {
                                for (int j = 0; j < aMostrar.ColumnCount; j++)
                                {
                                    aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                                }

                            }
                            numeroSelect = tablaParaMostrar.Count;
                            mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                        }
                        

                    }
                }
                else
                {
                    tipoSort = new List<String>();
                    columnaSort = new List<String>();
                    conTabla = true;
                    retorno = Visit(context.GetChild(7));
                    conTabla = false;
                    if (retorno.Equals(error))
                    {
                        return error;
                    }
                    if (seleccionarTodo)
                    {
                        //llamar metodo que usa 3 parametros (nombre, tipoSort, columnaSort)
                        List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla, columnaSort, tipoSort, true);
                        List<String> titulos = miControl.tituloColumnas;

                        aMostrar.ColumnCount = titulos.Count;
                        aMostrar.RowCount = tablaParaMostrar.Count + 1;

                        for (int i = 0; i < titulos.Count; i++)
                        {
                            aMostrar.Rows[0].Cells[i].Value = titulos[i];
                        }
                        for (int i = 1; i < aMostrar.RowCount; i++)
                        {
                            for (int j = 0; j < aMostrar.ColumnCount; j++)
                            {
                                aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                            }

                        }
                        numeroSelect = tablaParaMostrar.Count;
                        mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                    }
                    else
                    {
                        //llamar metodo que usa 4 parametros (nombre, select, tipoSort, columnaSort)
                        List<List<Object>> tablaParaMostrar = miControl.SelectFilas(nombresTabla,nombresCol, columnaSort, tipoSort);
                        List<String> titulos = miControl.tituloColumnas;

                        aMostrar.ColumnCount = titulos.Count;
                        aMostrar.RowCount = tablaParaMostrar.Count + 1;

                        for (int i = 0; i < titulos.Count; i++)
                        {
                            aMostrar.Rows[0].Cells[i].Value = titulos[i];
                        }
                        for (int i = 1; i < aMostrar.RowCount; i++)
                        {
                            for (int j = 0; j < aMostrar.ColumnCount; j++)
                            {
                                aMostrar.Rows[i].Cells[j].Value = tablaParaMostrar[i - 1][j];

                            }

                        }
                        numeroSelect = tablaParaMostrar.Count;
                        mensajeSelect = "Se muestran " + numeroSelect + " filas. ";
                    }
                    
                }
            }
            return "void";
        }

        public override string VisitListaColumna2_nombreColumna(gramSQLParser.ListaColumna2_nombreColumnaContext context)
        {
            String nombreColumna = context.GetChild(0).GetText();
            Boolean existeColumna = miControl.existeColumna(refNombreTabla, nombreColumna);
            if (!existeColumna)
            {
                mensajeError += "No existe una columna de nombre " + nombreColumna + ".\n";
                return error;
            }
            nombresCol.Add(nombreColumna);
            return "void";


        }

        public override string VisitListaTablaColumna2_nombreColumna(gramSQLParser.ListaTablaColumna2_nombreColumnaContext context)
        {
            
            //lista de columnas
            if (conTabla)
            {
                String nombreColumna = Visit(context.GetChild(0));
                if (nombreColumna.Equals(error))
                {
                    return error;
                }

                nombresCol.Add(nombreColumna);

            }
            //lista tablas
            else
            {
                String nombreTabla= Visit(context.GetChild(0));
                if (nombreTabla.Equals(error))
                {
                    return error;
                }
                nombresTabla.Add(nombreTabla);
            }

            return "void";

        }

        public override string VisitAlterExpression_table(gramSQLParser.AlterExpression_tableContext context)
        {
            String nombreViejo = context.GetChild(2).GetText();
            String nombreNuevo = context.GetChild(5).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreViejo);
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla " + nombreViejo + " a la cual se le desea cambiar nombre. \n";
                return error;
            }
            existeTabla = miControl.existeTabla(nombreNuevo);
            if (existeTabla)
            {
                mensajeError += "Ya existe una tabla con el nombre " + nombreNuevo + ".\n";
                return error;
            }
            miControl.cambiarNombreTabla(nombreViejo, nombreNuevo);
            return "void";
        }

        public override string VisitAccionTabla_AddConstraint(gramSQLParser.AccionTabla_AddConstraintContext context)
        {
            constraintContenido = true;
            String retorno = Visit(context.GetChild(1));
            if (retorno.Equals(error))
            {
                return error;
            }
            constraintContenido = false;
            return "void"; 

        }

        public override string VisitExpresionOrden1(gramSQLParser.ExpresionOrden1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitListaUpdate1(gramSQLParser.ListaUpdate1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitFloat_literal(gramSQLParser.Float_literalContext context)
        {
            return "float";
        }

        public override string VisitExpBooleana4_parentesis(gramSQLParser.ExpBooleana4_parentesisContext context)
        {
            return Visit(context.GetChild(1));
        }

        public override string VisitCreate_Database(gramSQLParser.Create_DatabaseContext context)
        {
            
            String nombreDB = context.GetChild(2).GetText();
            Boolean existe = miControl.existeDB(nombreDB);
            if (!existe)
            {
                miControl.agregarDB(nombreDB);
                return "void";
            }
            else
            {
                mensajeError += "Ya existe la base de datos " + nombreDB + ".\n";
                return error;
            }

        }

        public override string VisitDeclaracionColumnas2_comita(gramSQLParser.DeclaracionColumnas2_comitaContext context)
        {
            String nombreCol = context.GetChild(0).GetChild(0).GetText();
            String tipoCol = Visit(context.GetChild(0));
            Columna colTemp = new Columna();
            colTemp.setNombre(nombreCol);
            colTemp.setTipo(tipoCol);

            Boolean existeCol = tablaNueva.existeColumna(nombreCol);
            if (existeCol)
            {
                mensajeError += "La columna " + nombreCol + " ya ha sido especificada.\n";
                return error;
            }

            tablaNueva.agregarColumna(colTemp);

            
            String regreso = Visit(context.GetChild(2));
            if (regreso.Equals(error))
            {
                return error;
            }

            return "void";
        }

        public override string VisitExpBooleana_and(gramSQLParser.ExpBooleana_andContext context)
        {
            String retorno1 = Visit(context.GetChild(0));
            if (retorno1.Equals(error))
            {
                return error;
            }
            String retorno2 = Visit(context.GetChild(2));
            if (retorno2.Equals(error))
            {
                return error;
            }
            String nuevoRetorno = retorno1 + " " + retorno2 + " AND";
            return nuevoRetorno;
        }

        public override string VisitDate_literal(gramSQLParser.Date_literalContext context)
        {
            String año = context.GetChild(1).GetText();
            String mes = context.GetChild(3).GetText();
            String dia = context.GetChild(5).GetText();
            if (dia.Length == 1)
            {
                dia = 0 + dia;
            }
            if (mes.Length == 1)
            {
                mes = 0 + mes;
            }
            String fecha = dia + '-' + mes + '-' + año;
            try
            {
                DateTime date = DateTime.ParseExact(fecha, "dd-MM-yyyy", null);
            }
            catch (FormatException)
            {
                mensajeError += "Error en la linea: " + context.Start + ", Error: La fecha: " + fecha + " no es valida.\n";
                return error;
            }
            return "date";
        }

        public override string VisitExpression(gramSQLParser.ExpressionContext context)
        {
            String retorno = Visit(context.GetChild(0));
            return retorno;
        }

        public override string VisitRelOperator(gramSQLParser.RelOperatorContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitListaColumna2_comita(gramSQLParser.ListaColumna2_comitaContext context)
        {
            String nombreColumna = context.GetChild(0).GetText();
            Boolean existeColumna = miControl.existeColumna(refNombreTabla, nombreColumna);
            if (!existeColumna)
            {
                mensajeError += "No existe una columna de nombre " + nombreColumna + ".\n";
                return error;
            }
            nombresCol.Add(nombreColumna);

            String retorno = Visit(context.GetChild(2));
            if (retorno.Equals(error))
            {
                return error;
            }
            return "void";

            
        }

        public override string VisitListaTablaColumna2_comita(gramSQLParser.ListaTablaColumna2_comitaContext context)
        {
            //lista de columnas
            if (conTabla)
            {
                String nombreColumna = Visit(context.GetChild(0));
                if (nombreColumna.Equals(error))
                {
                    return error;
                }

                nombresCol.Add(nombreColumna);
            }
            //lista tablas
            else
            {
                String nombreTabla = Visit(context.GetChild(0));
                if (nombreTabla.Equals(error))
                {
                    return error;
                }
                nombresTabla.Add(nombreTabla);
            }

            String retorno = Visit(context.GetChild(2));
            if (retorno.Equals(error))
            {
                return error;
            }

            return "void";
        }

        public override string VisitExpBooleana3_expBooleana4(gramSQLParser.ExpBooleana3_expBooleana4Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitDeclaracionColumnas1(gramSQLParser.DeclaracionColumnas1Context context)
        {
            return Visit(context.GetChild(0));
            
        }

        public override string VisitExpBooleana2_expBooleana3(gramSQLParser.ExpBooleana2_expBooleana3Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitColumnaDatos_literal(gramSQLParser.ColumnaDatos_literalContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitIdComa(gramSQLParser.IdComaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitIdComa2_comita(gramSQLParser.IdComa2_comitaContext context)
        {
            String idNombre = context.GetChild(0).GetText();
            Boolean idRepetido = false;
            Boolean columnaExiste = false;
            Boolean esPrimary = false;
            if (TipoConstraint == 1)
            {
                idRepetido = tempPC.existeIdCol(idNombre);
                if (!idRepetido)
                {
                    columnaExiste = tablaNueva.existeColumna(idNombre);
                    if (columnaExiste)
                    {
                        tempPC.agregarPK(idNombre);
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la llave.\n";
                        return error;
                    }
                    
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en la llave primaria.\n";
                    return error;
                }

            }

            else if (TipoConstraint == 2)
            {
                idRepetido = tempFC.existeIdCol(idNombre);
                if (!idRepetido)
                {
                    columnaExiste = tablaNueva.existeColumna(idNombre);
                    if (columnaExiste)
                    {
                        tempFC.agregarFK(idNombre);
                        
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la tabla actual.\n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en la llave foranea.\n";
                    return error;
                }

            }

            else if (TipoConstraint == 3)
            {
                idRepetido = tempFC.existeRefCol(idNombre);
                if (!idRepetido)
                {

                    columnaExiste = miControl.existeColumna(refNombreTabla, idNombre);
                    if (columnaExiste)
                    {
                        esPrimary = miControl.columnaEnPrimaryK(refNombreTabla,idNombre);
                        if (esPrimary)
                        {
                            tempFC.agregarFK(idNombre);
                        }
                        else
                        {
                            mensajeError += "La columna " + idNombre + " debe ser primaria.\n";
                            return error;
                        }
                    }
                    else
                    {
                        mensajeError += "La columna " + idNombre + " no existe en la tabla "+refNombreTabla+".\n";
                        return error;
                    }
                }
                else
                {
                    mensajeError += "El id " + idNombre + " se repite en las referencias.\n";
                    return error;
                }

            }

            String regreso = Visit(context.GetChild(2));
            if (regreso.Equals(error))
            {
                return error;
            }

            return "void";
            
        }

        public override string VisitDeleteExpression(gramSQLParser.DeleteExpressionContext context)
        {
            String nombreTabla = context.GetChild(2).GetText();

            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla: " + nombreTabla + ".\n";
                return error;
            }

            //Llamar metodo Delete
            int filasDelete;
            String retorno;
            if (context.ChildCount == 3)
            {
                filasDelete = miControl.DeleteFilas(nombreTabla);
                if (miControl.falloEliminacion)
                {
                    mensajeError += miControl.mensajeFallo;
                    numeroDelete = numeroDelete + filasDelete;
                    mensajeDelete = "Delete " + numeroDelete + " con exito. \n";
                    return error;
                }
                numeroDelete = numeroDelete + filasDelete;
                mensajeDelete = "Delete " + numeroDelete + " con exito. \n";
            }
            else
            {
                refNombreTabla = nombreTabla;
                expConstraint = true;
                expWhere = true;
                retorno = Visit(context.GetChild(4));
                expConstraint = false;
                expWhere = false;
                if (retorno.Equals(error))
                {
                    return error;
                }
                List<string> whereElements = retorno.Split(new char[] { ' ' }).ToList();
                filasDelete = miControl.DeleteFilas(nombreTabla, whereElements);
                if(miControl.falloEliminacion){
                    mensajeError += miControl.mensajeFallo;
                    numeroDelete = numeroDelete + filasDelete;
                    mensajeDelete = "Delete " + numeroDelete + " con exito. \n";
                    return error;
                }
                numeroDelete = numeroDelete + filasDelete;
                mensajeDelete = "Delete " + numeroDelete + " con exito. \n";

            }
            return "void";

        }

        public override string VisitExpBooleana_or(gramSQLParser.ExpBooleana_orContext context)
        {
            String retorno1 = Visit(context.GetChild(0));
            if (retorno1.Equals(error))
            {
                return error;
            }
            String retorno2 = Visit(context.GetChild(2));
            if (retorno2.Equals(error))
            {
                return error;
            }
            String nuevoRetorno = retorno1 + " " + retorno2 + " OR";
            return nuevoRetorno;

        }

        public override string VisitAlterExpression_accion(gramSQLParser.AlterExpression_accionContext context)
        {
            String nombreTabla = context.GetChild(2).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (miControl.getDBActual().Equals(""))
            {
                mensajeError += "No ha seleccionado una base de datos en la cual trabajar.\n";
                return error;
            }
            if (!existeTabla)
            {
                mensajeError += "No existe la tabla " + nombreTabla + " a la cual se le desea realizar la accion. \n";
                return error;
            }
            tablaNueva = new Tabla();
            tablaNueva = miControl.obtenerTabla(nombreTabla);
            String retorno = Visit(context.GetChild(3));
            if (retorno.Equals(error))
            {
                return error;
            }
            miControl.sustituirTabla(nombreTabla, tablaNueva);
            return "void";
        }

        public override string VisitExpBooleana4_relacion(gramSQLParser.ExpBooleana4_relacionContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitDeclaracionConstraint2_declaracion(gramSQLParser.DeclaracionConstraint2_declaracionContext context)
        {
            return Visit(context.GetChild(0));
            
        }

        //public string Visit(Antlr4.Runtime.Tree.IParseTree tree)
        //{
        //    throw new NotImplementedException();
        //}

        //public string VisitChildren(Antlr4.Runtime.Tree.IRuleNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public string VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public string VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
