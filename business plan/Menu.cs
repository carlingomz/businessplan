using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business_plan
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cedula1 c1 = new Cedula1();
            c1.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cedula_2 c2 = new Cedula_2();
            c2.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cedula3 c3 = new Cedula3();
            c3.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cedula4 c4 = new cedula4();
            c4.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cedula5 c5 = new cedula5();
            c5.Show();
            this.Close();
        }

        private void button1_Enter(object sender, EventArgs e)
        {

            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cedula1 c1 = new Cedula1();
            c1.Show();
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Cedula_2 c2 = new Cedula_2();
            c2.Show();
            this.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Cedula3 c3 = new Cedula3();
            c3.Show();
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            cedula4 c4 = new cedula4();
            c4.Show();
            this.Close();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            cedula5 c5 = new cedula5();
            c5.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cedula6 c6 = new cedula6();
            c6.Show();
            this.Close();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            richTextBox1.Text = ("Cedula 1") + System.Environment.NewLine;
            richTextBox1.Text += "Pronostico de ventas" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de nivel de inventario" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de rotacion" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de precio de venta unitario" + System.Environment.NewLine;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            string s = "Cedula 2";
            richTextBox1.Text += s.ToString()+System.Environment.NewLine;
            richTextBox1.Text += "Pronostico de ventas" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de nivel de inventario" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de rotacion" + System.Environment.NewLine;
            richTextBox1.Text += "Captura de precio de venta unitario" + System.Environment.NewLine;
        }

        private void button2_Leave(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
