using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class MyVisitor : gramSQLBaseVisitor<String>
    {

        public string VisitAlterExpression_database(gramSQLParser.AlterExpression_databaseContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDropExpression_database(gramSQLParser.DropExpression_databaseContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAccionTabla_DropColumn(gramSQLParser.AccionTabla_DropColumnContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitTipo(gramSQLParser.TipoContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionColumnas(gramSQLParser.DeclaracionColumnasContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpresionOrden2_comita(gramSQLParser.ExpresionOrden2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitProgram(gramSQLParser.ProgramContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana_expBooleana2(gramSQLParser.ExpBooleana_expBooleana2Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionConstraint1(gramSQLParser.DeclaracionConstraint1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitCConstraint_check(gramSQLParser.CConstraint_checkContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitInt_literal(gramSQLParser.Int_literalContext context)
        {
            return "int";
        }

        public string VisitCreate_Table(gramSQLParser.Create_TableContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionConstraint2_comita(gramSQLParser.DeclaracionConstraint2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAccionTabla_AddColumn(gramSQLParser.AccionTabla_AddColumnContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitIdComa2_idComa(gramSQLParser.IdComa2_idComaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitCConstraint_foreign(gramSQLParser.CConstraint_foreignContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionColumnas2_declaracion(gramSQLParser.DeclaracionColumnas2_declaracionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpresionOrden(gramSQLParser.ExpresionOrdenContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitIdComa1(gramSQLParser.IdComa1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitColumnaDatos_id(gramSQLParser.ColumnaDatos_idContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDropExpression_table(gramSQLParser.DropExpression_tableContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaValores2_comita(gramSQLParser.ListaValores2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitShowColumnsExpression(gramSQLParser.ShowColumnsExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitCConstraint_primary(gramSQLParser.CConstraint_primaryContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitShowExpression_Tables(gramSQLParser.ShowExpression_TablesContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionConstraint(gramSQLParser.DeclaracionConstraintContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaValores2_valores(gramSQLParser.ListaValores2_valoresContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpresionOrden2_expresionOrden(gramSQLParser.ExpresionOrden2_expresionOrdenContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaColumna1(gramSQLParser.ListaColumna1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitVarchar_literal(gramSQLParser.Varchar_literalContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaValores1(gramSQLParser.ListaValores1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana3_not(gramSQLParser.ExpBooleana3_notContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAccionTabla_DropConstraint(gramSQLParser.AccionTabla_DropConstraintContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpRelacion(gramSQLParser.ExpRelacionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitShowExpression_Databases(gramSQLParser.ShowExpression_DatabasesContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitNombreColumna(gramSQLParser.NombreColumnaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitValores(gramSQLParser.ValoresContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitUpdateExpression(gramSQLParser.UpdateExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitUseExpression(gramSQLParser.UseExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitInsertExpression(gramSQLParser.InsertExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitColumnaDatos_referencia(gramSQLParser.ColumnaDatos_referenciaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitLiteral(gramSQLParser.LiteralContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitSelectExpression(gramSQLParser.SelectExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaColumna2_nombreColumna(gramSQLParser.ListaColumna2_nombreColumnaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAlterExpression_table(gramSQLParser.AlterExpression_tableContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAccionTabla_AddConstraint(gramSQLParser.AccionTabla_AddConstraintContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpresionOrden1(gramSQLParser.ExpresionOrden1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitFloat_literal(gramSQLParser.Float_literalContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana4_parentesis(gramSQLParser.ExpBooleana4_parentesisContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitCreate_Database(gramSQLParser.Create_DatabaseContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionColumnas2_comita(gramSQLParser.DeclaracionColumnas2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana_and(gramSQLParser.ExpBooleana_andContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDate_literal(gramSQLParser.Date_literalContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpression(gramSQLParser.ExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitRelOperator(gramSQLParser.RelOperatorContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitListaColumna2_comita(gramSQLParser.ListaColumna2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana3_expBooleana4(gramSQLParser.ExpBooleana3_expBooleana4Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionColumnas1(gramSQLParser.DeclaracionColumnas1Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana2_expBooleana3(gramSQLParser.ExpBooleana2_expBooleana3Context context)
        {
            throw new NotImplementedException();
        }

        public string VisitColumnaDatos_literal(gramSQLParser.ColumnaDatos_literalContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitIdComa(gramSQLParser.IdComaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitIdComa2_comita(gramSQLParser.IdComa2_comitaContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeleteExpression(gramSQLParser.DeleteExpressionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana_or(gramSQLParser.ExpBooleana_orContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitAlterExpression_accion(gramSQLParser.AlterExpression_accionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitExpBooleana4_relacion(gramSQLParser.ExpBooleana4_relacionContext context)
        {
            throw new NotImplementedException();
        }

        public string VisitDeclaracionConstraint2_declaracion(gramSQLParser.DeclaracionConstraint2_declaracionContext context)
        {
            throw new NotImplementedException();
        }

        public string Visit(Antlr4.Runtime.Tree.IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public string VisitChildren(Antlr4.Runtime.Tree.IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public string VisitErrorNode(Antlr4.Runtime.Tree.IErrorNode node)
        {
            throw new NotImplementedException();
        }

        public string VisitTerminal(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            throw new NotImplementedException();
        }
    }
}
