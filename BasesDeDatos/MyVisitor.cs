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
        List<Columna> columnasYconstraints = new List<Columna>();


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
            throw new NotImplementedException();
        }

        public override string VisitExpresionOrden2_comita(gramSQLParser.ExpresionOrden2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitProgram(gramSQLParser.ProgramContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override string VisitExpBooleana_expBooleana2(gramSQLParser.ExpBooleana_expBooleana2Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionConstraint1(gramSQLParser.DeclaracionConstraint1Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitCConstraint_check(gramSQLParser.CConstraint_checkContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitInt_literal(gramSQLParser.Int_literalContext context)
        {
            return "int";
        }

        public override string VisitCreate_Table(gramSQLParser.Create_TableContext context)
        {
            String nombreTabla = context.GetChild(2).GetText();
            Visit(context.GetChild(4));
            Visit(context.GetChild(5));
            //Q NO SE REPITEN COLUMNAS EN UNA TABLA DE UNA DBZ
            List<Columna>  columnasTabla = new List<Columna>();
            columnasTabla = columnasYconstraints;
            Boolean existe = miControl.existeTabla(nombreTabla);
            if (!existe)
            {
                miControl.agregarTabla(nombreTabla, columnasTabla.Count, columnasTabla);
                return "void";
            }
            else
            {
                mensajeError += "Ya existe la tabla " + nombreTabla;
                return error;
            }
        }

        public override string VisitDeclaracionConstraint2_comita(gramSQLParser.DeclaracionConstraint2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitAccionTabla_AddColumn(gramSQLParser.AccionTabla_AddColumnContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitIdComa2_idComa(gramSQLParser.IdComa2_idComaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitCConstraint_foreign(gramSQLParser.CConstraint_foreignContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionColumnas2_declaracion(gramSQLParser.DeclaracionColumnas2_declaracionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpresionOrden(gramSQLParser.ExpresionOrdenContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitIdComa1(gramSQLParser.IdComa1Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitColumnaDatos_id(gramSQLParser.ColumnaDatos_idContext context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string VisitShowExpression_Tables(gramSQLParser.ShowExpression_TablesContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionConstraint(gramSQLParser.DeclaracionConstraintContext context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string VisitAccionTabla_DropConstraint(gramSQLParser.AccionTabla_DropConstraintContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpRelacion(gramSQLParser.ExpRelacionContext context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string VisitInsertExpression(gramSQLParser.InsertExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitColumnaDatos_referencia(gramSQLParser.ColumnaDatos_referenciaContext context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                mensajeError += "Ya existe la base de datos "+nombreDB;
                return error;
            }

        }

        public override string VisitDeclaracionColumnas2_comita(gramSQLParser.DeclaracionColumnas2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpBooleana_and(gramSQLParser.ExpBooleana_andContext context)
        {
            throw new NotImplementedException();
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
                mensajeError += "Error en la linea: " + context.Start + ", Error: La fecha: " + fecha + " no es valida";
                return error;
            }
            return "date";
        }

        public override string VisitExpression(gramSQLParser.ExpressionContext context)
        {
            return Visit(context.GetChild(0));
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
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionColumnas1(gramSQLParser.DeclaracionColumnas1Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpBooleana2_expBooleana3(gramSQLParser.ExpBooleana2_expBooleana3Context context)
        {
            throw new NotImplementedException();
        }

        public override string VisitColumnaDatos_literal(gramSQLParser.ColumnaDatos_literalContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitIdComa(gramSQLParser.IdComaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitIdComa2_comita(gramSQLParser.IdComa2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeleteExpression(gramSQLParser.DeleteExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpBooleana_or(gramSQLParser.ExpBooleana_orContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitAlterExpression_accion(gramSQLParser.AlterExpression_accionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitExpBooleana4_relacion(gramSQLParser.ExpBooleana4_relacionContext context)
        {
            throw new NotImplementedException();
        }

        public override string VisitDeclaracionConstraint2_declaracion(gramSQLParser.DeclaracionConstraint2_declaracionContext context)
        {
            throw new NotImplementedException();
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
