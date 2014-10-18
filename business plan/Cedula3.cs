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
using System.Globalization;

namespace business_plan
{
    public partial class Cedula3 : Form
    {
        #region variables conexion
        MySqlConnection Conn,ConnCipsis;
        string query;
        MySqlCommand cmd;
        MySqlDataReader reader;
        private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
        private string conexion2 = "SERVER=10.10.1.76; DATABASE=cipsis; user=root; PASSWORD=zaptorre;";
        //private string conexion = "SERVER=localhost; DATABASE=cipsis; user=root; PASSWORD=;";
        //private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";
        #endregion
        #region variables globales
        DateTime fecharecibo = DateTime.Now;
        string fechareciboT = "";
        string cedula1 = "";
        string[] idd = new string[1000];
        string Fechainicial = "", Fechafinal = "";
        string FechaAnteriorInicial = " ", FechaAnteriorFinal = "";
        DateTime FechaAI = DateTime.Now;
        DateTime FechaAF = DateTime.Now;
        double costo = 0, cantidad = 0, plazo=0,importe=0,cantidadV=0,preciounit=0,rebajasimp=0,rebajaspor=0,costoneto=0,unidadesSaldo=0,importeSaldos=0,rotacion=0,diasINv=0; 
        int id = 0;
        string idsucursal = "Total";
        double[] cantidadA = new double[1000];
        double[] importeA = new double[1000];
        #endregion
        public Cedula3()
        {
            InitializeComponent();
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
            #endregion
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
            #region datagridview valores
            //for (int i = 0; i <= dgvCed3.ColumnCount; i++)
            //{
            //    if (i == 0)
            //    {
            //        dgvCed3.Rows[0].Cells[0].Value = " ";
            //    }
            //    else { }
            //    if (i >= 1 && i <= 3)
            //    {
            //        dgvCed3.Rows[0].Cells[i].Value = "Compras";
            //    }
            //    else { }
            //    if(i>=5 && i<=8)
            //    {
            //        dgvCed3.Rows[0].Cells[i].Value = "Ventas";
            //    }
            //    else { }
            //    if(i>=10&&i<=14)
            //    {
            //        dgvCed3.Rows[0].Cells[i].Value = "Saldos";
            //    }
            //    else { }
            //    if(i>=16&&i<=22)
            //    {
            //        dgvCed3.Rows[0].Cells[i].Value = "Saldos";
            //    }
            //    else { }
            //}
            #endregion
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
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
                #endregion

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
                #endregion
            }
            catch (Exception x)
            {
                MessageBox.Show("Error con las fechas " + x);
            }

            for (int i = 0; i <= dgvCed3.Rows.Count - 1; i++)
            {

                if (dgvCed3.Rows[i].Cells[0].Value != null)
                {
                    #region querys y estructura
                    if (cbEstructura2.Text == "Total")
                    {
                        #region query y obtener datos
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '"+FechaAI.ToString("yyyy-MM-dd")+"' AND DATE_ADD('"+FechaAI.ToString("yyyy-MM-dd")+"',INTERVAL 1 DAY);";
                            #endregion
                            #region ejecutar query
                            cmd.CommandTimeout = 120;
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '"+FechaAI.ToString("yyyy-MM-dd")+"' AND '"+FechaAF.ToString("yyyy-MM-dd")+"';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if(reader["cantidad"].ToString()!="")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+"  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND DATE_ADD('" + FechaAI.ToString("yyyy-MM-dd") + "',INTERVAL 1 DAY);";
                            #endregion
                            #region ejecutar query
                            cmd.CommandTimeout = 120;
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Sucursal")
                    {
                        #region query y obtener datos 
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`iddivisiones`=1;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Division")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query y obtener datos
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND DATE_ADD('" + FechaAI.ToString("yyyy-MM-dd") + "',INTERVAL 1 DAY);";
                            #endregion
                            #region ejecutar query
                            cmd.CommandTimeout = 120;
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND iddivisiones=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE "+idsucursal+"  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`iddivisiones`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.iddivisiones=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                            #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Departamento")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query y obtener datos
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`iddepto`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.iddepto=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE "+idsucursal+"  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`iddepto`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.iddepto=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion

                        }
                            #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Familia")
                    {
                        #region query y obtener datos
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`idfamilia`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.idfamilia=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else 
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE "+idsucursal+"  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`idfamilia`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.idfamilia=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Linea")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA INNER JOIN ventasbase AS b ON V.`idarticulo`=b.`idarticulo` WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND b.`idfamilia`=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V   INNER JOIN estarticulo AS E ON E.`marca`=V.`marca`;";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, ConnCipsis);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND V.idfamilia=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idlinea=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idlinea=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idlinea=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura2.Text == "Linea 1")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl1=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 2")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl2=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 3")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl3=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 4")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl4=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 5")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl5=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 6")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND idl6=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    if (cbEstructura.Text == "Marca")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "' AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        else
                        {
                            #region query saldos iniciales
                            query = "SELECT SUM(costot) AS costo,SUM(ctd) AS cantidad FROM EXIST AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["costo"].ToString() != "")
                                {
                                    costo = double.Parse(reader["costo"].ToString());
                                }
                                else { costo = 0; }
                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidad = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidad = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region Compras
                            query = "SELECT diaspp AS plazo FROM condicionesp AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["plazo"].ToString() != "")
                                {
                                    plazo = double.Parse(reader["plazo"].ToString());
                                }
                                else { plazo = 0; }
                            }
                            reader.Close();
                            #endregion
                            #region ventas
                            query = "SELECT SUM(impllenototal) AS importe,SUM(ctdneta) AS cantidad ,(SUM(impllenototal)/SUM(ctdneta)) AS preciounit, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "'  AND marca=" + idd[i] + ";";
                            #endregion
                            #region ejecutar query
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (reader["importe"].ToString() != "")
                                {
                                    importe = double.Parse(reader["importe"].ToString());
                                }
                                else { importe = 0; }

                                if (reader["cantidad"].ToString() != "")
                                {
                                    cantidadV = double.Parse(reader["cantidad"].ToString());
                                }
                                else { cantidadV = 0; }

                                if (reader["preciounit"].ToString() != "")
                                {
                                    preciounit = double.Parse(reader["preciounit"].ToString());
                                }
                                else { preciounit = 0; }

                                if (reader["rebajasimp"].ToString() != "")
                                {
                                    rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                                }
                                else { rebajasimp = 0; }

                                if (reader["rebajaspor"].ToString() != "")
                                {
                                    rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                                }
                                else { rebajaspor = 0; }

                            }
                            reader.Close();
                            #endregion
                        }
                        #endregion
                    }
                    else { }
                    #endregion
                    #region operaciones
                    fecharecibo = DateTime.Parse(dtpFechaRecibo.Text);
                    Math.Round(costo,2);
                    costoneto = costo / cantidad;
                    Math.Round(costoneto,2);
                    Math.Round(plazo,2);
                    unidadesSaldo=cantidadA[i]-cantidadV;
                    Math.Round(unidadesSaldo,2);
                    importeSaldos=importeA[i]-importe;
                    Math.Round(importeSaldos,2);
                    #endregion
                    //----------------------Saldos iniciales----------------------//
                    dgvCed3.Rows[i].Cells[1].Value = cantidad.ToString();
                    dgvCed3.Rows[i].Cells[2].Value = costo.ToString("C2");
                    dgvCed3.Rows[i].Cells[3].Value = costoneto.ToString("C2");
                    //----------------------Compras------------------------------//
                    dgvCed3.Rows[i].Cells[4].Value=fecharecibo.ToString("yyyy-MM-dd");
                    dgvCed3.Rows[i].Cells[5].Value = plazo.ToString("C2");
                    dgvCed3.Rows[i].Cells[6].Value = cantidadA[i].ToString();
                    dgvCed3.Rows[i].Cells[7].Value = importeA[i].ToString("C2");
                    //----------------------Ventas------------------------------//
                    dgvCed3.Rows[i].Cells[8].Value=fecharecibo.ToString("yyyy-MM-dd");
                    dgvCed3.Rows[i].Cells[9].Value = cantidadV.ToString();
                    dgvCed3.Rows[i].Cells[10].Value = preciounit.ToString("C2");
                    dgvCed3.Rows[i].Cells[11].Value = importe.ToString("C2");
                    dgvCed3.Rows[i].Cells[12].Value=rebajaspor.ToString();
                    dgvCed3.Rows[i].Cells[13].Value=rebajasimp.ToString("C2");
                    //---------------------Saldos------------------------------//
                    dgvCed3.Rows[i].Cells[14].Value=fecharecibo.ToString("yyyy-MM-dd");
                    dgvCed3.Rows[i].Cells[15].Value=unidadesSaldo.ToString();
                    dgvCed3.Rows[i].Cells[16].Value=importeSaldos.ToString("C2");
                    dgvCed3.Rows[i].Cells[17].Value=rotacion.ToString();
                    dgvCed3.Rows[i].Cells[18].Value = diasINv.ToString();
                }
            }
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbEstructura2.Text;
            #region total
            if (SeleccionActual == "Total")
            {
                dgvCed3.Rows.Clear();
                dgvCed3.Rows.Add();
                dgvCed3.Rows[0].Cells[0].Value = "Total";
            }
            else
            { }
            #endregion
            #region sucursal
            if (SeleccionActual == "Sucursal")
            {
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,iddepto from estdepartamento;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idfamilia from estfamilia;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idlinea from estlinea;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idl1 from estl1;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idl2 from estl2;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idl3 from estl3;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idl4 from estl4;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip,idl5 from estl5;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT distinct descrip, idl6 from estl6;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed3.Rows.Clear();
                query = "SELECT marca, descrip  from marca;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed3.Rows.Add();
                    dgvCed3.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["marca"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
        }

        private void cbEstructura_TextChanged_1(object sender, EventArgs e)
        {
            int i = 0;
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

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            query = "SELECT  DISTINCT nombre FROM cedula2;";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            query = "SELECT DISTINCT * FROM cedula2 WHERE nombre='"+comboBox1.Text+"';";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cantidadA[i] = double.Parse(reader["AUnid"].ToString());
                importeA[i] = double.Parse(reader["AImpo"].ToString());
                cedula1=reader["cedula1"].ToString();
                i++;
            }
            reader.Close();

            query = "SELECT * FROM escenarios WHERE Escenario='"+cedula1+"';";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rotacion=double.Parse(reader["RID"].ToString());
                diasINv = double.Parse(reader["DI"].ToString());
                j++;
            }
            reader.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            dtpEscenario.Value = DateTime.Now;
            dtpFechafinal.Value = DateTime.Now;
            dtpFechainicial.Value = DateTime.Now;
            dtpFechaRecibo.Value = DateTime.Now;
            comboBox1.Items.Clear();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            double unidadesSi = 0, importeSi = 0, costnSi = 0, plazp = 0, unidadesR = 0, importeR = 0, UnidadesV = 0, precioU = 0, importV = 0, rebajap = 0, rebajai = 0, unid = 0, impS = 0, imporS = 0, Rot = 0, Di = 0;
            for (int i = 0; i <= dgvCed3.Rows.Count; i++)
            {
                if (dgvCed3.Rows[0].Cells[0].Value != null)
                {
                    unidadesSi = double.Parse(dgvCed3.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                    importeSi = double.Parse(dgvCed3.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                    costnSi = double.Parse(dgvCed3.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                    plazp = double.Parse(dgvCed3.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                    unidadesR = double.Parse(dgvCed3.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                    importeR = double.Parse(dgvCed3.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                    UnidadesV = double.Parse(dgvCed3.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                    precioU = double.Parse(dgvCed3.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                    importV = double.Parse(dgvCed3.Rows[i].Cells[11].Value.ToString(), NumberStyles.Currency);
                    rebajap = double.Parse(dgvCed3.Rows[i].Cells[12].Value.ToString(), NumberStyles.Currency);
                    rebajai = double.Parse(dgvCed3.Rows[i].Cells[13].Value.ToString(), NumberStyles.Currency);
                    unid = double.Parse(dgvCed3.Rows[i].Cells[15].Value.ToString(), NumberStyles.Currency);
                    imporS = double.Parse(dgvCed3.Rows[i].Cells[16].Value.ToString(), NumberStyles.Currency);
                    Rot = double.Parse(dgvCed3.Rows[i].Cells[17].Value.ToString(), NumberStyles.Currency);
                    Di = double.Parse(dgvCed3.Rows[i].Cells[18].Value.ToString(), NumberStyles.Currency);
                    query = "INSERT INTO  cedula3(nombre,estructura,estructura2,unidadessi,importessi,fecharecibo,plazopago,unidadesrecibo,importesrecibo,unidadesV,preciounitario,importeV,rebajaspor,rebajasi,unidadesS,importes,rotacion,DI) VALUES('"+tbnombre.Text+"','"+cbEstructura.Text+"','"+cbEstructura2.Text+"',)";
                    cmd = new MySqlCommand(query, Conn);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("guardado");
        }
    }
}
