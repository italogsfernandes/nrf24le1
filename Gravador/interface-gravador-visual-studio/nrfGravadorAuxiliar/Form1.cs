using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nrfGravadorAuxiliar
{
    public partial class Form1 : Form
    {
        public program_data codigo_atual;
        public program_data codigo_lido;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Auxiliar de Gravador do nrf24le1";
            user_log("Aplicação Iniciada.");
            openFileDialog1.Title = "Selecione o arquivo .hex gerado pelo Keil.";
            codigo_atual = new program_data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user_log("Abrindo Arquivo");
            openFileDialog1.ShowDialog();
        }


        public void user_log(string mensagem)
        {
            richTextBox1.Text = "Status: " + "\n" + System.DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + " - "
                + mensagem + richTextBox1.Text.Substring(8);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {          
            user_log("Arquivo selecionado: " + openFileDialog1.FileName.ToString());
            // Open the file for reading.
            System.IO.StreamReader file =
                System.IO.File.OpenText(openFileDialog1.FileName);

            string str_codigo_hex = file.ReadToEnd();

            codigo_atual.carregar_arquivo(str_codigo_hex);

            user_log("Lido:\n" + str_codigo_hex.Substring(0,12) + "\n[...]\n" + str_codigo_hex.Substring(str_codigo_hex.Length - 12,12));
            user_log("Arquivo lido com sucesso.");

            richTextBox2.Clear();

            export_vector(codigo_atual);
            //show_code(codigo_atual);
            // Set the MainWindow Title to the filename.
            this.Text = "Auxiliar de Gravador do nrf24le1 -- " + openFileDialog1.FileName.ToString();
            file.Close(); // Close the file so as to not leave a mess.

        }

        private void export_vector(program_data codigo)
        {
            string str_out = "***********************************************************************************\n" +
                        "Copie o seguinte código em seu programa em C: \n\n" +
                        "uint8_t hex_code[" + codigo.hex_code.Length.ToString() + "] = {\n";
            for (int i = 0; i < Math.Ceiling((float)(codigo.hex_code.Length) / 16); i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (codigo.hex_code[i * 16 + j] < 16)
                    {
                        str_out += "0x0" + codigo.hex_code[i * 16 + j].ToString("X");
                    }
                    else
                    {
                        str_out += "0x" + codigo.hex_code[i * 16 + j].ToString("X");
                    }
                    if (j == 15 && i == Math.Ceiling((float)(codigo.hex_code.Length) / 16) - 1)
                    {
                        str_out += " };";
                    }
                    else
                    {
                        str_out += ", ";
                    }
                }
                str_out += "\n";
            }
            str_out += "\n***********************************************************************************\n";
            richTextBox2.Text = str_out;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text =
    "Software desenvolvido para realizar o upload de codigos" +
    "\n do tipo .hex para o transponder nrf24le1." +
    "\n Este codigo é apenas uma versão simplificada contendo"+
    "\n apenas a funcionalidade de ler um arquivo .hex e gerar um" +
    "\n array em linguagem c contendo as informações deste arquivo." +
    "\n" +
    "\n No caso do PIC este vetor pode ser copiado e colocado no"+
    "\n código dele." +
    "\n" +
    "\n Foi desenvolvido como parte de um programa que," +
    "\n junto ao arduino due devidademente configurado," +
    "\n é capaz de gravar códigos em CIs nrf24le1." +
    "\n Para ativar esta opção utilize a versão em Gtk-sharp (nrf-Burner)." +
    "\n link: https://github.com/italogfernandes/ltad-propinsitu/tree/master/GUI%20-%20Gravador%20do%20nrf" +
    "\n" +
    "\n Desenvolvido por Italo Fernandes em 2016\n" +
    "\n GitHub:  https://github.com/italogfernandes" +
    "\n Contato: italogsfernandes@gmail.com";
        }
    }
}
