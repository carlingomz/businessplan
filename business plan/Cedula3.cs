using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace business_plan
{
    public partial class Cedula3 : Form
    {
        #region variables conexion
        MySqlConnection Conn;
        string query;
        MySqlCommand cmd;
        MySqlDataReader reader;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        #endregion
        #region variables globales
        string[] idd = new string[1000];
        #endregion
        public Cedula3()
        {
            InitializeComponent();
            #region Abrir conexion
            Conn = new MySqlConnection(conexion);
            try
            {
                Conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cedula3_Load(object sender, EventArgs e)
        {
            #region Colorear Datagrid
            dgvCed3.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#2882ff");
            dgvCed3.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#abcdef");
            dgvCed3.CellBorderStyle = DataGridViewCellBorderStyle.None;
            #endregion
        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
