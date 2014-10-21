using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace business_plan
{
    public partial class Log_in : Form
    {
        #region variables de conexion

        SqlConnection con = new SqlConnection("Data Source=DIOS;Initial Catalog=Prueba;Integrated Security=True;");
        #endregion
        public Log_in()
        {
            InitializeComponent();
            //#region abrir conexion
            //try
            //{
            //    con.Open();
            //}
            //catch (Exception x)
            //{
            //    Console.WriteLine(x.ToString());
            //}
            //#endregion
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String usr = "";
            string pswd = "";
            string usrx = "";
            string pswdx = "";
            usr = tbUser.Text;
            pswd = tbPwd.Text;
            //try
            //{
            //    SqlDataReader reader = null;
            //    SqlCommand cmd = new SqlCommand("select [user],[password] from [Prueba].[dbo].[Log_in] where [user] ='" + usr + "' and [password] = '" + pswd + "';", con);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        usrx=reader["user"].ToString();
            //        pswdx=reader["password"].ToString();
            //    }
            //    if(usr==usrx)
            //    {
            Menu m = new Menu();
            m.Show();
            this.Hide();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Usuario o contraseña incorrectos");
            //        tbPwd.Clear();
            //        tbUser.Clear();
            //        tbUser.Focus();
            //    }
            //    reader.Close();
            //}
            //catch (Exception x)
            //{
            //    MessageBox.Show("Error "+x.ToString());
            //}
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
