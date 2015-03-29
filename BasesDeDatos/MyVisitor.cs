using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class MyVisitor : gramSQLBaseVisitor<String>
    {
        //Variables
        String error = "Fate Stay Night";
        String mensajeError = "";
        ControlDirectorios miControl = new ControlDirectorios();
        Tabla tablaNueva = new Tabla();
        PrimaryConstraint tempPC = new PrimaryConstraint();
        ForeignConstraint tempFC = new ForeignConstraint();
        CheckConstraint tempCC = new CheckConstraint();
        String refNombreTabla = "";
        int TipoConstraint = 0;
        Boolean expConstraint = false;
        String expBooleanPostfix = "";

        public override string VisitAlterExpression_database(gramSQLParser.AlterExpression_databaseContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDropExpression_database(gramSQLParser.DropExpression_databaseContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitAccionTabla_DropColumn(gramSQLParser.AccionTabla_DropColumnContext context)
        {
            throw new NotImplementedException();
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
                return "char";
            }
        }

        public override string VisitDeclaracionColumnas(gramSQLParser.DeclaracionColumnasContext context)
        {
            return Visit(context.GetChild(1));
        }

        public override string VisitExpresionOrden2_comita(gramSQLParser.ExpresionOrden2_comitaContext context)
        {
            return ""; //aqui habia pegado algo que no era 
        }

        public override string VisitProgram(gramSQLParser.ProgramContext context)
        {
            Boolean terminar = false;
            int hijo = 0;
            String retorno = "";
            while (!terminar)
            {
                retorno = Visit(context.GetChild(hijo));
                if (retorno.Equals(error))
                {
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
            tempCC.setRestriccionExp(expBooleanPostfix);
            tablaNueva.chConstraint.Add(tempCC);
            return "void";


            //return "void";


        }

        public override string VisitInt_literal(gramSQLParser.Int_literalContext context)
        {
            return "int";
        }

        public override string VisitCreate_Table(gramSQLParser.Create_TableContext context)
        {
            Console.WriteLine(miControl.getDBActual());
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
            throw new NotImplementedException();
        }

        public override string VisitIdComa2_idComa(gramSQLParser.IdComa2_idComaContext context)
        {
            String idNombre = context.GetChild(0).GetText();
            Boolean idRepetido = false;
            Boolean columnaExiste = false;
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
                        tempFC.agregarRefCol(idNombre);
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
            throw new NotImplementedException();
        }

        public override string VisitIdComa1(gramSQLParser.IdComa1Context context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitColumnaDatos_id(gramSQLParser.ColumnaDatos_idContext context)
        {
            String nombreCol = context.GetChild(0).GetText();
            Boolean existeCol = tablaNueva.existeColumna(nombreCol);
            if (!existeCol)
            {
                mensajeError += "no existe la columna " + nombreCol + ". \n";
                return error;
            }
            String tipoCol = tablaNueva.tipoColumna(nombreCol);
            return tipoCol;
        }

        public override string VisitDropExpression_table(gramSQLParser.DropExpression_tableContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitListaValores2_comita(gramSQLParser.ListaValores2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitShowColumnsExpression(gramSQLParser.ShowColumnsExpressionContext context)
        {
            throw new NotImplementedException();
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
            tablaNueva.pConstraint.Add(tempPC);
            return "void";
        }

        public override string VisitShowExpression_Tables(gramSQLParser.ShowExpression_TablesContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionConstraint(gramSQLParser.DeclaracionConstraintContext context)
        {
            return Visit(context.GetChild(1));
            
        }

        public override string VisitListaValores2_valores(gramSQLParser.ListaValores2_valoresContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpresionOrden2_expresionOrden(gramSQLParser.ExpresionOrden2_expresionOrdenContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitListaColumna1(gramSQLParser.ListaColumna1Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitVarchar_literal(gramSQLParser.Varchar_literalContext context)
        {
            return "varchar";
        }

        public override string VisitListaValores1(gramSQLParser.ListaValores1Context context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string VisitExpRelacion(gramSQLParser.ExpRelacionContext context)
        {
            String retorno1 = context.GetChild(0).GetText();
            String retorno2 = context.GetChild(2).GetText();
            String opSimb = context.GetChild(1).GetText();
            String type1 = Visit(context.GetChild(0));
            String type2 = Visit(context.GetChild(2));
            if (opSimb.Equals("<") || opSimb.Equals(">") || opSimb.Equals("<=") || opSimb.Equals(">="))
            {
                if (!((type1.Equals("int") || type1.Equals("float")) && (type2.Equals("int") || type1.Equals("float"))))
                {
                    mensajeError += "No se puede comparar un " + type1 + " con un " + type2 + " usando el operador " + opSimb;
                    return error;
                }
            }
            else if (!(type1.Equals(type2)))
            {
                if (!((type1.Equals("int") || type1.Equals("float")) && (type2.Equals("int") || type1.Equals("float"))))
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
            throw new NotImplementedException();
        }

        public override string VisitNombreColumna(gramSQLParser.NombreColumnaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitValores(gramSQLParser.ValoresContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitUpdateExpression(gramSQLParser.UpdateExpressionContext context)
        {
            throw new NotImplementedException();
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
            Console.WriteLine(nombreDataBase);
            Console.WriteLine(miControl.getDBActual());
            return "void";

        }

        public override string VisitInsertExpression(gramSQLParser.InsertExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitColumnaDatos_referencia(gramSQLParser.ColumnaDatos_referenciaContext context)
        {
            if (expConstraint)
            {
                mensajeError += "No se puede referenciar columnas de otras tablas en una Constraint CHECK. \n";
                return error;
            }
            //codigo del WHERE
            String nombreTabla = context.GetChild(0).GetText();
            String nombreCol = context.GetChild(2).GetText();
            Boolean existeTabla = miControl.existeTabla(nombreTabla);
            if (!existeTabla)
            {
                mensajeError += "no existe una tabla de nombre " + nombreTabla + ". \n";
            }
            Boolean existeCol = miControl.existeColumna(nombreTabla, nombreCol);
            if (!existeCol)
            {
                mensajeError += "no existe la columna " + nombreCol + "en la tabla "+nombreTabla+". \n";
                return error;
            }
            String tipoCol = tablaNueva.tipoColumna(nombreCol);
            return tipoCol;

        }

        public override string VisitLiteral(gramSQLParser.LiteralContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitSelectExpression(gramSQLParser.SelectExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitListaColumna2_nombreColumna(gramSQLParser.ListaColumna2_nombreColumnaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitAlterExpression_table(gramSQLParser.AlterExpression_tableContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitAccionTabla_AddConstraint(gramSQLParser.AccionTabla_AddConstraintContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpresionOrden1(gramSQLParser.ExpresionOrden1Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitFloat_literal(gramSQLParser.Float_literalContext context)
        {
            return "float";
        }

        public override string VisitExpBooleana4_parentesis(gramSQLParser.ExpBooleana4_parentesisContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitCreate_Database(gramSQLParser.Create_DatabaseContext context)
        {
            miControl.inicializar();
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
            String fecha = dia + '-' + mes + '-' + año;
            try
            {
                DateTime date = DateTime.ParseExact(fecha, "MM-dd-yyyy", null);
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
            Console.WriteLine(retorno+" "+mensajeError);
            return retorno;
        }

        public override string VisitRelOperator(gramSQLParser.RelOperatorContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitListaColumna2_comita(gramSQLParser.ListaColumna2_comitaContext context)
        {
            throw new NotImplementedException();
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
                        tempFC.agregarRefCol(idNombre);
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
