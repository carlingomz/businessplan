using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Windows.Forms;
using nmExcel = Microsoft.Office.Interop.Excel;

namespace business_plan
{
    public partial class Cedula1 : Form
    {
        #region variables conexion

        private MySqlCommand cmd;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        //private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";
        private MySqlConnection Conn;
        private string query;
        private MySqlDataReader reader;
        #endregion variables conexion

        #region variables globales

        private string Categoria = " ";
        private double DI = 0.00;
        private DateTime FechaAF = DateTime.Now;
        private DateTime FechaAI = DateTime.Now;
        private string FechaAnteriorInicial = " ", FechaAnteriorFinal = "";
        private string Fechainicial = "", Fechafinal = "";
        private int id = 0;
        private string[] idd = new string[1000];
        private double nInv = 0.00;
        private double PrP = 0.00;
        private double PVunit = 0.00;
        private double RinvD = 0.00;
        private double TasaInt = 0.00;
        private double VdI = 0.00;
        private double VdP = 0.00;
        private double VmI = 0.00;
        private double VmP = 0.00;
        private double VTI = 0.00;
        string valor1 = "";
        string valor2 = "";
        string valor3 = "";
        string valor4 = "";
        string valor5 = "";
        string valor6 = "";
        string valor7 = "";
        string valor8 = "";
        string valor9 = "";
        string valor10 = "";
        string valor0 = "";
        string idsucursal = "Total";
        #endregion variables globales

        public Cedula1()
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
            Conn.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Fechainicial = dtpFechainicial.Text;
            Fechafinal = dtpFechafinal.Text;
            string EscenarioN = "0";
            try
            {
                #region comprobar nombre

                query = "SELECT Escenario from Escenarios where Escenario='" + tbEscenario.Text + "'";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EscenarioN = reader["Escenario"].ToString();
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
                        if (dgvCed1.Rows[i].Cells[2].Value != null)
                        {
                            nInv = double.Parse(dgvCed1.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                            RinvD = double.Parse(dgvCed1.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                            VTI = double.Parse(dgvCed1.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                            PrP = double.Parse(dgvCed1.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                            PVunit = double.Parse(dgvCed1.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                            VmP = double.Parse(dgvCed1.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                            VmI = double.Parse(dgvCed1.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                            VdP = double.Parse(dgvCed1.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                            VdI = double.Parse(dgvCed1.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                            DI = double.Parse(dgvCed1.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                            TasaInt = double.Parse(tbInflacion.Text);

                            #region actualizar

                            query = "UPDATE escenarios SET NiVID=" + nInv.ToString() + ",RID=" + RinvD.ToString() + ",PP=" + PrP.ToString() + ",PrVpU=" + PVunit.ToString() + ",Vdp=" + VdP.ToString() + ",VMp=" + VmP.ToString() + ",Vmi=" + VmI.ToString() + ",Vdi=" + VdI.ToString() + ",DI=" + DI.ToString() + ",VTI=" + VTI.ToString() + ",Inflacion=" + TasaInt.ToString() + " where Escenario='" + tbEscenario.Text + "'";
                            cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();

                            #endregion actualizar
                        }
                    }
                    MessageBox.Show("actualizado");
                }
                else
                {
                    tbEscenario.Focus();
                }
            }
            else
            {
                try
                {
                    #region obtener Fecha anterior inicial

                    Fechainicial = dtpFechainicial.Text;
                    // MessageBox.Show(Fechainicial);
                    query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechainicial + "';";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FechaAI = DateTime.Parse(reader["FechaAnterior"].ToString());
                        //FechaAnteriorInicial = reader["FechaAnterior"].ToString();
                        //MessageBox.Show(FechaAI.ToString("yyyy-MM-dd"));
                        // MessageBox.Show(FechaAI.ToString("yyyy-MM-dd"));
                    }
                    reader.Close();

                    #endregion obtener Fecha anterior inicial

                    #region Obtener Fecha anterior final

                    Fechafinal = dtpFechafinal.Text;
                    query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechafinal + "';";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FechaAF = DateTime.Parse(reader["FechaAnterior"].ToString());
                        // MessageBox.Show(FechaAF.ToString("yyyy-MM-dd"));
                        //FechaAnteriorFinal = reader["FechaAnterior"].ToString();
                        //MessageBox.Show(FechaAnteriorFinal);
                    }
                    reader.Close();

                    #endregion Obtener Fecha anterior final
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error con las fechas " + x);
                }

                #region Insertar registros

                try
                {
                    for (int i = 0; i <= dgvCed1.Rows.Count - 1; i++)
                    {
                        if (dgvCed1.Rows[i].Cells[2].Value.ToString() != "")
                        {
                            FechaAnteriorInicial = dtpFechainicial.Text;
                            FechaAnteriorFinal = dtpFechafinal.Text;
                            Categoria = dgvCed1.Rows[i].Cells[0].Value.ToString();

                            nInv = double.Parse(dgvCed1.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                            RinvD = double.Parse(dgvCed1.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                            VTI = double.Parse(dgvCed1.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                            PrP = double.Parse(dgvCed1.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                            PVunit = double.Parse(dgvCed1.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                            VmP = double.Parse(dgvCed1.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                            VmI = double.Parse(dgvCed1.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                            VdP = double.Parse(dgvCed1.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                            VdI = double.Parse(dgvCed1.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                            DI = double.Parse(dgvCed1.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                            TasaInt = double.Parse(tbInflacion.Text);

                            query = "INSERT INTO  Escenarios (Escenario,Fecha,PeriodoI,PeriodoF,NivID,RID,Pp,PrVpU,VMp,Vmi,Vdp,Vdi,DI,VTI,Inflacion,estructura,estructura2,Categoria) VALUES('" + tbEscenario.Text + "','" + dtpEscenario.Text + "','" + FechaAI.ToString("yyyy-MM-dd") + "','" + FechaAF.ToString("yyyy-MM-dd") + "'," + nInv.ToString() + "," + RinvD.ToString() + "," + PrP.ToString() + "," + PVunit.ToString() + "," + VmP.ToString() + "," + VmI.ToString() + "," + VdP.ToString() + "," + VdI.ToString() + "," + DI.ToString() + "," + VTI.ToString() + "," + TasaInt.ToString() + ",'" + CbCategoria.Text + "','" + cbEstructura2.Text + "','"+dgvCed1.Rows[i].Cells[0].Value.ToString()+"');";
                            cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Guardado");
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error al guardar " + x.ToString());
                }

                #endregion Insertar registros
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e) //Nuevo Escenario
        {
            tbEscenario.Clear();
            dgvCed1.Rows.Clear();
            tbInflacion.Clear();
            dtpEscenario.Value = DateTime.Now;
            dtpFechafinal.Value = DateTime.Now;
            dtpFechainicial.Value = DateTime.Now;
            CbCategoria.Items.Clear();

            #region cargar combo estructura

            CbCategoria.Items.Clear();
            CbCategoria.Items.Add("Total");
            CbCategoria.Items.Add("Sucursal");
            CbCategoria.Items.Add("Division");
            CbCategoria.Items.Add("Departamento");
            CbCategoria.Items.Add("Familia");
            CbCategoria.Items.Add("Linea");
            CbCategoria.Items.Add("Linea 1");
            CbCategoria.Items.Add("Linea 2");
            CbCategoria.Items.Add("Linea 3");
            CbCategoria.Items.Add("Linea 4");
            CbCategoria.Items.Add("Linea 5");
            CbCategoria.Items.Add("Linea 6");
            CbCategoria.Items.Add("Marca");

            #endregion cargar combo estructura

            tbEscenario.Focus();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            dgvCed1.Rows.Clear();
            CbCategoria.Items.Clear();

            #region cargar combo estructura

            CbCategoria.Items.Clear();
            CbCategoria.Items.Add("Division");
            CbCategoria.Items.Add("Departamento");
            CbCategoria.Items.Add("Familia");
            CbCategoria.Items.Add("Linea");
            CbCategoria.Items.Add("Linea 1");
            CbCategoria.Items.Add("Linea 2");
            CbCategoria.Items.Add("Linea 3");
            CbCategoria.Items.Add("Linea 4");
            CbCategoria.Items.Add("Linea 5");
            CbCategoria.Items.Add("Linea 6");

            #endregion cargar combo estructura

            #region cargar dgv con la primera estructura

            string matriz = "Matriz", triana = "Triana", juarez = "Juarez", hidalgo = "Hidalgo";
            dgvCed1.Rows.Add(4);
            dgvCed1.Rows[0].Cells[0].Value = matriz;
            dgvCed1.Rows[1].Cells[0].Value = triana;
            dgvCed1.Rows[2].Cells[0].Value = juarez;
            dgvCed1.Rows[3].Cells[0].Value = hidalgo;

            #endregion cargar dgv con la primera estructura
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            // dgvRep.Rows.Clear();
            double cantidad = 0, precio = 0;
            string scantidad = "", sprecio = "";
            Fechainicial = dtpFechainicial.Text;
            Fechafinal = dtpFechafinal.Text;
            TasaInt = double.Parse(tbInflacion.Text);
            for (int i = 0; i <= dgvCed1.Rows.Count - 1; i++)
            {
                #region cueros
                try
                {
                    #region obtener Fecha anterior inicial

                    Fechainicial = dtpFechainicial.Text;
                    query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechainicial + "';";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FechaAI = DateTime.Parse(reader["FechaAnterior"].ToString());
                    }
                    reader.Close();

                    #endregion obtener Fecha anterior inicial

                    #region Obtener Fecha anterior final

                    Fechafinal = dtpFechafinal.Text;
                    query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechafinal + "';";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FechaAF = DateTime.Parse(reader["FechaAnterior"].ToString());
                    }
                    reader.Close();

                    #endregion Obtener Fecha anterior final
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error con las fechas " + x);
                }
                try
                {
                    if (cbEstructura2.Text == "Total")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
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
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }
                        reader.Close();

                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Sucursal")
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
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Division")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddivisiones=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Departamento")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddepto=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddepto=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "Familia")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idfamilia=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idfamilia=" + idd[i] + ";";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "Linea")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idlinea=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idlinea=" + idd[i] + ";";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "Linea 1")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";

                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "l2")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "l3")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "l4")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "l5")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "l6")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura2.Text == "Marca")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                        }
                        else
                        {
                            query = "SELECT ((SUM(impneto))/(SUM(ctdneta))) AS prom FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE  " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["prom"].ToString() == "")
                            {
                                PVunit = 0;
                            }
                            else
                            {
                                PVunit = Math.Round(double.Parse(reader["prom"].ToString()), 0);
                            }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                }
                catch (Exception y)
                {
                    MessageBox.Show("Error " + y);
                }
                #endregion
                #region caso 1
                if (dgvCed1.Rows[i].Cells[1].Value != null && dgvCed1.Rows[i].Cells[2].Value != null)
                {
                    nInv = int.Parse(dgvCed1.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                    RinvD = int.Parse(dgvCed1.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                    TimeSpan dias = FechaAF.Subtract(FechaAI);
                    int meses=(FechaAI.Month - FechaAF.Month) + 12 * (FechaAI.Year - FechaAF.Year);
                    TasaInt = (TasaInt / 100) + 1;

                    PrP = (nInv * RinvD);
                    VmI = ((PVunit * PrP) / (-1*meses))*TasaInt;
                    VdI = ((PrP * PVunit) / double.Parse(dias.Days.ToString()))*TasaInt;
                    VTI = (PrP * PVunit)*TasaInt;

                    PVunit = PVunit * TasaInt;
                    VmP = (PrP /(-1*meses));
                    VdP = Math.Round(PrP / double.Parse(dias.Days.ToString()));
                    DI = Math.Round(double.Parse(dias.Days.ToString()) / RinvD, 2);

                }
                else { }
                #endregion
                #region caso 2
                if (dgvCed1.Rows[i].Cells[2].Value != null && dgvCed1.Rows[i].Cells[3].Value != null)
                {
                    TasaInt = (TasaInt / 100) + 1;
                    PVunit = PVunit * TasaInt;
                    VTI = int.Parse(dgvCed1.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                    TimeSpan dias = FechaAF.Subtract(FechaAI);
                    int meses = (FechaAI.Month - FechaAF.Month) + 12 * (FechaAI.Year - FechaAF.Year);
                    RinvD = int.Parse(dgvCed1.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                    nInv = (VTI / PVunit)/RinvD;
                    PrP = Math.Round(nInv * RinvD, 0);

                    VmP = (PrP / (-1 * meses));
                    VdP = Math.Round(PrP / double.Parse(dias.Days.ToString()));
                    VmI = ((PVunit * PrP) / (-1 * meses)) * TasaInt;
                    VdI = Math.Round((PrP * PVunit) / double.Parse(dias.Days.ToString()));
                    VmI = PrP * PVunit / double.Parse(meses.ToString());
                    VdI = PrP / double.Parse(dias.Days.ToString());
                    DI = Math.Round(double.Parse(dias.Days.ToString()) / RinvD, 2);
                   
                }
                #endregion
                try
                {
                    #region Mostrar en dgvCed1
                    dgvCed1.Rows[i].Cells[1].Value=nInv.ToString("00,0");
                    dgvCed1.Rows[i].Cells[2].Value=RinvD.ToString();
                    dgvCed1.Rows[i].Cells[3].Value = VTI.ToString("C0");
                    dgvCed1.Rows[i].Cells[4].Value = PrP.ToString("00,0");
                    dgvCed1.Rows[i].Cells[5].Value = PVunit.ToString("C0");
                    dgvCed1.Rows[i].Cells[6].Value = VmP.ToString("00,0");
                    dgvCed1.Rows[i].Cells[7].Value = VmI.ToString("C0");
                    dgvCed1.Rows[i].Cells[8].Value = VdP.ToString();
                    dgvCed1.Rows[i].Cells[9].Value = VdI.ToString("C0");
                    dgvCed1.Rows[i].Cells[10].Value = DI.ToString();

                    #endregion Mostrar en dgvCed1
                }
                catch (Exception z)
                {
                    MessageBox.Show("Error " + z);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //Exportar a excel
        {
            if (dgvRep.Rows.Count >= 1)
            {
                nmExcel.Application Excelapp = new nmExcel.Application();
                Excelapp.Application.Workbooks.Add(Type.Missing);
                Excelapp.Columns.ColumnWidth = 13;
                for (int j2 = 0; j2 < dgvRep.ColumnCount; j2++)
                {
                    Excelapp.Cells[1, j2 + 1] = dgvRep.Columns[j2].HeaderText;
                    //Excelapp.Cells[1, j2 + 1].Font.Bold = true;
                }
                for (int i = 0; i < dgvRep.Rows.Count; i++)
                {
                    DataGridViewRow Fila = dgvRep.Rows[i];
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

        private void CbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CbCategoria_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbEstructura2.Text;
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

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbModificar_TextChanged(object sender, EventArgs e)
        {
            dgvCed1.Rows.Clear();
            string escenario = "";
            int i = 0;
            escenario = cbModificar.Text;
            DialogResult boton = MessageBox.Show("Desea abrir el escenario previo Se borraran los progresos no guardados?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (boton == DialogResult.OK)
            {
                query = "SELECT * FROM ESCENARIOS WHERE Escenario='" + escenario + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nInv = double.Parse(reader["NivID"].ToString());
                    RinvD = double.Parse(reader["RID"].ToString());
                    PrP = double.Parse(reader["Pp"].ToString());
                    PVunit = double.Parse(reader["PrVpU"].ToString());
                    VmP = double.Parse(reader["VMp"].ToString());
                    VdP = double.Parse(reader["Vdp"].ToString());
                    DI = double.Parse(reader["DI"].ToString());
                    VmI = double.Parse(reader["Vmi"].ToString());
                    VdI = double.Parse(reader["Vdi"].ToString());
                    VTI = double.Parse(reader["VTI"].ToString());
                    Categoria = reader["Categoria"].ToString();
                    TasaInt = double.Parse(reader["Inflacion"].ToString());

                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[i].Cells[0].Value = reader["Categoria"].ToString();
                    dgvCed1.Rows[i].Cells[1].Value = nInv.ToString("000,00");
                    dgvCed1.Rows[i].Cells[2].Value = RinvD.ToString();
                    dgvCed1.Rows[i].Cells[3].Value = VTI.ToString("000,00");
                    dgvCed1.Rows[i].Cells[4].Value = PrP.ToString("000,00");
                    dgvCed1.Rows[i].Cells[5].Value = PVunit.ToString();
                    dgvCed1.Rows[i].Cells[6].Value = VmP.ToString("00,00");
                    dgvCed1.Rows[i].Cells[7].Value = VmI.ToString("00,00");
                    dgvCed1.Rows[i].Cells[8].Value = VdP.ToString("00,00");
                    dgvCed1.Rows[i].Cells[9].Value = VdI.ToString("00,00");
                    dgvCed1.Rows[i].Cells[10].Value = DI.ToString();

                    tbInflacion.Text = reader["Inflacion"].ToString();

                    //CbCategoria.SelectedValue(reader["Categoria"].ToString());
                    //CbCategoria.SelectedIndex = CbCategoria.FindStringExact(reader["Estructura"].ToString());
                    tbEscenario.Text = reader["Escenario"].ToString();
                    DateTime Fecha = DateTime.Parse(reader["Fecha"].ToString());
                    DateTime PeriodoI = DateTime.Parse(reader["PeriodoI"].ToString());
                    DateTime PeriodoF = DateTime.Parse(reader["PeriodoF"].ToString());

                    //dtpEscenario.Value = Fecha;
                    dtpFechainicial.Value = PeriodoI;
                    dtpFechafinal.Value = PeriodoF;
                    i++;
                }
                reader.Close();
            }
            else
            {
            }
        }

        private void Cedula1_Load(object sender, EventArgs e)
        {
            //dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#B4FF8F");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#43BF43");
            dgvCed1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            tbEscenario.Focus();
            //dgvCed1.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADEBEB");
            dgvRep.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#B4FF8F");
            //dgvCed1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#9DC1C1");
            dgvRep.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#33D633");
            dgvRep.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }

        private void Cedula1_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void chbModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (chbModificar.Checked == true)
            {
                cbModificar.Show();
                cbModificar.Items.Clear();
                dtpEscenario.Hide();
                cbEstructura2.Hide();
                CbCategoria.Hide();
                try
                {
                    query = "SELECT DISTINCT  Escenario FROM ESCENARIOS;";
                    cmd = new MySqlCommand(query, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cbModificar.Items.Add(reader["Escenario"].ToString());
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
                cbEstructura2.Show();
                CbCategoria.Show();
                dtpEscenario.Show();
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            query = "SELECT DISTINCT Escenario FROM ESCENARIOS ;";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["Escenario"].ToString());
            }
            reader.Close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e) //Combo Para cargar reporte
        {
            dgvRep.Rows.Clear();
            try
            {
                query = "SELECT * FROM ESCENARIOS WHERE Escenario='" + comboBox1.Text + "';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                int i = 0;
                // MessageBox.Show(query);
                while (reader.Read())
                {
                    nInv = double.Parse(reader["NivID"].ToString());
                    RinvD = double.Parse(reader["RID"].ToString());
                    VTI = double.Parse(reader["VTI"].ToString());
                    PrP = double.Parse(reader["PP"].ToString());
                    PVunit = double.Parse(reader["PrVpU"].ToString());
                    VmP = double.Parse(reader["VMp"].ToString());
                    VmI = double.Parse(reader["Vmi"].ToString());
                    VdP = double.Parse(reader["Vdp"].ToString());
                    VdI = double.Parse(reader["Vdi"].ToString());
                    DI = double.Parse(reader["DI"].ToString());
                    Categoria = reader["Categoria"].ToString();
                    dgvRep.Rows.Add();
                    dgvRep.Rows[i].Cells[0].Value = Categoria;
                    dgvRep.Rows[i].Cells[1].Value = nInv.ToString("00,0");
                    dgvRep.Rows[i].Cells[2].Value = RinvD.ToString();
                    dgvRep.Rows[i].Cells[3].Value = VTI.ToString("C0");
                    dgvRep.Rows[i].Cells[4].Value = PrP.ToString("00,0");
                    dgvRep.Rows[i].Cells[5].Value = PVunit.ToString("C0");
                    dgvRep.Rows[i].Cells[6].Value = VmP.ToString("00,0");
                    dgvRep.Rows[i].Cells[7].Value = VmI.ToString("C0");
                    dgvRep.Rows[i].Cells[8].Value = VdP.ToString("0,0");
                    dgvRep.Rows[i].Cells[9].Value = VdI.ToString("C0");
                    dgvRep.Rows[i].Cells[10].Value = DI.ToString("0,0");

                    //lbrepo.Text = "Escenario " + reader["Escenario"].ToString() + " creado el " + reader["Fecha"].ToString() + " de el periodo de " + reader["PeriodoI"].ToString() + " a " + reader["PeriodoF"].ToString() + "";
                    i++;
                }
                reader.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show("Error " + x);
            }
        }

        private void dgvCed1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbEstructura2.Text;
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
    }
}