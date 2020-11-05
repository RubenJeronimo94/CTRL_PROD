using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPage
{
    public partial class Index : System.Web.UI.Page
    {
        public static int ID
        {
            get
            {
                if (HttpContext.Current.Session["ID"] == null)
                    HttpContext.Current.Session["ID"] = 0;

                return (int)HttpContext.Current.Session["ID"];
            }
            set
            {
                HttpContext.Current.Session["ID"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string query = Request.QueryString["ID"];

            if (!string.IsNullOrEmpty(query) && Diversos.IsNumeric(query))
                ID = Convert.ToInt32(query);
            else
                Response.Redirect("/index.aspx?id=1");



            #region Get Maquinas

            try
            {
                if (VARS.UserSession.SessaoIniciada)
                {
                    if (VARS.Maquinas.Count == 0)
                        Response.Redirect("/notfound.html");
                    else
                   if (VARS.Maquinas.Count == 1)
                        divSelectMaquinas.Style.Add("display", "none");
                    else
                    {
                        StringBuilder strRadios = new StringBuilder();

                        for (int i = 0; i < VARS.Maquinas.Count; i++)
                        {

                            strRadios.AppendLine("<div class=\"form-check\">");
                            strRadios.AppendLine("<label class=\"form-check-label\">");
                            strRadios.AppendLine("<input type=\"radio\" class=\"form-check-input\" name=\"choice\" value=\"" + VARS.Maquinas[i].Id.ToString() + "\" onclick=\"RadioClick();\"" + (VARS.Maquinas[i].Id == ID ? "checked" : "") + " > ");
                            strRadios.AppendLine(VARS.Maquinas[i].Nome);
                            strRadios.AppendLine("</label>");
                            strRadios.AppendLine("</div>");
                            strRadios.AppendLine("<div class=\"form-check\">");
                            strRadios.AppendLine("&nbsp  &nbsp  &nbsp");
                            strRadios.AppendLine("</div>");

                        }

                        divRadioButtons.InnerHtml = strRadios.ToString();
                    }

                    Maquina maq = VARS.Maquinas.Find(x => x.Id == ID);

                    if (maq != null)
                    {
                        ControloDiametro control = (ControloDiametro)LoadControl("/ControloDiametro.ascx");

                        control.IdLinha = ID;

                        pnlEspacoMaquinas.Controls.Add(control);
                    }
                    else
                        Response.Redirect("/index.aspx?id=1");
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Page_Load(): " + ex.Message);
            }
            #endregion

        }


        [WebMethod]
        public static string GetUserControlInfos(string id)
        {
            //Populating a DataTable from database.
            DataTable dt = new DataTable();
            dt.Columns.Add("Diametro", typeof(double));
            dt.Columns.Add("Classificacao", typeof(int));
            dt.Columns.Add("Error", typeof(bool));
            dt.Columns.Add("Timestamp", typeof(string));

            try
            {
                Maquina maquina = VARS.Maquinas.First(x => x.Id == Convert.ToInt32(id));

                if (maquina == null)
                    throw new Exception("máquina não encontrada!");

                uint maxDifSec = 30;

                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand("select top 1 diametro, ABS(DATEDIFF(SECOND, getdate() , DtUpload)) AS DateDiff, DtUpload from " + maquina.DataTable + " order by id desc", sqlConn))
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        if (dr.Read())
                            dt.Rows.Add(Math.Round(Convert.ToDouble(dr[0]), 3), (int)maquina.Classe.GetClassificacao(Convert.ToDouble(dr[0])), Convert.ToUInt32(dr[1]) > maxDifSec, Convert.ToDateTime(dr[2]).ToString("dd/MM/yyyy HH:mm:ss"));
                        else
                            throw new Exception("sem dados lidos!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetUserControlInfos(): " + ex.Message);

                dt.Rows.Clear();
                dt.Rows.Add(0, true);

            }

            return VARS.ConvertDataTabletoString(dt);

        }

        [WebMethod]
        public static string GetAlertasProducao(string id)
        {
            try
            {
                Maquina maquina = VARS.Maquinas.First(x => x.Id == Convert.ToInt32(id));

                //Populating a DataTable from database.
                DataTable dt = new DataTable();

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT Alertas.Id, TipoAlerta.Tipo, Alertas.Diametro, FORMAT(Alertas.DataHora, 'dd/MM/yyyy HH:mm:ss') AS 'DataHora' FROM Alertas ");
                strQuery.Append("INNER JOIN TipoAlerta ON Alertas.IdAlerta = TipoAlerta.Id ");
                strQuery.Append("WHERE Alertas.IdMaquina = @IdMaquina AND Alertas.DataHora >= DATEADD (dd, " + (VARS.NUM_OF_DAYS_TO_SHOW_ALERT).ToString() + ", GETDATE()) ");
                strQuery.Append("ORDER BY Alertas.Id DESC");

                if (maquina == null)
                    throw new Exception("máquina não encontrada!");
                else
                    using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                    using (SqlCommand sqlCmd = new SqlCommand(strQuery.ToString(), sqlConn))
                    {
                        sqlCmd.Parameters.Add("@IdMaquina", SqlDbType.TinyInt).Value = Convert.ToInt32(id);

                        using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
                            sda.Fill(dt);
                    }

                return VARS.ConvertDataTabletoString(dt);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAlertasProducao(): " + ex.Message);

                return string.Empty;

            }

        }

        [WebMethod]
        public static string CountAlertasProducao(string id)
        {
            try
            {

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT COUNT(Id) FROM Alertas ");
                strQuery.Append("WHERE IdMaquina = @IdMaquina AND DataHora >= DATEADD (dd, " + (VARS.NUM_OF_DAYS_TO_SHOW_ALERT).ToString() + ", GETDATE()) ");

                return Convert.ToInt32(SQLHelper.ExecuteScalar(strQuery.ToString(), VARS.ConnectionString, new List<SqlParameter>() { new SqlParameter("@IdMaquina", Convert.ToInt32(id)) })).ToString();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("CountAlertasProducao(): " + ex.Message);

                return "0";

            }
        }

        [WebMethod]
        public static string GetLastChartInfo(string id)
        {
            //Populating a DataTable from database.
            DataTable dt = new DataTable();
            dt.Columns.Add("Diametro", typeof(double));
            dt.Columns.Add("Classificacao", typeof(int));
            dt.Columns.Add("Timestamp", typeof(string));

            try
            {
                Maquina maquina = VARS.Maquinas.First(x => x.Id == Convert.ToInt32(id));

                int lastSec = -1;

                if (maquina == null)
                    throw new Exception("máquina não encontrada!");
                else
                    if (maquina.GraficoPontos.GetLastNPoints(1200)) //antes - 240 (2min)
                    foreach (Maquina.PontosGraph.DataPoint point in maquina.GraficoPontos.Pontos)
                    {
                        if (lastSec != point.DataHora.Second) //xanato para so enviar pontos a cada seg
                            dt.Rows.Add(Math.Round(point.Value, 3), (int)point.Classe, point.DataHora.ToString("dd/MM/yyyy HH:mm:ss"));

                        lastSec = point.DataHora.Second;
                    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetLastChartInfo(): " + ex.Message);

                return string.Empty;

            }

            return VARS.ConvertDataTabletoString(dt);

        }

    }
}