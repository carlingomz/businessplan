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
    public partial class cedula5 : Form
    {
        #region variables conexion
        MySqlConnection Conn, ConnCipsis;
        string query;
        MySqlCommand cmd;
        MySqlDataReader reader;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        private string conexion2 = "SERVER=10.10.1.76; DATABASE=cipsis; user=root; PASSWORD=zaptorre;";
        //private string conexion = "SERVER=localhost; DATABASE=cipsis; user=root; PASSWORD=;";
        //private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";
        #endregion
        #region variables globales
        string[] idd = new string[1000];
        string[,] provedor =new string[1,1];
        string[,] marca =new string[1,1];
        DateTime ejercicio = DateTime.Now;
        double enero = 0, febrero = 0, marzo = 0, abril = 0, mayo = 0, junio = 0, julio = 0, agosto = 0, septiembre = 0, octubre = 0, noviembre = 0, diciembre = 0, saldoAcum = 0;
        string marc = "",prov="",nump="";
        #endregion

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
            #endregion
            #region cargar combo provedor
            query = "SELECT * FROM prov";
            cmd = new MySqlCommand(query, ConnCipsis);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cbEstructura.Items.Add(reader["raz_soc"].ToString());
            }
            reader.Close();
            #endregion
            #region cargar combo marca
            query = "SELECT * FROM marca";
            cmd = new MySqlCommand(query, ConnCipsis);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cbEstructura2.Items.Add(reader["descrip"].ToString());
            }
            reader.Close();
            #endregion
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            int i = 0;
            ejercicio = DateTime.Parse(dtpFechaEjercicio.Value.ToString());
            if(cbEstructura.Text=="Total"&& cbEstructura2.Text=="Total")
            {
                
                    query = "SELECT saldoact AS saldoact, enero AS enero1,febrero AS febrero1, marzo AS marzo1,abril AS abril1, mayo AS mayo1, junio AS junio1, julio AS julio1, agosto AS agosto1,septiembre AS septiembre1, octubre AS octubre1, noviembre AS noviembre1, diciembre AS diciembre1 FROM saldoprov WHERE ejercicio = '" + ejercicio.ToString("yyyy") + "'";
                    cmd = new MySqlCommand(query, ConnCipsis);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
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
                        #region mostrar
                        if (dgvCed1.Rows[0].Cells[0].Value == null)
                        {
                            dgvCed1.Rows.Add();
                            dgvCed1.Rows[i].Cells[3].Value = enero.ToString("C2");
                            dgvCed1.Rows[i].Cells[4].Value = febrero.ToString("C2");
                            dgvCed1.Rows[i].Cells[5].Value = marzo.ToString("C2");
                            dgvCed1.Rows[i].Cells[6].Value = abril.ToString("C2");
                            dgvCed1.Rows[i].Cells[7].Value = mayo.ToString("C2");
                            dgvCed1.Rows[i].Cells[8].Value = junio.ToString("C2");
                            dgvCed1.Rows[i].Cells[9].Value = julio.ToString("C2");
                            dgvCed1.Rows[i].Cells[10].Value = agosto.ToString("C2");
                            dgvCed1.Rows[i].Cells[11].Value = septiembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[12].Value = octubre.ToString("C2");
                            dgvCed1.Rows[i].Cells[13].Value = noviembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[14].Value = diciembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[15].Value = saldoAcum.ToString("C2");
                        }
                        else
                        {
                            dgvCed1.Rows.Add();
                            dgvCed1.Rows[i].Cells[3].Value = enero.ToString("C2");
                            dgvCed1.Rows[i].Cells[4].Value = febrero.ToString("C2");
                            dgvCed1.Rows[i].Cells[5].Value = marzo.ToString("C2");
                            dgvCed1.Rows[i].Cells[6].Value = abril.ToString("C2");
                            dgvCed1.Rows[i].Cells[7].Value = mayo.ToString("C2");
                            dgvCed1.Rows[i].Cells[8].Value = junio.ToString("C2");
                            dgvCed1.Rows[i].Cells[9].Value = julio.ToString("C2");
                            dgvCed1.Rows[i].Cells[10].Value = agosto.ToString("C2");
                            dgvCed1.Rows[i].Cells[11].Value = septiembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[12].Value = octubre.ToString("C2");
                            dgvCed1.Rows[i].Cells[13].Value = noviembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[14].Value = diciembre.ToString("C2");
                            dgvCed1.Rows[i].Cells[15].Value = saldoAcum.ToString("C2");
                        }
                        i++;



                        #endregion
                        i++;
                    }
                    reader.Close();
            }
            if(cbEstructura.Text=="Total"&&cbEstructura2.Text=="Total")
            { }
        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
        {
            dgvCed1.Rows.Clear();
            if (cbEstructura.Text != "Total")
            {
                query = "SELECT * FROM prov where raz_soc='"+cbEstructura.Text+"'";
                cmd = new MySqlCommand(query, ConnCipsis);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvCed1.Rows.Add();
                    dgvCed1.Rows[0].Cells[0].Value=reader["proveedor"].ToString();
                    dgvCed1.Rows[0].Cells[1].Value = reader["Raz_soc"].ToString();
                }
                reader.Close();
            }
            if(cbEstructura.Text=="Total")
            {
                dgvCed1.Rows.Add();
                dgvCed1.Rows[0].Cells[0].Value = "Total";
                dgvCed1.Rows[0].Cells[1].Value = "Total";
            }
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
            int i=0;
            if (cbEstructura2.Text!="Total")
            {
                query = "SELECT * FROM marca where descrip='" + cbEstructura2.Text + "'";
                cmd = new MySqlCommand(query, ConnCipsis);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvCed1.Rows[0].Cells[2].Value=reader["descrip"].ToString();
                }
                reader.Close();
            }
            if(cbEstructura2.Text=="Total")
            {
                query = "SELECT * FROM marca";
                cmd = new MySqlCommand(query, ConnCipsis);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (dgvCed1.Rows[0].Cells[0].Value == null)
                    {
                        dgvCed1.Rows.Add();
                        dgvCed1.Rows[i].Cells[2].Value = reader["descrip"].ToString();
                    }
                    else 
                    {
                        dgvCed1.Rows.Add();
                        dgvCed1.Rows[i].Cells[2].Value = reader["descrip"].ToString();

                    }
                    i++;
                }
                reader.Close();
            }
        }
    }
}
