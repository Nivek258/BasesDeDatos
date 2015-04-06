﻿// Proyecto 1
// Kevin Avenaño - 12151
// Ernesto Solis - 12286

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasesDeDatos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            OpenFileDialog1.Filter = "txt files (*.txt)|*.txt";
            OpenFileDialog1.FilterIndex = 2;
            OpenFileDialog1.RestoreDirectory = true;
            textQuery.Text = "";

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = OpenFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    StreamReader myWriter = new StreamReader(myStream);
                    textQuery.Text += myWriter.ReadToEnd();
                    myWriter.Close();
                    myStream.Close();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    StreamWriter myWriter = new StreamWriter(myStream);
                    myWriter.WriteLine(textQuery.Text);
                    myWriter.Close();
                    myStream.Close();
                }
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            MyVisitor miVisitor = new MyVisitor();
            ErroresANTLR ea = new ErroresANTLR();
            AntlrInputStream input = new AntlrInputStream(textQuery.Text);
            gramSQLLexer lexer = new gramSQLLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            gramSQLParser parser = new gramSQLParser(tokens);
            parser.RemoveParseListeners();
            parser.AddErrorListener(ea);
            IParseTree tree = parser.program();
            textErrores.Text = "";

            if (ea.getIError() == false)
            {
                miVisitor.aMostrar = gridTabla;
                miVisitor.Visit(tree);
                //mostrar errores
                //mostrar tablas
                String errores = miVisitor.mensajeError;
                textErrores.Text = miVisitor.mensajeInsert+"\n";
                textErrores.Text += miVisitor.mensajeUpdate + "\n";
                textErrores.Text += miVisitor.mensajeDelete + "\n";
                textErrores.Text += miVisitor.mensajeSelect + " \n";
                textErrores.Text += errores;
                
            }
            else
            {
                textErrores.Text = ea.getErrores();
                ea.setIError(false);
                ea.setErrores("");
            }	
        }
    }
}
