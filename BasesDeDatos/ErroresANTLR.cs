// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasesDeDatos
{
    class ErroresANTLR : IAntlrErrorListener<IToken>
    {
        Boolean iError = false;
        String errores = "";
        public Boolean getIError()
        {
            return this.iError;
        }

        public void setIError(Boolean iError)
        {
            this.iError = iError;
        }

        public String getErrores()
        {
            return this.errores;
        }

        public void setErrores(String errores)
        {
            this.errores = errores;
        }
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            iError = true;
            errores += "Error en la Linea: " + line + " En la posicion: " + charPositionInLine + " Error: " + msg + "\n";
        }
    }
}
