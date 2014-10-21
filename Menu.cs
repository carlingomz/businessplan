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
    }
}
