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
    public partial class ConfiguracoesAvancadasDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!VARS.UserSession.AdministratorPermission && !VARS.DebugMode)
                Response.Redirect("/notfound.html");


            if (!IsPostBack)
            {
                //    txtDt.Value = this.GetFormatedDateString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1));

                //atualiza listagem de maquinas
                selectionLinha.Items.Clear();

                foreach (Maquina maquina in VARS.Maquinas)
                    selectionLinha.Items.Add(new ListItem(maquina.Nome, maquina.DataTable));
 
                if (selectionLinha.Items.Count > 0)
                    selectionLinha.SelectedIndex = 0;
            }
        }


        [WebMethod]
        public static string GetDbUsedSpace()
        {
            try
            {
                const int MaxDbSizeMb = 10000;

                //Populating a DataTable from database.
                DataTable dt = new DataTable();
                dt.Columns.Add("EspacoUsado", typeof(int));
                dt.Columns.Add("EspacoLivre", typeof(int));
                dt.Columns.Add("PercUsado", typeof(int));
                dt.Columns.Add("PercLivre", typeof(int));


                StringBuilder strQuery = new StringBuilder();



                strQuery.AppendLine("SELECT ");
                strQuery.AppendLine("CAST(ROUND((SUM(a.total_pages) / 128.00), 2) AS NUMERIC(36, 2)) AS Total_MB ");
                strQuery.AppendLine("FROM sys.tables t ");
                strQuery.AppendLine("INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id ");
                strQuery.AppendLine("INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id ");
                strQuery.AppendLine("INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id ");
                strQuery.AppendLine("INNER JOIN sys.schemas s ON t.schema_id = s.schema_id ");
                strQuery.AppendLine("GROUP BY t.Name, s.Name, p.Rows ");
                strQuery.AppendLine("ORDER BY s.Name, t.Name ");

                double totalSize = 0;

                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                using (SqlCommand sqlCmd = new SqlCommand(strQuery.ToString(), sqlConn))
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        while (dr.Read()) totalSize += Convert.ToDouble(dr[0]);
                }

                int usedSpace = Convert.ToInt32(totalSize);
                int freeSpace = MaxDbSizeMb - usedSpace;



                int percFreeSpace = freeSpace * 100 / MaxDbSizeMb;
                int percUsedSpace = 100 - percFreeSpace;

                dt.Rows.Add(usedSpace, freeSpace, percFreeSpace, percUsedSpace);

                return VARS.ConvertDataTabletoString(dt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetDbUsedSpace(): " + ex.Message);
                return string.Empty;
            }
        }


        public string GetFormatedDateString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (selectionLinha.SelectedIndex < 0)
                    throw new Exception("sem itens selecionados!");

                //  SQLHelper.ExecuteNonQuery("DELETE FROM " + selectionLinha.Value + " WHERE DtUpload < @DtUpload", VARS.ConnectionString, new List<SqlParameter>() { new SqlParameter("@DtUpload", DateTime.Parse(txtDt.Value)) });
                SQLHelper.ExecuteNonQuery("TRUNCATE TABLE " + selectionLinha.Value, VARS.ConnectionString);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("btnDelete_ServerClick(): " + ex.Message);
            }

        }

    }
}