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
         private string conexion = "SERVER=localhost; DATABASE=dwh; user=root; PASSWORD= ;";
        //private string conexion = "SERVER=10.10.1.76; DATABASE=dwh; user=root; PASSWORD=zaptorre;";
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
        //nueva consulta 
        string fechainicialsuma = "";
        string fechafinalsuma = "";
        double gastototal = 0;
        string ventatotal = "";
        string gastotot = "";
        string venta = "";
         int id = 0;
        string idsucursal = "Total";
        string divisiones = "";
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
                    fechainicialsuma = Convert.ToString(FechaAI);
                }
                reader.Close();
                #endregion

                #region Obtener Fecha anterior final
                Fechafinal = dtpFechafinal.Text;
                query = "SELECT FechaAnterior FROM fecha WHERE Fecha='" + Fechafinal + "';";
                cmd = new MySqlCommand(query, Conn);
                cmd.CommandTimeout = 0; //posible solucion 
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FechaAF = DateTime.Parse(reader["FechaAnterior"].ToString());
                    fechafinalsuma = Convert.ToString(FechaAF);
                    label9.Text = fechafinalsuma;
                }
                reader.Close();
                #endregion

                //metodo GASTOT
                m_gastotot(fechainicialsuma, fechafinalsuma);///////////  
              //  gastototal = gastototal / 100;
                gastotot = gastototal.ToString();

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
                        if (cbEstructura.Text == "Total")
                        {
                            m_ventatot_suc(fechainicialsuma, fechafinalsuma, "(V.IDSUCURSAL= '01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')");///////////////////
                            m_venta_suc("SELECT SUM(impllenototal) AS venta FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';");
                            if (venta == "") { venta = "1"; }
                            if (ventatotal == "") { ventatotal = "1"; }
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+ " + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor, (1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + ")) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                         //   query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {

                            m_ventatot_suc(fechainicialsuma, fechafinalsuma, idsucursal);///////////////////
                            m_venta_suc("SELECT SUM(impllenototal) AS venta FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE "+idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';");
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+ " + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor, (1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + ")) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " +  idsucursal+" AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                       
                          }
                        #endregion
                           m_llenardgv(query);
                    }
                    else { }
                    if (cbEstructura2.Text == "Sucursal")
                    {    
                        #region query
                        if (idsucursal == "(V.IDSUCURSAL= '01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')")
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE V.IDSUCURSAL=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                        #endregion*
                        m_llenardgv(query); 
                    }
                    else { }
                    if (cbEstructura2.Text == "Division")
                    {

                           #region query
                        if (idsucursal == "(V.IDSUCURSAL= '01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')")
                        {
                            m_ventatot(fechainicialsuma, fechafinalsuma, "(V.IDSUCURSAL= '01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')",divisiones,idd,i);///////////////////
                           
                            m_venta("SELECT SUM(impllenototal) AS venta FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')AND "+divisiones +" = " +idd[i]+ " AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';");

                            if (venta == "") { venta = "1"; }
                            if (ventatotal == "") { ventatotal = "1"; }
                            
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp, ((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+ " + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor, (1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + ")) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp, (1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor, ((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+" + gastotot + "*(" + venta + "/" + ventatotal + "))  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND iddivisiones= '" + idd[i] + "' AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                   
                            //query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + idsucursal + "AND iddivisiones=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                         }
                        #endregion

                        m_llenardgv(query);  
                     }
                    else { }
                    if (cbEstructura2.Text == "Departamento")
                    {
                        if (idsucursal == "Total")
                        {
                            #region query
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+40000099)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+40000099)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+40000099)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE (V.IDSUCURSAL='01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08') AND iddepto=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";
                        }
                        else
                        {
                            query = "SELECT (1 - SUM(costomargenneto) /((SUM(impllenototal))))*100 AS margeninipor,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS margenfinpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)))))*SUM(impllenototal) AS margeninimp,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal) AS margenfinimp, (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)) AS rebajasimp,((SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))/SUM(impllenototal))*100  AS rebajaspor,SUM(impllenototal) AS venta,SUM(ctdneta) AS cantidad,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 AS dpppor,(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS dppimp,(1 - SUM(costomargenneto) /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100 - ((1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))*100) AS utilidadpor,((1 - SUM(costomargenneto) /((SUM(impllenototal)-(SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva))))))*SUM(impllenototal)-(1 - (SUM(costomargenneto)+400000)  /((SUM(impllenototal)-  (SUM(rebajaregsiva)+SUM(rebajapromsiva)+SUM(rebajanormalsiva)+SUM(rebajadesctosiva)))))* SUM(impllenototal) AS UTILIDADIMP FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE" + idsucursal + "AND iddepto=" + idd[i] + "  AND F.FECHA BETWEEN '" + FechaAI.ToString("yyyy-MM-dd") + "' AND '" + FechaAF.ToString("yyyy-MM-dd") + "';";

                        }
                            #endregion 
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
                        m_llenardgv(query);
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
 
                        m_llenardgv(query);
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
 
                        m_llenardgv(query);
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
                        m_llenardgv(query);
                        reader.Close();
                    }
                    else { } 
                    dgvCed4.Rows[i].Cells[1].Value = margeninipor.ToString("N2");
                    dgvCed4.Rows[i].Cells[2].Value = margeniniImp.ToString("C0");      ///margeniniImp
                    dgvCed4.Rows[i].Cells[3].Value = rebajaspor.ToString("N2");//c2   rebajaspor
                    dgvCed4.Rows[i].Cells[4].Value = rebajasimp.ToString("C0");//c2    rebajasimp
                    dgvCed4.Rows[i].Cells[5].Value = margenfinpor.ToString("N2");         //margenfinpor
                    dgvCed4.Rows[i].Cells[6].Value = margenfinImp.ToString("C0");//c2     margenfinImp

                    dgvCed4.Rows[i].Cells[7].Value = dppPor.ToString("N2");//                       n2
                    dgvCed4.Rows[i].Cells[8].Value = dppImp.ToString("C0");//c2
                    dgvCed4.Rows[i].Cells[9].Value = utilidadpor.ToString("N2");//                     n2
                    dgvCed4.Rows[i].Cells[10].Value = utilidadImp.ToString("C0");//c2
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

      

        private void cbEstructura_TextChanged(object sender, EventArgs e) /////ESTRUCT TOTAL, juarez, hidalgo,triana,lerdo,matriz
        { 
            string SeleccionActual = cbEstructura.Text;
            switch (SeleccionActual)
            {
                case "Total":
                    idsucursal = "(V.IDSUCURSAL= '01' OR V.IDSUCURSAL='02' OR V.IDSUCURSAL='06' OR V.IDSUCURSAL='08')";
                    break;
                case "Juarez":
                    idsucursal = "V.IDSUCURSAL='01' ";
                    break;
                case "Hidalgo":
                    idsucursal = "V.IDSUCURSAL='02' ";
                    break;
                case "Triana":
                    idsucursal = "V.IDSUCURSAL='06' ";
                    break;
                
                case "Matriz":
                    idsucursal = "V.IDSUCURSAL='08' ";
                    break;

            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            dtpEscenario.Value = DateTime.Now;
            dtpFechafinal.Value = DateTime.Now;
            dtpFechainicial.Value = DateTime.Now;
            tbEscenario.Clear();
           // cbEstructura.Text = "";
            //cbEstructura2.Text = "";
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
                divisiones = ""; 
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
                divisiones = "iddivisiones";
                dgvCed4.Rows.Clear();
                query = "SELECT descrip,iddivisiones from estdivisiones;";
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
                divisiones = "idepto";
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

        

        
        private void m_gastotot(string fecha1, string fecha2)
        {
            string mes1 = Convert.ToString(fecha1.Substring(3, 2));
            string mes2 = Convert.ToString(fecha2.Substring(3, 2));
            string ano1 = Convert.ToString(fecha1.Substring(6, 4));
            string ano2 = Convert.ToString(fecha2.Substring(6, 4));
            int vmes1 = Convert.ToInt32(mes1);
            int vmes2 = Convert.ToInt32(mes2);
            int vano1 = Convert.ToInt32(ano1);
            int vano2 = Convert.ToInt32(ano2);
            gastototal = 0;

            #region añomes
            for (; vano1 <= vano2; vano1++)
            {
                if (vmes1 <= vmes2)
                {
                    for (; vmes1 <= vmes2; vmes1++)
                        try
                        {
                            #region mes1 menor que mes2
                            if (vmes1 == 10)
                            {
                                query = "SELECT gasto FROM gasto WHERE gasto.month = '10' AND gasto.year = '" + vano1 + "'";
                            }
                            else if (vmes1 == 11)
                            {
                                query = "SELECT gasto FROM gasto WHERE gasto.month = '11' AND gasto.year = '" + vano1 + "'";
                            }
                            else if (vmes1 == 12)
                            {
                                query = "SELECT gasto FROM gasto WHERE gasto.month = '12' AND gasto.year = '" + vano1 + "'";
                            }
                            else
                            {
                                query = "SELECT gasto FROM gasto WHERE gasto.month = '0" + vmes1 + "' AND gasto.year = '" + vano1 + "'";
                            }
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                double gasto = Convert.ToDouble(reader["gasto"]);
                                gastototal = gastototal + gasto;
                            }
                            reader.Close();
                            #endregion

                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Error con las fechas " + x);
                        }
                }
                else if (vmes2 < vmes1)
                {
                    for (; vmes1 <= 12; vmes1++)
                        try
                        {
                            #region mes2 menor que mes1
                            query = "SELECT * FROM gasto WHERE gasto.month = '" + mes1 + "' AND gasto.year = '" + vano1 + "'";
                            cmd = new MySqlCommand(query, Conn);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                double gasto = Convert.ToDouble(reader["gasto"]);
                                gastototal = gastototal + gasto;

                            }
                            reader.Close();
                            #endregion
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Error con las fechas " + x);
                        }
                }
                vmes1 = 1;
            }
            #endregion

        }

        private void m_ventatot_suc(string fecha1, string fecha2,string suc)
        {
             
            string f1 = Convert.ToString(fecha1.Substring(6, 4)) + "-" + Convert.ToString(fecha1.Substring(3, 2)) + "-" + Convert.ToString(fecha1.Substring(0, 2));
            string f2 = Convert.ToString(fecha2.Substring(6, 4)) + "-" + Convert.ToString(fecha2.Substring(3, 2)) + "-" + Convert.ToString(fecha2.Substring(0, 2));
            query = "SELECT SUM(impneto) AS ventatot,SUM(ctdneta) AS cantidadtot FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + suc+ " AND F.FECHA BETWEEN '" + f1 + " 'AND' " + f2 + "'" ;
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ventatotal = reader["ventatot"].ToString(); 
            }
            reader.Close();
        

        }

        private void m_venta_suc(string queryventa)
        {
            query = queryventa;
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                venta = reader["venta"].ToString();
            }
            reader.Close();
        
        }

        private void m_ventatot(string fecha1, string fecha2, string suc,string iddiv, string []idd,int i)
        {

            string f1 = Convert.ToString(fecha1.Substring(6, 4)) + "-" + Convert.ToString(fecha1.Substring(3, 2)) + "-" + Convert.ToString(fecha1.Substring(0, 2));
            string f2 = Convert.ToString(fecha2.Substring(6, 4)) + "-" + Convert.ToString(fecha2.Substring(3, 2)) + "-" + Convert.ToString(fecha2.Substring(0, 2));
            query = "SELECT SUM(impneto) AS ventatot,SUM(ctdneta) AS cantidadtot FROM VENTASBASE AS V INNER JOIN SUCURSAL AS S ON V.IDSUCURSAL = S.IDSUCURSAL INNER JOIN FECHA AS F ON F.IDFECHA = V.IDFECHA WHERE " + suc + " AND " +iddiv +"='" +idd[i]+ "' AND F.FECHA BETWEEN '" + f1 + " 'AND' " + f2 + "'";
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ventatotal = reader["ventatot"].ToString();
            }
            reader.Close();


        }

        private void m_venta(string queryventa)
        {
            query = queryventa;
            cmd = new MySqlCommand(query, Conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                venta = reader["venta"].ToString();
            }
            reader.Close();

        }

        public void m_llenardgv(string query)
        { 
#region llenardgv
                 cmd = new MySqlCommand(query, Conn);
            
                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
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
    }
}
