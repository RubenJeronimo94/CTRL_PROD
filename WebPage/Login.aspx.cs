using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPage
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (VARS.UserSession.SessaoIniciada)
                Response.Redirect("/index.aspx");

            lblError.Visible = false;
        }

        protected void btnIniciarSessao_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (VARS.UserSession.SessaoIniciada)
                    throw new Exception("Já existe uma sessão iniciada!");

                bool sessaoIniciou = false;
                int idUsr = 0;

                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                {

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT Id, Nome, Nivel, Avatar FROM UTILIZADORES WHERE Username LIKE @USER AND Password LIKE @PASS AND Id > 0", sqlConn))
                    {
                        sqlCmd.Parameters.Add("@USER", SqlDbType.NVarChar).Value = txtUsername.Value;
                        sqlCmd.Parameters.Add("@PASS", SqlDbType.NVarChar).Value = txtPassword.Value;

                        sqlConn.Open();

                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                            if (dr.Read())
                            {
                                idUsr = Convert.ToInt32(dr[0]);
                                sessaoIniciou = VARS.UserSession.FazLogin(idUsr, Diversos.RemoveAcentos(Convert.ToString(dr[1])), Convert.ToInt32(dr[2]), Convert.ToInt32(dr[3]));
                            }
                            else
                                throw new Exception("Nenhum utilizador encontrado!");

                        sqlConn.Close();
                    }

                    if (sessaoIniciou)
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("UPDATE UTILIZADORES SET UltimoAcesso = GETDATE() WHERE Id = @ID", sqlConn))
                        {
                            sqlCmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = idUsr;

                            sqlConn.Open();

                            if (sqlCmd.ExecuteNonQuery() != 1)
                                throw new Exception("Erro ao atualizar datetime acesso em base de dados!");

                            sqlConn.Close();
                        }
                    }
                }

                if (sessaoIniciou)
                      Response.Redirect("/index.aspx");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("btnLogin_ServerClick(): " + ex.Message);
                lblError.Visible = true;
            }

        }
    }
}