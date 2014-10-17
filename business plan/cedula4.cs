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
    public partial class cedula4 : Form
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
        string Fechainicial = "", Fechafinal = "";
        string FechaAnteriorInicial = " ", FechaAnteriorFinal = "";
        DateTime FechaAI = DateTime.Now;
        DateTime FechaAF = DateTime.Now;
        double margeninipor=0.0,margeniniImp=0.0,rebajaspor=0.0,rebajasimp=0.0,margenfinpor=0.0,margenfinImp=0.0,dppPor=0.0,dppImp=0.0,utilidadpor=0.0,utilidadImp=0.0;
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
        
        int id = 0;
        string idsucursal = "Total";
        #endregion
        public cedula4()
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

            for (int i = 0; i <= dgvCed4.Rows.Count - 1; i++)
            {

                if (dgvCed4.Rows[i].Cells[0].Value != null)
                {
                    if (cbEstructura2.Text == "Total")
                    {
                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            //dgvCed4.Rows[i].Cells[1].Value = reader["margeninipor"].ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = reader["margenfinpor"].ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = reader["margeninimp"].ToString();
                            //dgvCed4.Rows[i].Cells[4].Value = reader["margenfinimp"].ToString();
                            //dgvCed4.Rows[i].Cells[5].Value = reader["rebajasimp"].ToString();
                            //dgvCed4.Rows[i].Cells[6].Value = reader["rebajaspor"].ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = reader["dpppor"].ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = reader["dppimp"].ToString();
                            //dgvCed4.Rows[i].Cells[9].Value = reader["utilidadpor"].ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = reader["UTILIDADIMP"].ToString();
                            if(reader["margeninipor"].ToString()!="")
                            {
                            margeninipor=double.Parse(reader["margeninipor"].ToString());
                            }
                            else{margeninipor=0;}
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString()!="")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString()!="")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND iddepto=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE" + idsucursal + "AND iddepto=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idfamilia=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idfamilia=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idlinea=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idlinea=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
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
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl1=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idl1=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                        #endregion
                        #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 2")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl2=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idl2=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                         #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 3")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl3=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idl3=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                           #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 4")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl4=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "AND idl4=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                          #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 5")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl5=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE" + idsucursal + " AND idl5=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                           #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Linea 6")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND idl6=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND idl6=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion
                           #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    if (cbEstructura.Text == "Marca")
                    {

                        #region query
                        if (idsucursal == "Total")
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND marca = '" + idd[i] + "'  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + " AND marca = '" + idd[i] + "'  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                        #endregion
                         #region llenardgv
                        cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            #region
                            //margeninipor = double.Parse(reader["margeninipor"].ToString());
                            //margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            //margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            //margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            //rebajasimp = double.Parse(reader["rebajasimp"].ToString());
                            //rebajaspor = double.Parse(reader["rebajaspor"].ToString());
                            //dppPor = double.Parse(reader["dpppor"].ToString());
                            //dppImp = double.Parse(reader["dppimp"].ToString());
                            //utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            //utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            //dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                            //dgvCed4.Rows[i].Cells[2].Value = margenfinpor.ToString();
                            //dgvCed4.Rows[i].Cells[3].Value = margeniniImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[4].Value = margenfinImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[5].Value = rebajasimp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[6].Value = rebajaspor.ToString();
                            //dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                            //dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                            //dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                            //dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                            #endregion
                            if (reader["margeninipor"].ToString() != "")
                            {
                                margeninipor = double.Parse(reader["margeninipor"].ToString());
                            }
                            else { margeninipor = 0; }
                            if (reader["margenfinpor"].ToString() != "")
                            {
                                margenfinpor = double.Parse(reader["margenfinpor"].ToString());
                            }
                            else { margenfinpor = 0; }
                            if (reader["margeninimp"].ToString() != "")
                            {
                                margeniniImp = double.Parse(reader["margeninimp"].ToString());
                            }
                            else { margeniniImp = 0; }
                            if (reader["margenfinimp"].ToString() != "")
                            {
                                margenfinImp = double.Parse(reader["margenfinimp"].ToString());
                            }
                            else { margenfinImp = 0; }
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
                            if (reader["dpppor"].ToString() != "")
                            {
                                dppPor = double.Parse(reader["dpppor"].ToString());
                            }
                            else { dppPor = 0; }
                            if (reader["dppimp"].ToString() != "")
                            {
                                dppImp = double.Parse(reader["dppimp"].ToString());
                            }
                            else { dppImp = 0; }
                            if (reader["utilidadpor"].ToString() != "")
                            {
                                utilidadpor = double.Parse(reader["utilidadpor"].ToString());
                            }
                            else { utilidadpor = 0; }
                            if (reader["UTILIDADIMP"].ToString() != "")
                            {
                                utilidadImp = double.Parse(reader["UTILIDADIMP"].ToString());
                            }
                            else { utilidadImp = 0; }
                        }

                        reader.Close();
                        #endregion
                        reader.Close();
                    }
                    else { }
                    dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString();
                    dgvCed4.Rows[i].Cells[2].Value = margeniniImp.ToString("C2");

                    dgvCed4.Rows[i].Cells[3].Value = rebajaspor.ToString();
                    dgvCed4.Rows[i].Cells[4].Value = rebajasimp.ToString("C2");
                    dgvCed4.Rows[i].Cells[5].Value = margenfinpor.ToString();

                    dgvCed4.Rows[i].Cells[6].Value = margenfinImp.ToString("C2");
                    
                    dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString();
                    dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C2");
                    dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString();
                    dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C2");
                }
            }
        }

        private void cedula4_Load(object sender, EventArgs e)
        {
            #region Colorear Datagrid
            dgvCed4.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#2882ff");
            dgvCed4.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#abcdef");
            dgvCed4.CellBorderStyle = DataGridViewCellBorderStyle.None;
            #endregion
        }

        private void chbModificar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbEstructura_TextChanged(object sender, EventArgs e)
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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            dtpEscenario.Value = DateTime.Now;
            dtpFechafinal.Value = DateTime.Now;
            dtpFechainicial.Value = DateTime.Now;
            tbEscenario.Clear();
            dgvCed4.Rows.Clear();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string EscenarioN = "0";
            try
            {
                #region comprobar nombre
                query = "SELECT Escenario from cedula4 where Escenario='" + tbEscenario.Text + "'";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EscenarioN = reader["Escenario"].ToString();
                }
                reader.Close();
                #endregion
            }
            catch(Exception x)
            {
                MessageBox.Show("Error "+x);
            }
            if (EscenarioN == tbEscenario.Text)
            {
                DialogResult boton = MessageBox.Show("Desea modificar el esenario previamente guardado?", "Alerta", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (boton == DialogResult.OK)
                {
                    for (int i = 0; i <= dgvCed4.Rows.Count-1; i++)
                    {
                        if (dgvCed4.Rows[0].Cells[0].Value != null)
                        {
                            margeninipor = double.Parse(dgvCed4.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                            margenfinpor = double.Parse(dgvCed4.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                            margeniniImp = double.Parse(dgvCed4.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                            margenfinImp = double.Parse(dgvCed4.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                            rebajasimp = double.Parse(dgvCed4.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                            rebajaspor = double.Parse(dgvCed4.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                            dppPor = double.Parse(dgvCed4.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                            dppImp = double.Parse(dgvCed4.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                            utilidadpor = double.Parse(dgvCed4.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                            utilidadImp = double.Parse(dgvCed4.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                            #region actualizar
                            query = "UPDATE cedula4 SET margeniniPor=" +margeninipor.ToString()+ ",margeniniImp=" +margeniniImp+ ",rebajasPor=" +rebajaspor.ToString() + ",rebajasImp=" + rebajasimp.ToString() + ",margenfinPor=" + margenfinpor.ToString() + ",margenfinImp=" +margenfinImp.ToString()+ ",dppPor=" + dppPor.ToString() + ",dppImp=" +dppImp.ToString() + ",utilidadPor=" + utilidadpor.ToString() + ",utilidadImp=" + utilidadpor.ToString()+" where Escenario='" + tbEscenario.Text + "'";
                            cmd = new MySqlCommand(query, Conn);
                            cmd.ExecuteNonQuery();
                            #endregion
                        }
                        else 
                        {
                            tbEscenario.Clear();
                            tbEscenario.Focus();
                        }
                    }
                }
                else { }
            }
            else 
            {
                for (int i = 0; i <= dgvCed4.Rows.Count-1; i++)
                {

                    if (dgvCed4.Rows[0].Cells[0].Value != null)
                    {
                        margeninipor = double.Parse(dgvCed4.Rows[i].Cells[1].Value.ToString(), NumberStyles.Currency);
                        margenfinpor = double.Parse(dgvCed4.Rows[i].Cells[5].Value.ToString(), NumberStyles.Currency);
                        margeniniImp = double.Parse(dgvCed4.Rows[i].Cells[2].Value.ToString(), NumberStyles.Currency);
                        margenfinImp = double.Parse(dgvCed4.Rows[i].Cells[6].Value.ToString(), NumberStyles.Currency);
                        rebajasimp = double.Parse(dgvCed4.Rows[i].Cells[4].Value.ToString(), NumberStyles.Currency);
                        rebajaspor = double.Parse(dgvCed4.Rows[i].Cells[3].Value.ToString(), NumberStyles.Currency);
                        dppPor = double.Parse(dgvCed4.Rows[i].Cells[7].Value.ToString(), NumberStyles.Currency);
                        dppImp = double.Parse(dgvCed4.Rows[i].Cells[8].Value.ToString(), NumberStyles.Currency);
                        utilidadpor = double.Parse(dgvCed4.Rows[i].Cells[9].Value.ToString(), NumberStyles.Currency);
                        utilidadImp = double.Parse(dgvCed4.Rows[i].Cells[10].Value.ToString(), NumberStyles.Currency);
                        #region insertar
                        query = "INSERT INTO  cedula4 (Escenario,margeniniPor,margeniniImp,rebajasPor,rebajasImp,margenfinPor,margenfinImp,dppPor,dppImp,utilidadPor,utilidadImp) VALUES('" + tbEscenario.Text + "'," + margeninipor.ToString() + "," + margeniniImp.ToString() + "," + rebajaspor.ToString() + "," + rebajasimp.ToString() + "," + margenfinpor.ToString() + "," + margenfinImp.ToString() + "," + dppPor.ToString() + "," + dppImp.ToString() + "," + utilidadpor.ToString() + "," + utilidadImp.ToString() + ")";
                        cmd = new MySqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                        #endregion
                    }
                    else { }
                }
                MessageBox.Show("Guardado");
            }
        }

        private void cbEstructura2_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string SeleccionActual = cbEstructura2.Text;
            #region total
            if (SeleccionActual == "Total")
            {
                dgvCed4.Rows.Clear();
                dgvCed4.Rows.Add();
                dgvCed4.Rows[0].Cells[0].Value = "Total";
            }
            else
            { }
            #endregion
            #region sucursal
            if (SeleccionActual == "Sucursal")
            {
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idsucursal from sucursal where visible='S';";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,iddivisiones from estdivisiones;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,iddepto from estdepartamento;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idfamilia from estfamilia;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idlinea from estlinea;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idl1 from estl1;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idl2 from estl2;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idl3 from estl3;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idl4 from estl4;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip,idl5 from estl5;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT distinct descrip, idl6 from estl6;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
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
                dgvCed4.Rows.Clear();
                query = "SELECT marca, descrip  from marca;";
                cmd = new MySqlCommand(query, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //cbEstructura.Items.Add(reader["descrip"].ToString());
                    dgvCed4.Rows.Add();
                    dgvCed4.Rows[i].Cells[0].Value = reader["descrip"].ToString();
                    idd[i] = reader["marca"].ToString();
                    i++;
                }
                reader.Close();
            }
            else
            { }
            #endregion
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            query = "SELECT * FROM cedula4;";
            cmd = new MySqlCommand(query, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["Escenario"].ToString());
                        }
                        reader.Close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //int i=0;
            //query = "SELECT * FROM cedula4 WHERE Escenario='"+comboBox1.Text+"';";
            //cmd = new MySqlCommand(query, Conn);
            //            reader = cmd.ExecuteReader();
            //            while (reader.Read())
            //            {
            //                dgvCed4.Rows[].Cells[].Value=;
            //            }
        }  
    }
}
