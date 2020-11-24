using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPage
{
    public partial class Configuracoes : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                #region Preenche Infos

                try
                {
                    Maquina maquina = VARS.Maquinas.First(x => x.Id == ID);

                    if (maquina != null)
                    {
                        txtNomeMaquina.Value = maquina.Nome;

                        this.lblNomeMaquina.InnerText = this.Title = "Configurações Avançadas - " + maquina.Nome;

                        txtLimConformeMin.Value = maquina.Classe.Conforme.Min.ToString("0.000");
                        txtLimConformeMax.Value = maquina.Classe.Conforme.Max.ToString("0.000");

                        txtLimClasse2Min.Value = maquina.Classe.Classe2.Min.ToString("0.000");
                        txtLimClasse2Max.Value = maquina.Classe.Classe2.Max.ToString("0.000");
                        txtMaxPointsclasse2.Value = maquina.Classe.MaxPontosClasse2.ToString();

                        txtLimClasse3Min.Value = maquina.Classe.Classe3.Min.ToString("0.000");
                        txtLimClasse3Max.Value = maquina.Classe.Classe3.Max.ToString("0.000");
                        txtMaxPointsclasse3.Value = maquina.Classe.MaxPontosClasse3.ToString();

                        txtMaxPointsNC.Value = maquina.Classe.MaxPontosNC.ToString();
                        txtSptempoMaxPointsNC.Value = Convert.ToInt32(maquina.Classe.SpTempoMaxPontosNC.TotalSeconds).ToString();
                    }
                    else
                        throw new Exception("sem dados lidos para a maquina " + ID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                #endregion

            }

        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNomeMaquina.Value) || string.IsNullOrEmpty(txtLimConformeMin.Value) || string.IsNullOrEmpty(txtLimConformeMax.Value) ||
                    string.IsNullOrEmpty(txtLimClasse2Min.Value) || string.IsNullOrEmpty(txtLimClasse2Max.Value) || string.IsNullOrEmpty(txtMaxPointsclasse2.Value) ||
                    string.IsNullOrEmpty(txtLimClasse3Min.Value) || string.IsNullOrEmpty(txtLimClasse3Max.Value) || string.IsNullOrEmpty(txtMaxPointsclasse3.Value) ||
                    string.IsNullOrEmpty(txtMaxPointsNC.Value) || string.IsNullOrEmpty(txtSptempoMaxPointsNC.Value))
                    throw new Exception("campos estão vazios!");

                StringBuilder strQuery = new StringBuilder();

                strQuery.Append("UPDATE MAQUINAS SET Nome = @Nome, FilConformeMin = @FilConformeMin, FilConformeMax = @FilConformeMax, ");
                strQuery.Append("FilClasse2Min = @FilClasse2Min, FilClasse2Max = @FilClasse2Max, MaxPontosClasse2 = @MaxPontosClasse2, ");
                strQuery.Append("FilClasse3Min = @FilClasse3Min, FilClasse3Max = @FilClasse3Max, MaxPontosClasse3 = @MaxPontosClasse3, ");
                strQuery.Append("MaxPontosNC = @MaxPontosNC, SpSecMaxPontosNC = @SpSecMaxPontosNC WHERE ID = @ID");

                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand(strQuery.ToString(), sqlConn))
                {
                    sqlCmd.Parameters.AddWithValue("@Nome", txtNomeMaquina.Value);
                    sqlCmd.Parameters.AddWithValue("@FilConformeMin", Convert.ToDouble(txtLimConformeMin.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@FilConformeMax", Convert.ToDouble(txtLimConformeMax.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@FilClasse2Min", Convert.ToDouble(txtLimClasse2Min.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@FilClasse2Max", Convert.ToDouble(txtLimClasse2Max.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@MaxPontosClasse2", Convert.ToInt32(txtMaxPointsclasse2.Value));
                    sqlCmd.Parameters.AddWithValue("@FilClasse3Min", Convert.ToDouble(txtLimClasse3Min.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@FilClasse3Max", Convert.ToDouble(txtLimClasse3Max.Value.Replace(".", ",")));
                    sqlCmd.Parameters.AddWithValue("@MaxPontosClasse3", Convert.ToInt32(txtMaxPointsclasse3.Value));

                    sqlCmd.Parameters.AddWithValue("@MaxPontosNC", Convert.ToInt32(txtMaxPointsNC.Value));
                    sqlCmd.Parameters.AddWithValue("@SpSecMaxPontosNC", Convert.ToInt32(txtSptempoMaxPointsNC.Value));
                    sqlCmd.Parameters.AddWithValue("@ID", ID);

                    sqlConn.Open();

                    if (sqlCmd.ExecuteNonQuery() == 1)
                        Response.Redirect("/index.aspx", false);
                    else
                        throw new Exception("erro ao executar query!");


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                Response.Write("<b>Ocorreu um erro!</b> <br/> Clique <a href='javascript:window.history.back();'>aqui</a> para voltar a página anterior!  <br/>  <br/> <b>Descrição: </b>" + ex.Message);

                Response.End();

                //    Response.Redirect("/notfound.html", false);
            }
            finally
            {
                VARS.Maquinas = null;
            }
        }
    }
}