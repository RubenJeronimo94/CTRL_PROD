using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPage
{
    public partial class GestaoEmails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!VARS.UserSession.AdministratorPermission && !VARS.DebugMode)
                Response.Redirect("/notfound.html");
        }


        [WebMethod]
        public static bool Update(string strQuery)
        {
            return SQLHelper.ExecuteNonQuery(strQuery, VARS.ConnectionString) == 1;
        }

        [WebMethod]
        public static string GetTbl()
        {
            try
            {
                //Populating a DataTable from database.
                DataTable dt = new DataTable();

                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand("SELECT Id, Nome, Email, FORMAT(DtAlteracao, 'dd/MM/yyyy HH:mm:ss') as 'DtAlteracao', Ativo FROM Emails WHERE Id > 0 ORDER BY Id", sqlConn))
                using (SqlDataAdapter sda = new SqlDataAdapter(sqlCmd))
                    sda.Fill(dt);

                return VARS.ConvertDataTabletoString(dt);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetTbl(): " + ex.Message);
                return string.Empty;
            }
        }
    }
}