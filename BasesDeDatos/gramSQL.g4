grammar gramSQL;

/*
 * Parser Rules
 */
 //Palabras Reservadas
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
PRIMARY KEY: 'primary' | 'PRIMARY' | 'Primary';
FOREIGN KEY: 'foreign' | 'FOREIGN' | 'Foreign';
KEY: 'key' | 'KEY' | 'Key';
CHECK: 'check' | 'CHECK' | 'Check';
INT: 'int' | 'INT' | 'Int';
FLOAT: 'float' | 'FLOAT' | 'Float'
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
REFERENCES: 'references' | 'REFERENCES' | 'References';
WS : (' ' | '\n' | '\t'|COMMENTS)+ {skip();};
WSOPT : (' ' | '\n' | '\t')* {skip();};

fragment COMMENTS : '//' .*? '\r'? '\n';
fragment LETTER: [A-Z] | [a-z];
fragment DIGIT: [0-9];

ID : LETTER (LETTER | DIGIT)*;
NUM: DIGIT (DIGIT)*;
CHARACTER: '\'' ('\\\''|[ -~]|'\\"'|'\\t'|'\\n'|'\t'|'\\\\') '\'';


/*
 * Lexer Rules

 QUE HACER CON LOS ;
 QUE SI TIENE UNA SOGA PARA AHORCAR
 UNA O MAS VECES
 EXISTE TIPO CHAR?
 */

program: (expression)*;
expression: createExpression
		   | alterExpression 
		   | dropExpression
		   | showExpression
		   | useExpression;
createExpression: CREATE DATABASE ID #create_Database
				| CREATE TABLE ID '('(declaracionColumnas)+ CONSTRAINT (cConstraint)+ ')' #create_Table;
declaracionColumnas: ID tipo;
tipo: INT| CHAR | FLOAT | DATE
cConstraint: ID PRIMARY KEY '(' (ID)+')' #cConstraint_primary
			| ID FOREING KEY '(' (ID)+ ')'  REFERENCES ID '('  (ID)+ ')' #cConstraint_foreign
			| ID CHECK '(' expBooleana ')' #cConstraint_check;
alterExpression: ALTER DATABASE ID	RENAME TO ID
				| ALTER TABLE ID RENAME TO ID
				| ALTER TABLE accionTabla

