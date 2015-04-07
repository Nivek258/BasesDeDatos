// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

grammar gramSQL;


/*
 * Parser Rules
 */
 //PALABRAS RESERVADAS DDL
DATABASE: 'database' | 'DATABASE' | 'Database';
DATABASES: 'databases' | 'DATABASES' | 'Databases';
TABLE: 'table' | 'TABLE' | 'Table';
TABLES: 'table' | 'TABLES' | 'Tables';
CREATE: 'create' | 'CREATE' | 'Create';
ALTER: 'alter' | 'ALTER' | 'Alter';
DROP: 'drop' | 'DROP' | 'Drop';
SHOW: 'show' | 'SHOW' | 'Show';
USE: 'use' | 'USE' | 'use';
CONSTRAINT: 'constraint' | 'CONSTRAINT' | 'Constraint';
PRIMARY: 'primary' | 'PRIMARY' | 'Primary';
FOREIGN: 'foreign' | 'FOREIGN' | 'Foreign';
KEY: 'key' | 'KEY' | 'Key';
CHECK: 'check' | 'CHECK' | 'Check';
INT: 'int' | 'INT' | 'Int';
FLOAT: 'float' | 'FLOAT' | 'Float';
DATE: 'date' | 'DATE' | 'Date';
CHAR: 'char' | 'CHAR' | 'Char';
AND: 'and' | 'AND' | 'And';
OR: 'or' | 'OR' | 'Or';
NOT: 'not' | 'NOT' | 'Not';
RENAME: 'rename' | 'RENAME' | 'Rename';
TO: 'to' | 'TO' | 'To';
COLUMN: 'column' | 'COLUMN' | 'Column';
ADD: 'add' | 'ADD' | 'Add';
COLUMNS: 'columns' | 'COLUMNS' | 'Columns';
FROM: 'from' | 'FROM' | 'From';
REFERENCES: 'references' | 'REFERENCES' | 'References';
//PALABRAS RESERVADAS DML
INSERT: 'insert' | 'INSERT'| 'Insert';
INTO: 'into' | 'INTO' | 'Into';
VALUES: 'values' | 'VALUES' | 'Values';
NULL: 'null' | 'NULL' | 'Null';
UPDATE: 'update' | 'UPDATE' | 'Update';
SET: 'set' | 'SET' | 'Set';
WHERE: 'where' | 'WHERE' | 'where';
DELETE: 'delete' | 'DELETE' | 'Delete';
SELECT: 'select' | 'SELECT' | 'Select';
ORDER: 'order' | 'ORDER' | 'Order';
BY: 'by' | 'BY' | 'By';
ASC: 'asc' | 'ASC' | 'Asc';
DESC: 'desc' | 'DESC' | 'Desc';



WS : (' ' | '\n' | '\t'|COMMENTS)+   -> channel(HIDDEN) ;
WSOPT : (' ' | '\n' | '\t')*   -> channel(HIDDEN);

fragment COMMENTS : '//' .*? '\r'? '\n';
fragment LETTER: [A-Z] | [a-z];
fragment DIGIT: [0-9];

ID : LETTER (LETTER | DIGIT)*;
NUM: DIGIT (DIGIT)*;
CHARACTER: '\'' ('\\\''|[ -~]|'\\"'|'\\t'|'\\n'|'\t'|'\\\\') '\'';
CHARACTER2:   '\'' ('\\\''|[ -&]|[(-~]|'\\"'|'\\t'|'\\n'|'\t'|'\\\\')* '\'';



/*
 * Lexer Rules

 QUE HACER CON LOS ;

 */

// GRAMATICA PARTE DDL
program: (expression ';')+;
expression: createExpression 
		   | alterExpression 
		   | dropExpression
		   | showExpression
		   | useExpression
		   | showColumnsExpression
		   | insertExpression
		   | updateExpression
		   | deleteExpression
		   | selectExpression;
createExpression: CREATE DATABASE ID #create_Database
				| CREATE TABLE ID '('declaracionColumnas1  declaracionConstraint1? ')' #create_Table;
declaracionColumnas1: declaracionColumnas2;
declaracionColumnas2: declaracionColumnas ',' declaracionColumnas2 #declaracionColumnas2_comita
					  | declaracionColumnas                        #declaracionColumnas2_declaracion;
declaracionColumnas: ID tipo;
declaracionConstraint1: declaracionConstraint2;
declaracionConstraint2: declaracionConstraint ',' declaracionConstraint2 #declaracionConstraint2_comita
					  | declaracionConstraint                       #declaracionConstraint2_declaracion;
declaracionConstraint: CONSTRAINT cConstraint;
cConstraint: ID PRIMARY KEY '(' idComa1')' #cConstraint_primary
			| ID FOREIGN KEY '(' idComa1 ')'  REFERENCES ID '('  idComa1 ')' #cConstraint_foreign
			| ID CHECK '(' expBooleana ')' #cConstraint_check;
tipo: INT| CHAR '(' NUM ')' | FLOAT | DATE;

idComa1: idComa2;
idComa2: idComa ',' idComa2    #idComa2_comita
		| idComa                #idComa2_idComa;
idComa: ID;
expBooleana: expBooleana OR expBooleana2     #expBooleana_or
			| expBooleana2                  #expBooleana_expBooleana2; 
expBooleana2: expBooleana2 AND expBooleana3   #expBooleana_and
			|expBooleana3                    #expBooleana2_expBooleana3;
expBooleana3: expBooleana4                  #expBooleana3_expBooleana4
			| NOT expBooleana4              #expBooleana3_not;
expBooleana4: expRelacion                  #expBooleana4_relacion 
			| '(' expBooleana')'			#expBooleana4_parentesis;
expRelacion: columnaDatos relOperator columnaDatos;
columnaDatos: literal  #columnaDatos_literal
			| ID       #columnaDatos_id
			| ID'.'ID  #columnaDatos_referencia;
relOperator: '<' | '>' | '<=' | '>=' | '<>' | '=';
literal: int_literal | varchar_literal | date_literal | float_literal;
int_literal: NUM;
varchar_literal: CHARACTER2 ;
date_literal: '\'' NUM '-' NUM '-' NUM  '\'';
float_literal: NUM'.'NUM;

alterExpression: ALTER DATABASE ID	RENAME TO ID      #alterExpression_database
				| ALTER TABLE ID RENAME TO ID         #alterExpression_table
				| ALTER TABLE ID accionTabla             #alterExpression_accion;
accionTabla: ADD COLUMN ID TIPO declaracionConstraint1?  #accionTabla_AddColumn
			| ADD declaracionConstraint					#accionTabla_AddConstraint
			| DROP COLUMN ID							#accionTabla_DropColumn
			| DROP CONSTRAINT ID						#accionTabla_DropConstraint;
dropExpression: DROP DATABASE ID                        #dropExpression_database
			| DROP TABLE ID							#dropExpression_table;
showExpression: SHOW DATABASES							#showExpression_Databases
			| SHOW TABLES								#showExpression_Tables;
useExpression: USE DATABASE ID;
showColumnsExpression: SHOW COLUMNS FROM ID;

// GRAMATICA PARTE DML
insertExpression: INSERT INTO ID '('listaColumna1 ')' VALUES '('listaValores1 ')';
listaColumna1: listaColumna2;
listaColumna2: nombreColumna ',' listaColumna2        #listaColumna2_comita
				|nombreColumna                        #listaColumna2_nombreColumna;
nombreColumna: ID;
listaValores1: listaValores2;
listaValores2: valores ',' listaValores2			 #listaValores2_comita
				| valores                            #listaValores2_valores;
valores: literal; 
updateExpression: UPDATE ID SET listaUpdate1 ( WHERE expBooleana)?;
listaUpdate1: listaUpdate2;
listaUpdate2: igualacion ',' listaUpdate2			#listaUpdate2_comita
				| igualacion						#listaUpdate2_igualacion;
igualacion: ID '=' literal;
deleteExpression: DELETE FROM ID (WHERE expBooleana)?;
selectExpression: SELECT ( '*' | listaTablaColumna1 ) FROM listaTablaColumna1 (WHERE expBooleana)? (ORDER BY '[' expresionOrden1 ']')?;
listaTablaColumna1: listaTablaColumna2;
listaTablaColumna2: nombreTablaColumna ',' listaTablaColumna2        #listaTablaColumna2_comita
				|nombreTablaColumna                        #listaTablaColumna2_nombreColumna;
nombreTablaColumna: ID	#nombreTablaColumna_id
			| ID'.'ID	#nombreTablaColumna_referencia;
expresionOrden1: expresionOrden2;
expresionOrden2: expresionOrden ',' expresionOrden2    #expresionOrden2_comita
				| expresionOrden                       #expresionOrden2_expresionOrden;
expresionOrden: nombreTablaColumna (ASC|DESC)?;


