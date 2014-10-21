using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Windows.Forms;
using nmExcel = Microsoft.Office.Interop.Excel;

namespace business_plan
{
    public partial class Cedula_2 : Form
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
        private double importe = 0.00;
        private double porciento = 0.00;
        private double pp = 0.00;
        private double Vti = 0.00;
        #endregion variables_globales

        public Cedula_2()
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = "0";
            string categoria = " ";
            double porcientoAsig = 0.0;
            double AUnid = 0.00;
            double AImpo = 0.0;
            nombre = tbNombre.Text;
            string EscenarioN = "0";
            try
            {
                #region comprobar nombre

                query = "SELECT nombre from cedula2 where nombre='" + tbNombre.Text + "'";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EscenarioN = reader["nombre"].ToString();
                }
                reader.Close();

                #endregion comprobar nombre
            }
            catch (Exception x)
            {
                MessageBox.Show("Error " + x);
            }
            if (EscenarioN == tbNombre.Text)
            {
                DialogResult boton = MessageBox.Show("Desea modificar el esenario previamente guardado?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (boton == DialogResult.OK)
                {
                    for (int i = 0; i <= dgvCed2.Rows.Count - 1; i++)
                    {
                        if (dgvCed2.Rows[i].Cells[1].Value != null)
                        {
                            #region actualizar

                            categoria = dgvCed2.Rows[i].Cells[0].Value.ToString();
                            porcientoAsig = double.Parse(dgvCed2.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                            AUnid = double.Parse(dgvCed2.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                            AImpo = double.Parse(dgvCed2.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);

                            query = "UPDATE cedula2 set nombre='" + tbNombre.Text + "',Pasignacion=" + porcientoAsig.ToString() + ",AUnid=" + AUnid.ToString() + ",AImpo=" + AImpo.ToString() + " where nombre ='" + tbNombre.Text + "';";
                            cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();

                            #endregion actualizar
                        }
                    }
                    MessageBox.Show("actualizado");
                }
                else
                {
                    tbNombre.Clear();
                    tbNombre.Focus();
                }
            }
            else
            {
                #region Insertar registros

                for (int i = 0; i <= dgvCed2.Rows.Count - 1; i++)
                {
                    if (dgvCed2.Rows[i].Cells[3].Value != null)
                    {
                        categoria = dgvCed2.Rows[i].Cells[0].Value.ToString();
                        porcientoAsig = double.Parse(dgvCed2.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                        AUnid = double.Parse(dgvCed2.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                        AImpo = double.Parse(dgvCed2.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);

                        query = "INSERT INTO  cedula2 (nombre,categoria,Pasignacion,AUnid,AImpo,cedula1) VALUES('" + tbNombre.Text + "','" + categoria + "'," + porcientoAsig.ToString() + "," + AUnid.ToString() + "," + AImpo.ToString() + ",'"+cbEscenarios.Text+"');";
                        cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Guardado");

                #endregion Insertar registros
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            dgvCed2.Rows.Clear();
            tbNombre.Clear();
            cbEstructura.Items.Clear();

            #region cargar combo estructura

            cbEstructura.Items.Clear();
            cbEstructura.Items.Add("Total");
            cbEstructura.Items.Add("Sucursal");
            cbEstructura.Items.Add("Division");
            cbEstructura.Items.Add("Departamento");
            cbEstructura.Items.Add("Familia");
            cbEstructura.Items.Add("Linea");
            cbEstructura.Items.Add("Linea 1");
            cbEstructura.Items.Add("Linea 2");
            cbEstructura.Items.Add("Linea 3");
            cbEstructura.Items.Add("Linea 4");
            cbEstructura.Items.Add("Linea 5");
            cbEstructura.Items.Add("Linea 6");
            cbEstructura.Items.Add("Marca");

            #endregion cargar combo estructura
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            dgvCed2.Rows.Clear();

            #region cargar dgv con la primera estructura

            string matriz = "Matriz", triana = "Triana", juarez = "Juarez", hidalgo = "Hidalgo";
            dgvCed2.Rows.Add(4);
            dgvCed2.Rows[0].Cells[0].Value = matriz;
            dgvCed2.Rows[1].Cells[0].Value = triana;
            dgvCed2.Rows[2].Cells[0].Value = juarez;
            dgvCed2.Rows[3].Cells[0].Value = hidalgo;

            #endregion cargar dgv con la primera estructura
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            int contador = 0;
            for (int j = 0; j <= dgvCed2.Rows.Count - 1; j++)
            {
                contador = contador + int.Parse(dgvCed2.Rows[j].Cells[1].Value.ToString());
            }
            if (contador <= 100)
            {
                for (int i = 0; i <= dgvCed2.Rows.Count - 1; i++)
                {
                    if (dgvCed2.Rows[i].Cells[1].Value != null)
                    {
                        #region obtener escenario

                        query = "SELECT Pp,VTI from escenarios where escenario='" + cbEscenarios.Text + "' and Categoria='" + dgvCed2.Rows[i].Cells[0].Value.ToString() + "';";
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            //escenario = reader["Escenario"].ToString();
                            Vti = double.Parse(reader["VTI"].ToString());
                            pp = double.Parse(reader["Pp"].ToString());
                        }
                        reader.Close();

                        #endregion obtener escenario

                        porciento = double.Parse(dgvCed2.Rows[i].Cells[1].Value.ToString());
                        importe = (porciento * pp) / 100 + 1;
                        dgvCed2.Rows[i].Cells[2].Value = importe.ToString("C2");
                        importe = (porciento * Vti) / 100 + 1;
                        dgvCed2.Rows[i].Cells[3].Value = importe.ToString("C2");
                    }
                }
            }
            else
            {
                MessageBox.Show("El porsentaje es mayor a 100");
            }
        }

        private void button1_Click(object sender, EventArgs e) //Exportar a excell
        {
            if (dgvCed2Rep.Rows.Count >= 1)
            {
                nmExcel.Application Excelapp = new nmExcel.Application();
                Excelapp.Application.Workbooks.Add(Type.Missing);
                Excelapp.Columns.ColumnWidth = 13;
                for (int j2 = 0; j2 < dgvCed2Rep.ColumnCount; j2++)
                {
                    Excelapp.Cells[1, j2 + 1] = dgvCed2Rep.Columns[j2].HeaderText;
                    //Excelapp.Cells[1, j2 + 1].Font.Bold = true;
                }
                for (int i = 0; i < dgvCed2Rep.Rows.Count; i++)
                {
                    DataGridViewRow Fila = dgvCed2Rep.Rows[i];
                    for (int j = 0; j < Fila.Cells.Count; j++)
                    {
                        Excelapp.Cells[i + 2, j + 1] = Fila.Cells[j].Value;
                    }
                }
                // ---------- cuadro de dialogo para Guardar
                SaveFileDialog CuadroDialogo = new SaveFileDialog();
                CuadroDialogo.DefaultExt = "xlsx";
                CuadroDialogo.Filter = "xlsx file(*.xlsx)|*.xlsx";
                CuadroDialogo.AddExtension = true;
                CuadroDialogo.RestoreDirectory = true;
                CuadroDialogo.Title = "Guardar";
                CuadroDialogo.InitialDirectory = @"c:\";
                if (CuadroDialogo.ShowDialog() == DialogResult.OK)
                {
                    Excelapp.ActiveWorkbook.SaveCopyAs(CuadroDialogo.FileName);

                    Excelapp.ActiveWorkbook.Saved = true;

                    CuadroDialogo.Dispose();
                    CuadroDialogo = null;
                    Excelapp.Quit();
                    MessageBox.Show("Guardado");
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el documento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No existe información a exportar", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void cbEscenarios_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
            #region
            //int i = 0;
            //string SeleccionActual = cbEstructura.Text;

            //#region total

            //if (SeleccionActual == "Total")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Total";
            //}
            //else
            //{ }
            //#endregion total

            //#region juarez

            //if (SeleccionActual == "Juarez")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Juarez";
            //}
            //else
            //{ }
            //#endregion juarez

            //#region hidalgo

            //if (SeleccionActual == "Hidalgo")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Hidalgo";
            //}
            //else
            //{ }
            //#endregion hidalgo

            //#region triana

            //if (SeleccionActual == "Triana")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Triana";
            //}
            //else
            //{ }
            //#endregion triana

            //#region Matriz

            //if (SeleccionActual == "Matriz")
            //{
            //    dgvCed2.Rows.Clear();
            //    dgvCed2.Rows.Add();
            //    dgvCed2.Rows[0].Cells[0].Value = "Matriz";
            //}
            //else
            //{ }
            //#endregion Matriz
            #endregion
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

        private void cbModificar_TextChanged(object sender, EventArgs e)
        {
            dgvCed2.Rows.Clear();
            string escenario = "";
            int i = 0;
            escenario = cbModificar.Text;
            DialogResult boton = MessageBox.Show("Desea abrir el escenario previo Se borraran los progresos no guardados?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (boton == DialogResult.OK)
            {
                query = "SELECT * FROM cedula2 WHERE nombre='" + escenario + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tbNombre.Text = reader["nombre"].ToString();
                    dgvCed2.Rows.Add();
                    dgvCed2.Rows[i].Cells[0].Value = reader["Categoria"].ToString();
                    dgvCed2.Rows[i].Cells[1].Value = reader["Pasignacion"].ToString();
                    dgvCed2.Rows[i].Cells[2].Value = reader["AUnid"].ToString();
                    dgvCed2.Rows[i].Cells[3].Value = reader["AImpo"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            {
            }
        }

        private void cbRepo_DropDown(object sender, EventArgs e)
        {
            cbRepo.Items.Clear();
            query = "SELECT distinct nombre from cedula2;";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cbRepo.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();
            dgvCed2Rep.Rows.Clear();
        }

        private void cbRepo_TextChanged(object sender, EventArgs e)
        {
            string categoria = " ";
            double porcientoAsig = 0.0;
            double AUnid = 0.0;
            double AImpo = 0.0;
            dgvCed2Rep.Rows.Clear();
            query = "SELECT * FROM cedula2 WHERE nombre='" + cbRepo.Text + "';";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            int i = 0;
            // MessageBox.Show(query);
            while (reader.Read())
            {
                categoria = reader["categoria"].ToString();
                porcientoAsig = double.Parse(reader["Pasignacion"].ToString());
                AUnid = double.Parse(reader["AUnid"].ToString());
                AImpo = double.Parse(reader["AImpo"].ToString());
                dgvCed2Rep.Rows.Add();

                //dgvCed2Rep.Rows[i].Cells[0].Value=reader["categoria"].ToString();
                dgvCed2Rep.Rows[i].Cells[0].Value = categoria;
                dgvCed2Rep.Rows[i].Cells[1].Value = porcientoAsig.ToString("C2");
                dgvCed2Rep.Rows[i].Cells[2].Value = AUnid.ToString("C2");
                dgvCed2Rep.Rows[i].Cells[3].Value = AImpo.ToString("C2");

                i++;
            }
            reader.Close();
        }

        private void Cedula_2_Load(object sender, EventArgs e)
        {
            //dgvCed2.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvCed2.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#2882ff");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvCed2.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#abcdef");
            dgvCed2.CellBorderStyle = DataGridViewCellBorderStyle.None;

            #region cargar cbescenarios

            //---------------------------------//
            //query = "SELECT distinct Escenario from escenarios;";
            //cmd = new MySqlCommand(query, Conn);
            //reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    //escenario = reader["Escenario"].ToString();
            //    cbEscenarios.Items.Add(reader["Escenario"].ToString());
            //}
            //reader.Close();
            //---------------------------------//

            #endregion cargar cbescenarios

            #region cargar combo repo

            query = "SELECT distinct nombre from cedula2;";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cbRepo.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();

            #endregion cargar combo repo
        }
        private void chbModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (chbModificar.Checked == true)
            {
                cbModificar.Show();
                cbModificar.Items.Clear();
                try
                {
                    query = "SELECT DISTINCT  nombre FROM cedula2;";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cbModificar.Items.Add(reader["nombre"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error " + x);
                }
            }
            else
            {
                cbModificar.Hide();
            }
        }

        private void dgvCed2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dgvCed2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //dgvCed2.Rows[e.RowIndex].Cells[1].ErrorText = "";
            //if (e.ColumnIndex == 1)
            //{
            //    string c = dgvCed2.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    if (c != null && c != "" && c != " ")
            //    {
            //        contador += int.Parse(c);
            //        if (contador >= 100)
            //        {
            //            dgvCed2.Rows[e.RowIndex].Cells[1].ErrorText = "La suma de valores no puede ser mayor a 100";
            //        }
            //    }
            //    else
            //        c = "0";
            //}
        }

        private void dgvCed2_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void dgvCed2_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        private void cbEscenarios_DropDown(object sender, EventArgs e)
        {
            cbEscenarios.Items.Clear();
            #region cargar combo Escenario

            query = "SELECT distinct Escenario from escenarios where estructura='" + cbEstructura.Text + "' AND estructura2='" + cbEstructura2.Text + "';";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cbEscenarios.Items.Add(reader["Escenario"].ToString());
            }
            reader.Close();

            #endregion cargar combo Escenario

        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
 }
    
}