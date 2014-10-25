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
    public partial class cedula6 : Form
    {
        #region variables conexion

        private MySqlCommand cmd;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        //private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";
        private MySqlConnection Conn;
        private string query;
        private MySqlDataReader reader;
        #endregion variables conexion
        #region variables_globales

        private int contador = 0;
        private string escenario = "0";
        private string[] idd = new string[1000];
        string idsucursal = "Total";
        #endregion variables_globales

        public cedula6()
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

            #endregion Abrir conexion
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbEstructura2.Text;
            #region
            //cbEscenarios.Items.Clear();
            //string SeleccionActual = cbEstructura2.Text;
            //string SeleccionSuc = cbEstructura.Text;
            //int i = 0;

            //#region Totales estructura

            //#region total

            //if (SeleccionActual == "Total")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Total";
            //    //cbEscenarios.Items.Clear();

            //    //#region cargar combo Escenario

            //    //query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    //cmd = new MySqlCommand(query, Conn);
            //    //reader = cmd.ExecuteReader();
            //    //while (reader.Read())
            //    //{
            //    //    cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    //}
            //    //reader.Close();

            //    //#endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion total

            //#region sucursal

            //if (SeleccionActual == "Sucursal")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idsucursal"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion sucursal

            //#region division

            //if (SeleccionActual == "Division")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["iddivisiones"].ToString();

            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion division

            //#region Departamento

            //if (SeleccionActual == "Departamento")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,iddepto from estdepartamento;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["iddepto"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion Departamento

            //#region Familia

            //if (SeleccionActual == "Familia")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idfamilia from estfamilia;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idfamilia"].ToString();

            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion Familia

            //#region Linea

            //if (SeleccionActual == "Linea")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idlinea from estlinea;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idlinea"].ToString();

            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion Linea

            //#region linea 1

            //if (SeleccionActual == "Linea 1")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl1 from estl1;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl1"].ToString();

            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion linea 1

            //#region linea 2

            //if (SeleccionActual == "Linea 2")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl2 from estl2;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl2"].ToString();

            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion linea 2

            //#region linea 3

            //if (SeleccionActual == "Linea 3")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl3 from estl3;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl3"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion linea 3

            //#region linea 4

            //if (SeleccionActual == "Linea 4")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl4 from estl4;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl4"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion linea 4

            //#region linea 5

            //if (SeleccionActual == "Linea 5")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl5 from estl5;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl5"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion linea 5

            //#region linea6
            //if (SeleccionActual == "Linea 6")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip,idl6 from estl6;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl6"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion

            //#region marca
            //if (SeleccionActual == "Marca")
            //{
            //    dgvCed2.Rows.Clear();
            //    query = "SELECT distinct descrip, marca from marca;";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //cbEstructura.Items.Add(reader["descrip"].ToString());
            //        dgvCed2.Rows.Add();
            //        dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
            //        idd[i] = reader["idl6"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    cbEscenarios.Items.Clear();

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion
            //#endregion Totales estructura

            //#region sucursal * estructura

            //#region total

            //if (SeleccionActual != " " && SeleccionSuc != " ")
            //{
            //    cbEscenarios.Items.Clear();
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Total";

            //    #region cargar combo Escenario

            //    query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "' and estructura2='" + cbEstructura2.Text + "';";
            //    cmd = new MySqlCommand(query, Conn);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //    }
            //    reader.Close();

            //    #endregion cargar combo Escenario
            //}
            //else
            //{ }
            //#endregion total

            //#endregion sucursal * estructura
            #endregion
            #region total
            if (SeleccionActual == "Total")
            {
                dgvCed2.Rows.Clear();
                dgvCed2.Rows.Add();
                dgvCed2.Rows[0].Cells[0].Value = "Total";
            }
            else
            { }
            #endregion
            #region sucursal
            if (SeleccionActual == "Sucursal")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idsucursal"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region division
            if (SeleccionActual == "Division")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["iddivisiones"].ToString();

                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Departamento
            if (SeleccionActual == "Departamento")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,iddepto from estdepartamento;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["iddepto"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Familia
            if (SeleccionActual == "Familia")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idfamilia from estfamilia;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idfamilia"].ToString();

                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Linea
            if (SeleccionActual == "Linea")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idlinea from estlinea;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idlinea"].ToString();

                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 1
            if (SeleccionActual == "Linea 1")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idl1 from estl1;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl1"].ToString();

                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 2
            if (SeleccionActual == "Linea 2")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idl2 from estl2;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl2"].ToString();

                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 3
            if (SeleccionActual == "Linea 3")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idl3 from estl3;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl3"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 4
            if (SeleccionActual == "Linea 4")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idl4 from estl4;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl4"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 5
            if (SeleccionActual == "Linea 5")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip,idl5 from estl5;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl5"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 6
            if (SeleccionActual == "Linea 6")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT distinct descrip, idl6 from estl6;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl6"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Marca
            if (SeleccionActual == "Marca")
            {
                dgvCed2.Rows.Clear();
                query = "SELECT marca, descrip  from marca;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["marca"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
            string SeleccionActual = cbEstructura.Text;
            switch (SeleccionActual)
            {
                case "Total":
                    idsucursal = "Total";
                    break;
                case "Sucursal Juarez":
                    idsucursal = "(V.IDSUCURSAL='01')";
                    break;

                case "Sucursal Hidalgo":
                    idsucursal = "(V.IDSUCURSAL='02')";
                    break;
                case "Sucursal Triana":
                    idsucursal = "(V.IDSUCURSAL='06')";
                    break;
                case "Sucursal Lerdo":
                    idsucursal = "(V.IDSUCURSAL='07')";
                    break;
                case "Sucursal Matriz":
                    idsucursal = "(V.IDSUCURSAL='08')";
                    break;

            }
        }

        private void cedula6_Load(object sender, EventArgs e)
        {
            //dgvCed2.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvCed2.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#2882ff");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvCed2.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#abcdef");
            dgvCed2.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }
    }
}
