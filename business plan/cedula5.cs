using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Globalization;


namespace business_plan
{
    public partial class cedula5 : Form
    {
        #region variables conexion

        private MySqlConnection Conn, ConnCipsis;
        private string query;
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        private string conexion2 = "SERVER=10.10.1.76; DATABASE=cipsis; user=root; PASSWORD=zaptorre;";
        //private string conexion = "SERVER=localhost; DATABASE=cipsis; user=root; PASSWORD=;";
        //private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";

        #endregion variables conexion

        #region variables globales
        string idsucursal = "Total";
        string querypromedio_ctasXpagar = "",querypromedio_ctasXcobrar="";
        private string[] idd = new string[1000];
        private string[,] provedor =new string[1,1];
        private string[,] marca =new string[1,1];
        private DateTime ejercicio = DateTime.Now;
        double enero = 0, febrero = 0, marzo = 0, abril = 0, mayo = 0, junio = 0, julio = 0, agosto = 0, septiembre = 0, octubre = 0, noviembre = 0, diciembre = 0, saldoAcum = 0;
        private string marc = "",prov="",nump="";
        string nombre = "";
        double[] rotacion = new double[1000];
        double[] VTI=new double[1000];
        double diasINv = 0;
        DateTime FechaAI = DateTime.Now;
        DateTime FechaAF = DateTime.Now;
        double DPMA = 0;
        double[] promedio=new double[1000];
        #endregion variables globales

        public cedula5()
        {
            InitializeComponent();
        }

        private void cedula5_Load(object sender, EventArgs e)
        {
            #region Abrir conexion cipsis

            ConnCipsis = new MySqlConnection(conexion2);
            try
            {
                ConnCipsis.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion Abrir conexion cipsis

            #region Abrir conexion dwh

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

            #region Colorear Datagrid
            //dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#B4FF8F");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#33D633");
            dgvCed1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            #endregion
            #region Colorear Datagrid
            //dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvced5b.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#B4FF8F");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvced5b.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#33D633");
            dgvced5b.CellBorderStyle = DataGridViewCellBorderStyle.None;
            #endregion
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            
        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvCed1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbestructura3_TextChanged(object sender, EventArgs e)
        {
            string SeleccionActual = cbestructura3.Text;
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

        private void cbestructura4_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbestructura4.Text;
            comboBox2.Items.Clear();
            #region total
            if (SeleccionActual == "Total")
            {
                dgvced5b.Rows.Clear();
                dgvced5b.Rows.Add();
                dgvced5b.Rows[0].Cells[0].Value = "Total";
                query = "SELECT * FROM escenarios where estructura='"+cbestructura3.Text+"' AND estructura2='"+cbestructura4.Text+"';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region sucursal
            if (SeleccionActual == "Sucursal")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idsucursal"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region division
            if (SeleccionActual == "Division")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["iddivisiones"].ToString();

                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Departamento
            if (SeleccionActual == "Departamento")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,iddepto from estdepartamento;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["iddepto"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Familia
            if (SeleccionActual == "Familia")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idfamilia from estfamilia;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idfamilia"].ToString();

                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Linea
            if (SeleccionActual == "Linea")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idlinea from estlinea;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idlinea"].ToString();

                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 1
            if (SeleccionActual == "Linea 1")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idl1 from estl1;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl1"].ToString();

                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 2
            if (SeleccionActual == "Linea 2")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idl2 from estl2;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl2"].ToString();

                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 3
            if (SeleccionActual == "Linea 3")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idl3 from estl3;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl3"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 4
            if (SeleccionActual == "Linea 4")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idl4 from estl4;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl4"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 5
            if (SeleccionActual == "Linea 5")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip,idl5 from estl5;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl5"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region linea 6
            if (SeleccionActual == "Linea 6")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT distinct descrip, idl6 from estl6;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["idl6"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
            #region Marca
            if (SeleccionActual == "Marca")
            {
                dgvced5b.Rows.Clear();
                query = "SELECT marca, descrip  from marca;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvced5b.Rows.Add();
                    dgvced5b.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["marca"].ToString();
                    i++;
                }
                reader.Close();
                query = "SELECT * FROM escenarios where estructura='" + cbestructura3.Text + "' AND estructura2='" + cbestructura4.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Escenario"].ToString());
                }
                reader.Close();
            }
            else
            { }
            #endregion
        }

        private void cbEstructura_TextChanged_1(object sender, EventArgs e)
        {
            //dgvCed1.Rows.Clear();
            //cbEstructura2.Items.Clear();
            //cbEstructura2.Items.Add("Total");
            //int i = 0;
            //if (cbEstructura.Text != "Total")
            //{
            //    cbEstructura2.Show();
            //    query = "SELECT p.`proveedor`, p.`raz_soc` AS provedor ,m.`marca`, m.`descrip` FROM saldoprov AS S INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE p.raz_soc='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, ConnCipsis);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        dgvCed1.Rows[0].Cells[0].Value = reader["proveedor"].ToString();
            //        dgvCed1.Rows[0].Cells[1].Value = reader["proveedor"].ToString();
            //    }

            //    reader.Close();

            //    query = "SELECT p.`proveedor`, p.`raz_soc` AS provedor ,m.`marca`, m.`descrip` FROM saldoprov AS S INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE p.`raz_soc`='" + cbEstructura.Text + "';";
            //    cmd = new MySqlCommand(query, ConnCipsis);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        cbEstructura2.Items.Add(reader["descrip"].ToString());
            //    }
            //    reader.Close();
            //}
            //if (cbEstructura.Text == "Total")
            //{
            //    cbEstructura2.Hide();
            //    query = "SELECT p.`proveedor`, p.`raz_soc` AS provedor ,m.`marca`, m.`descrip` FROM saldoprov AS S INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca`;";
            //    cmd = new MySqlCommand(query, ConnCipsis);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        dgvCed1.Rows.Add();
            //        dgvCed1.Rows[i].Cells[0].Value = reader["proveedor"].ToString();
            //        dgvCed1.Rows[i].Cells[1].Value = reader["provedor"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //    i = 0;
            //    ////////////////////////////////////////////////////////////////////
            //    query = "SELECT p.`proveedor`, p.`raz_soc` AS provedor ,m.`marca`, m.`descrip` FROM saldoprov AS S INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca`;";
            //    cmd = new MySqlCommand(query, ConnCipsis);
            //    reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        //dgvCed1.Rows[i].Cells[0].Value = reader["proveedor"].ToString();
            //        dgvCed1.Rows[i].Cells[2].Value = reader["descrip"].ToString();
            //        i++;
            //    }
            //    reader.Close();
            //}
        }

        private void cbEstructura2_TextChanged_1(object sender, EventArgs e)
        {
            //int i = 0;
            //if (cbEstructura2.Text == "Total")
            //{
            //    dgvCed1.Rows[0].Cells[2].Value = "Total";
            //    idd[0] = "";
            //}
            //else
            //    if (cbEstructura2.Text != "Total")
            //    {
            //        dgvCed1.Rows[0].Cells[2].Value = cbEstructura2.Text;
            //        idd[0] = "AND m.`descrip`='" + cbEstructura2.Text + "';";
            //    }
            //    else { }
        }

        private void btnSimular_Click_1(object sender, EventArgs e)
        {
            ejercicio = DateTime.Parse(dtpFechaEjercicio.Value.ToString());
            int j = 0;
            for (int i = 0; i <= dgvCed1.Rows.Count - 1; i++)
            {
                #region cueros
                try
                {
                    if (comboBox3.Text == "Total")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT saldoact AS saldoact, enero AS enero1,febrero AS febrero1, marzo AS marzo1,abril AS abril1, mayo AS mayo1, junio AS junio1, julio AS julio1, agosto AS agosto1,septiembre AS septiembre1, octubre AS octubre1, noviembre AS noviembre1, diciembre AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "';";
                        }
                        else
                        {
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto =0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if(j<dgvCed1.RowCount-1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Sucursal")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + " AND V.IDSUCURSAL='" + idd[i] + "';";

                        }
                        #endregion
                        #region llenardgv
                        //cmd = new MySqlCommand(query, Conn);
                        //reader = cmd.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //    if (reader["prom"].ToString() == "")
                        //    {
                        //        PVunit = 0;
                        //    }
                        //    else
                        //    {
                        //        PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                        //    }
                        //}

                        //reader.Close();
                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Division")
                    {
                            #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.iddivisiones='" + idd[i] + "';";
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Departamento")
                    {
                            #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.iddepto='" + idd[i] + "';";
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Familia")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idfamilia='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idlinea='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 1")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl1='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 2")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl2='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 3")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl3='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 4")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl4='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 5")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl5='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (comboBox3.Text == "Linea 6")
                    {
                        #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.idl6='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                        if (comboBox3.Text == "Marca")
                        {
                            #region query
                        query = "SELECT saldoact AS saldoact, SUM(enero) AS enero1,SUM(febrero) AS febrero1, SUM(marzo) AS marzo1,SUM(abril) AS abril1, SUM(mayo) AS mayo1, SUM(junio) AS junio1, SUM(julio) AS julio1, SUM(agosto) AS agosto1,SUM(septiembre) AS septiembre1, SUM(octubre) AS octubre1, SUM(noviembre) AS noviembre1, SUM(diciembre) AS diciembre1 FROM saldoprovfactorajeart AS S LEFT JOIN estarticulo AS E ON S.`idarticulo`=E.`idarticulo` INNER JOIN prov AS p ON S.`idproveedor`=p.`proveedor` INNER JOIN marca AS m ON S.`marca`=m.`marca` WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "' AND E.marca='" + idd[i] + "';";
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, ConnCipsis);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["saldoact"].ToString() != "")
                            {
                                enero = double.Parse(reader["enero1"].ToString());
                                febrero = double.Parse(reader["febrero1"].ToString());
                                marzo = double.Parse(reader["marzo1"].ToString());
                                abril = double.Parse(reader["abril1"].ToString());
                                mayo = double.Parse(reader["mayo1"].ToString());
                                junio = double.Parse(reader["junio1"].ToString());
                                julio = double.Parse(reader["julio1"].ToString());
                                agosto = double.Parse(reader["agosto1"].ToString());
                                septiembre = double.Parse(reader["septiembre1"].ToString());
                                octubre = double.Parse(reader["octubre1"].ToString());
                                noviembre = double.Parse(reader["noviembre1"].ToString());
                                diciembre = double.Parse(reader["diciembre1"].ToString());
                                saldoAcum = double.Parse(reader["saldoact"].ToString());
                            }
                            else
                            {
                                enero = 0;
                                febrero = 0;
                                marzo = 0;
                                abril = 0;
                                mayo = 0;
                                junio = 0;
                                julio = 0;
                                agosto = 0;
                                septiembre = 0;
                                octubre = 0;
                                noviembre = 0;
                                diciembre = 0;
                                saldoAcum = 0;
                            }
                            #region Mostrar en dgvCed1
                            if (j < dgvCed1.RowCount - 1)
                            {
                                dgvCed1.Rows[j].Cells[1].Value = enero.ToString("C2");
                                dgvCed1.Rows[j].Cells[2].Value = febrero.ToString("C2");
                                dgvCed1.Rows[j].Cells[3].Value = marzo.ToString("C2");
                                dgvCed1.Rows[j].Cells[4].Value = abril.ToString("C2");
                                dgvCed1.Rows[j].Cells[5].Value = mayo.ToString("C2");
                                dgvCed1.Rows[j].Cells[6].Value = junio.ToString("C2");
                                dgvCed1.Rows[j].Cells[7].Value = julio.ToString("C2");
                                dgvCed1.Rows[j].Cells[8].Value = agosto.ToString("C2");
                                dgvCed1.Rows[j].Cells[9].Value = septiembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[10].Value = octubre.ToString("C2");
                                dgvCed1.Rows[j].Cells[11].Value = noviembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[12].Value = diciembre.ToString("C2");
                                dgvCed1.Rows[j].Cells[13].Value = saldoAcum.ToString("C2");
                            }
                            j++;
                            #endregion Mostrar en dgvCed1
                        }
                        reader.Close();

                        #endregion
                        }
                        else { }
                    }
                 catch (Exception y)
                {
                    MessageBox.Show("Error " + y);
                }
                #endregion
                try
                {
                    #region Mostrar en dgvCed1
                    //dgvCed1.Rows[i].Cells[1].Value = enero.ToString("C2");
                    //dgvCed1.Rows[i].Cells[2].Value = febrero.ToString("C2");
                    //dgvCed1.Rows[i].Cells[3].Value = marzo.ToString("C2");
                    //dgvCed1.Rows[i].Cells[4].Value = abril.ToString("C2");
                    //dgvCed1.Rows[i].Cells[5].Value = mayo.ToString("C2");
                    //dgvCed1.Rows[i].Cells[6].Value = junio.ToString("C2");
                    //dgvCed1.Rows[i].Cells[7].Value = julio.ToString("C2");
                    //dgvCed1.Rows[i].Cells[8].Value = agosto.ToString("C2");
                    //dgvCed1.Rows[i].Cells[9].Value = septiembre.ToString("C2");
                    //dgvCed1.Rows[i].Cells[10].Value = octubre.ToString("C2");
                    //dgvCed1.Rows[i].Cells[11].Value = noviembre.ToString("C2");
                    //dgvCed1.Rows[i].Cells[12].Value = diciembre.ToString("C2");
                    //dgvCed1.Rows[i].Cells[13].Value = saldoAcum.ToString("C2");
                    #endregion Mostrar en dgvCed1
                }
                catch (Exception z)
                {
                    MessageBox.Show("Error " + z);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbEscenario.Clear();
            tbnombre.Clear();
            dgvCed1.Rows.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            nombre = tbEscenario.Text;
            string EscenarioN = "0";
            double mes1 = 0, mes2 = 0, mes3 = 0, mes4 = 0, mes5 = 0, mes6 = 0, mes7 = 0, mes8 = 0, mes9 = 0, mes10 = 0, mes11 = 0, mes12 = 0, saldo = 0;
            try
            {
                #region comprobar nombre

                query = "SELECT nombre from cedula5a where nombre='" + nombre + "'";
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
            if (EscenarioN == tbEscenario.Text)
            {
                DialogResult boton = MessageBox.Show("Desea modificar el esenario previamente guardado?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (boton == DialogResult.OK)
                {
                    for (int i = 0; i <= dgvCed1.Rows.Count - 1; i++)
                    {
                        if (dgvCed1.Rows[i].Cells[1].Value != null)
                        {
                            #region actualizar

                            mes1 = double.Parse(dgvCed1.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                            mes2 = double.Parse(dgvCed1.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                            mes3 = double.Parse(dgvCed1.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                            mes4 = double.Parse(dgvCed1.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                            mes5 = double.Parse(dgvCed1.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                            mes6 = double.Parse(dgvCed1.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                            mes7 = double.Parse(dgvCed1.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                            mes8 = double.Parse(dgvCed1.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                            mes9 = double.Parse(dgvCed1.Rows[i].Cells[11].Value.ToString(), NumberStyles.Currency);
                            mes10 = double.Parse(dgvCed1.Rows[i].Cells[12].Value.ToString(), NumberStyles.Currency);
                            mes11 = double.Parse(dgvCed1.Rows[i].Cells[13].Value.ToString(), NumberStyles.Currency);
                            mes12 = double.Parse(dgvCed1.Rows[i].Cells[14].Value.ToString(), NumberStyles.Currency);
                            saldo = double.Parse(dgvCed1.Rows[i].Cells[15].Value.ToString(), NumberStyles.Currency);

                            query = "UPDATE cedula5a SET mes1="+mes1.ToString()+",mes2="+mes2+",mes3="+mes3+",mes4="+mes4+",mes5="+mes5+",mes6="+mes6+",mes7="+mes7+",mes8="+mes8+",mes9="+mes9+",mes10="+mes10+",mes11="+mes11+",mes12="+mes12+",saldoAcumulado="+saldo+" WHERE nombre='"+tbEscenario.Text+"';";
                            cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();
                            #endregion actualizar
                        }
                    }
                    MessageBox.Show("actualizado");
                }
                else
                {
                    tbEscenario.Clear();
                    tbEscenario.Focus();
                }
            }
            else
            {
                #region Insertar registros

                for (int i = 0; i <= dgvCed1.Rows.Count -1 ; i++)
                {
                    if (dgvCed1.Rows[i].Cells[0].Value != null)
                    {
                    mes1=double.Parse(dgvCed1.Rows[i].Cells[3].Value.ToString(),NumberStyles.Currency);
                    mes2 = double.Parse(dgvCed1.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                    mes3 = double.Parse(dgvCed1.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                    mes4 = double.Parse(dgvCed1.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                    mes5 = double.Parse(dgvCed1.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                    mes6 = double.Parse(dgvCed1.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                    mes7 = double.Parse(dgvCed1.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                    mes8 = double.Parse(dgvCed1.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                    mes9 = double.Parse(dgvCed1.Rows[i].Cells[11].Value.ToString(), NumberStyles.Currency);
                    mes10 = double.Parse(dgvCed1.Rows[i].Cells[12].Value.ToString(), NumberStyles.Currency);
                    mes11 = double.Parse(dgvCed1.Rows[i].Cells[13].Value.ToString(), NumberStyles.Currency);
                    mes12 = double.Parse(dgvCed1.Rows[i].Cells[14].Value.ToString(), NumberStyles.Currency);
                    saldo = double.Parse(dgvCed1.Rows[i].Cells[15].Value.ToString(), NumberStyles.Currency);
                    
                        query = "INSERT INTO  cedula5a (nombre,provedor,marca,mes1,mes2,mes3,mes4,mes5,mes6,mes7,mes8,mes9,mes10,mes11,mes12,saldoAcumulado) VALUES('" + tbEscenario.Text + "','" + dgvCed1.Rows[i].Cells[1].Value.ToString() + "','" + dgvCed1.Rows[i].Cells[2].Value.ToString() + "',"+mes1+","+mes2+","+mes3+","+mes4+","+mes5+","+mes6+","+mes7+","+mes8+","+mes9+","+mes10+","+mes11+","+mes12+","+saldo+");";
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
            tbEscenario.Clear();
            dgvCed1.Rows.Clear();
        }

        private void btnsimular2_Click(object sender, EventArgs e) //--------Floating---------//
        {
            for (int i = 0; i <= dgvCed1.Rows.Count - 1; i++)
            {
                if (dgvCed1.Rows[i].Cells[0].Value != null)
                {
                    try
                    {
                        #region obtener Fecha anterior inicial

                        //Fechainicial = dtpFechainicial.Text;
                        //query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechainicial + "';";
                        //cmd = new MySqlCommand(query, Conn);
                        //reader = cmd.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //    FechaAI = DateTime.Parse(reader["FechaAnterior"].ToString());
                        //}
                        //reader.Close();

                        #endregion obtener Fecha anterior inicial

                        #region Obtener Fecha anterior final

                        //Fechafinal = dtpFechafinal.Text;
                        //query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechafinal + "';";
                        //cmd = new MySqlCommand(query, Conn);
                        //reader = cmd.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //    FechaAF = DateTime.Parse(reader["FechaAnterior"].ToString());
                        //}
                        //reader.Close();

                        #endregion Obtener Fecha anterior final
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Error con las fechas " + x);
                    }
                        #region querys
                    //try
                    //{
                    if (comboBox3.Text == "Total")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region rotacion cuentas por pagar 
                            querypromedio_ctasXpagar = "SELECT (SUM(saldoact)+SUM(saldoant))/2 AS promedio FROM saldoprovfactorajeart AS S INNER JOIN estarticulo AS E ON S.idarticulo=E.idarticulo WHERE E.fecha BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'";
                            #endregion
                            #region plazo medio de cobros
                            querypromedio_ctasXpagar = "SELECT (SUM(total)+SUM(apagar))/2 AS promedio FROM factprov AS P LEFT JOIN estarticulo AS E ON P.proveedor=E.proveedor WHERE E.fecha BETWEEN '"+FechaAI.ToString("yyyy-MM-dd")+"' AND '"+FechaAF.ToString("yyyy-MM-dd")+"'";
                            #endregion
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                            }
                            else
                            {
                            }
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    //    if (cbEstructura2.Text == "Sucursal")
                    //    {
                    //        #region query
                    //        if (idsucursal == "Total")
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + " AND V.IDSUCURSAL='" + idd[i] + "';";

                    //        }
                    //        #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Division")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddivisiones=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Departamento")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddepto=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddepto=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Familia")
                    //    {

                    //        #region query
                    //        if (idsucursal == "Total")
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idfamilia=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idfamilia=" + idd[i] + ";";
                    //        }
                    //        #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Linea")
                    //    {

                    //        #region query
                    //        if (idsucursal == "Total")
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idlinea=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idlinea=" + idd[i] + ";";
                    //        }
                    //        #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Linea 1")
                    //    {

                    //        #region query
                    //        if (idsucursal == "Total")
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";

                    //        }
                    //        #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "l2")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "l3")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "l4")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "l5")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "l6")
                    //    {
                    //        if (idsucursal == "Total")
                    //        {
                    //            #region query
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";

                    //        }
                    //            #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //    if (cbEstructura2.Text == "Marca")
                    //    {

                    //        #region query
                    //        if (idsucursal == "Total")
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                    //        }
                    //        else
                    //        {
                    //            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                    //        }
                    //        #endregion
                    //        #region llenardgv
                    //        cmd = new MySqlCommand(query, Conn);
                    //        reader = cmd.ExecuteReader();
                    //        while (reader.Read())
                    //        {
                    //            if (reader["prom"].ToString() == "")
                    //            {
                    //                PVunit = 0;
                    //            }
                    //            else
                    //            {
                    //                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                    //            }
                    //        }

                    //        reader.Close();
                    //        #endregion
                    //        reader.Close();
                    //    }
                    //    else { }
                    //}
                    //catch (Exception y)
                    //{
                    //    MessageBox.Show("Error " + y);
                    //}
                    #endregion
                    try
                    {
                        #region operaciones
                        TimeSpan dias = FechaAF.Subtract(FechaAI);
                        DPMA=double.Parse(dias.Days.ToString())/rotacion[i];
                        #endregion
                        #region Mostrar en dgvCed1
                        dgvCed1.Rows[i].Cells[1].Value = DPMA.ToString("0,0");
                        //dgvCed1.Rows[i].Cells[4].Value = PrP.ToString("00,0");
                        //dgvCed1.Rows[i].Cells[5].Value = PVunit.ToString("C0");
                        //dgvCed1.Rows[i].Cells[6].Value = VmP.ToString("C0");
                        //dgvCed1.Rows[i].Cells[7].Value = VmI.ToString("C0");
                        //dgvCed1.Rows[i].Cells[8].Value = VdP.ToString();
                        //dgvCed1.Rows[i].Cells[9].Value = VdI.ToString("C0");
                        //dgvCed1.Rows[i].Cells[10].Value = DI.ToString();
                        #endregion Mostrar en dgvCed1
                    }
                    catch (Exception z)
                    {
                        MessageBox.Show("Error " + z);
                    }
                }
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            query = "SELECT * FROM escenarios WHERE Escenario='" + comboBox2.Text + "';";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rotacion[i] = double.Parse(reader["RID"].ToString());
                VTI[i] = double.Parse(reader["VTI"].ToString());
                FechaAI = DateTime.Parse(reader["PeriodoI"].ToString());
                FechaAF = DateTime.Parse(reader["PeriodoF"].ToString());
                i++;
            }
            reader.Close();
            formargrid(FechaAI,FechaAF);
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = comboBox3.Text;
            #region total
            if (SeleccionActual == "Total")
            {
                dgvCed1.Rows.Clear();
                dgvCed1.Rows.Add();
                dgvCed1.Rows[0].Cells[0].Value = "Total";
            }
            else
            { }
            #endregion
            #region sucursal
            if (SeleccionActual == "Sucursal")
            {
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,iddepto from estdepartamento;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idfamilia from estfamilia;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idlinea from estlinea;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idl1 from estl1;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idl2 from estl2;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idl3 from estl3;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idl4 from estl4;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip,idl5 from estl5;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT distinct descrip, idl6 from estl6;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed1.Rows.Clear();
                query = "SELECT marca, descrip  from marca;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["marca"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
        }

        private void formargrid(DateTime fecha1, DateTime fecha2)
        {
            int c = 0, i = 1;
            c = Math.Abs((fecha1.Month - fecha2.Month) + 12 * (fecha1.Year - fecha2.Year));
            dgvced5b.ColumnCount = (c*7)+1;
            dgvced5b.ColumnHeadersVisible = true;
            int j = c;
            int m = 1;
            while (j != 0)
            {
                dgvced5b.Columns[i].Name = "Dias que permanece mercancia en almacen mes "+m;
                dgvced5b.Columns[i + 1].Name = "Rotacion de cuentas por pagar mes "+m;
                dgvced5b.Columns[i + 2].Name = "Plazo medio de pagos mes "+m;
                dgvced5b.Columns[i + 3].Name = "Plazo medio de cobros mes "+m;
                dgvced5b.Columns[i + 4].Name = "Dias financiados mes "+m;
                dgvced5b.Columns[i + 5].Name = "Importe dias financiados mes "+m;
                dgvced5b.Columns[i + 6].Name = "Utilidad de dias financiados mes "+m;
                j--;
                i = i + 7;
                m++;
            }
        }

    }
}