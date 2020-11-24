using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebPage
{
    public class VARS
    {
        public static string ConnectionString
        {
            get
            {

                if (!Debugger.IsAttached)
                    return @"data source=.,1433\SQLEXPRESS; Initial Catalog=CTRL_PROD; MultipleActiveResultSets=True; persist security info=False; UID=user; PWD=user;"; //Forma de conexão remota
                else
                {
                    return @"data source=192.168.33.7,1433\SQLEXPRESS; Initial Catalog=CTRL_PROD; MultipleActiveResultSets=True; persist security info=False; UID=user; PWD=user;"; //Forma de conexão remota
                }


                //DataBaseConnection
                //ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DBConnection"];

                //if (mySetting == null)
                //    throw new Exception("Database connection settings have not been set in Web.config file");

                //return mySetting.ConnectionString;
            }
        }

        public static bool DebugMode
        {
            get
            {
                return false;
                // return Debugger.IsAttached && VARS.SessaoOperador.SessaoIniciada;
            }
        }

        public static string Dominio = String.Empty;

        public static AppSession UserSession = new AppSession();

        public const int NUM_OF_DAYS_TO_SHOW_ALERT = -7;

        public static bool TemPermissao(int nivelMinimo)
        {
            if (nivelMinimo <= 0)
                return true;
            else
                return UserSession.Nivel >= nivelMinimo;
        }

        public static string ConvertDataTabletoString(System.Data.DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public static List<Maquina> Maquinas
        {
            get
            {
                if (lstMaqs == null || (lstMaqs != null && lstMaqs.Count == 0))
                    try
                    {
                        if (lstMaqs == null)
                            lstMaqs = new List<Maquina>();

                        lstMaqs.Clear();

                        using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM MAQUINAS WHERE Id > 0 ORDER BY Id", sqlConn))
                        {
                            sqlConn.Open();

                            using (SqlDataReader dr = sqlCmd.ExecuteReader())
                                while (dr.Read())
                                    lstMaqs.Add(new Maquina(dr));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("VARS - Loading Maquinas: " + ex.Message);
                    }
                return lstMaqs;
            }
            set
            {
                if (lstMaqs != null)
                    lstMaqs = value;
            }
        }


        private static List<Maquina> lstMaqs
        {
            get
            {
                if (HttpContext.Current.Session["lstMaqs"] == null)
                    HttpContext.Current.Session["lstMaqs"] = new List<Maquina>();

                return (List<Maquina>)HttpContext.Current.Session["lstMaqs"];
            }
            set
            {
                HttpContext.Current.Session["lstMaqs"] = value;
            }
        }
            
    //    public static string DOMINIO = @"http://producao3d.tucab.local/";

    }


    public class Maquina
    {
        public int Id { get; private set; } = 0;

        public string Nome { get; private set; } = string.Empty;

        public Classificacoes Classe { get; private set; }

        public string DataTable { get; private set; } = string.Empty;

        public string Observacoes { get; private set; } = string.Empty;

        public PontosGraph GraficoPontos { get; set; }

        public Maquina(int id, string strConexao)
        {
            this.AtualizaDados(id, strConexao);

            this.GraficoPontos = new PontosGraph(this);
        }

        public Maquina(SqlDataReader dr)
        {
            this.AtualizaDados(dr);
            this.GraficoPontos = new PontosGraph(this);
        }

        public void AtualizaDados(int id, string strConexao)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Maquinas WHERE Id = @Id", sqlConn))
                {
                    sqlCmd.Parameters.AddWithValue("@Id", id);

                    sqlConn.Open();

                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        if (dr.Read())
                            this.ReadSqlDataReader(dr);
                        else
                            throw new Exception("AtualizaDados(): sem dados lidos em base de dados para id '" + this.Id + "' !");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AtualizaDados(): " + ex.Message);
            }
        }

        public void AtualizaDados(SqlDataReader dr)
        {
            this.ReadSqlDataReader(dr);
        }

        private bool ReadSqlDataReader(SqlDataReader dr)
        {
            try
            {
                int index = 0;
                this.Id = Convert.ToInt32(dr[index]); index++;
                this.Nome = Convert.ToString(dr[index]); index++;

                int auxIndex = 0;
                this.Classe = new Classificacoes(dr, index, out auxIndex); index += auxIndex;
                this.DataTable = Convert.ToString(dr[index]); index++;
                this.Observacoes = Convert.ToString(dr[index]); index++;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadSqlDataReader(): " + ex.Message);
                return false;
            }
        }


        public class PontosGraph
        {
            public List<DataPoint> Pontos { get; private set; } = new List<DataPoint>();

            public DataPoint Minimum { get; private set; }
            public DataPoint Maximum { get; private set; }


            private Maquina maq = null;

            public PontosGraph(Maquina maq)
            {
                this.maq = maq;
            }

            public bool GetLastNPoints(int numOfPoints)
            {
                DateTime dtStart = DateTime.Now;

                try
                {
                    double max = double.MinValue;
                    double min = double.MaxValue;

                    this.Pontos.Clear();

                    using (SqlConnection sqlConnection = new SqlConnection(VARS.ConnectionString))
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT TOP " + numOfPoints + " Diametro, DtUpload FROM " + maq.DataTable + " ORDER BY ID DESC", sqlConnection))
                    {
                        sqlConnection.Open();

                        using (SqlDataReader dr = sqlCommand.ExecuteReader())
                            while (dr.Read())
                            {
                                DataPoint point = new DataPoint(Math.Round(Convert.ToDouble(dr[0]), 3), Convert.ToDateTime(dr[1]), maq.Classe.GetClassificacao(Convert.ToDouble(dr[0])));
                                this.Pontos.Add(point);

                                //get min
                                if (point.Value < min)
                                {
                                    this.Minimum = point;
                                    min = point.Value;
                                }

                                //get max
                                if (point.Value > max)
                                {
                                    this.Maximum = point;
                                    max = point.Value;
                                }

                            }
                    }

                    //altera a ordem do array
                    this.Pontos.Reverse();

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("GetLastNPoints(): " + ex.Message);
                    return false;

                }
                finally
                {
                    Debug.WriteLine("GetLastNPoints(): Elapsed Time " + Convert.ToInt32((DateTime.Now - dtStart).TotalMilliseconds) + " ms");

                }
            }

            public bool GetPoints(DateTime dtInicio, DateTime dtFim, double offset)
            {
                DateTime dtStart = DateTime.Now;

                try
                {
                    double max = double.MinValue;
                    double min = double.MaxValue;

                    this.Pontos.Clear();

                    using (SqlConnection sqlConnection = new SqlConnection(VARS.ConnectionString))
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT (Diametro + @OFFSET) AS 'Diametro', DtUpload FROM " + maq.DataTable + " WHERE DtUpload BETWEEN @DATA1 AND @DATA2 ORDER BY ID", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@OFFSET", offset);
                        sqlCommand.Parameters.AddWithValue("@DATA1", dtInicio);
                        sqlCommand.Parameters.AddWithValue("@DATA2", dtFim);

                        sqlConnection.Open();

                        using (SqlDataReader dr = sqlCommand.ExecuteReader())
                            while (dr.Read())
                                while (dr.Read())
                                {
                                    DataPoint point = new DataPoint(Math.Round(Convert.ToDouble(dr[0]), 3), Convert.ToDateTime(dr[1]), maq.Classe.GetClassificacao(Convert.ToDouble(dr[0])));
                                    this.Pontos.Add(point);

                                    //get min
                                    if (point.Value < min)
                                    {
                                        this.Minimum = point;
                                        min = point.Value;
                                    }

                                    //get max
                                    if (point.Value > max)
                                    {
                                        this.Maximum = point;
                                        max = point.Value;
                                    }

                                }
                    }


                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("GetLastNPoints(): " + ex.Message);
                    return false;

                }
                finally
                {
                    Debug.WriteLine("GetLastNPoints(): Elapsed Time " + Convert.ToInt32((DateTime.Now - dtStart).TotalMilliseconds) + " ms");

                }
            }

            public struct DataPoint
            {
                public DateTime DataHora;
                public double Value;

                public Classificacoes.Classificacao Classe;

                public DataPoint(double value, DateTime dataHora, Classificacoes.Classificacao classe)
                {
                    this.Value = value;
                    this.DataHora = dataHora;
                    this.Classe = classe;
                }
            }


        }

    }

    public class Classificacoes
    {
        public MinMax Conforme
        {
            get; private set;
        } = new MinMax();

        public MinMax Classe2
        {
            get; private set;
        }

        public int MaxPontosClasse2 = 0;

        public MinMax Classe3
        {
            get; private set;
        }

        public int MaxPontosClasse3 = 0;

        public int MaxPontosNC = 0;
        public TimeSpan SpTempoMaxPontosNC = new TimeSpan();


        public Classificacoes(SqlDataReader dr, int startOffset, out int index)
        {
            index = 0;

            this.Conforme = new MinMax(Convert.ToDouble(dr[index + startOffset]), Convert.ToDouble(dr[index + startOffset + 1])); index += 2;
            this.Classe2 = new MinMax(Convert.ToDouble(dr[index + startOffset]), Convert.ToDouble(dr[index + startOffset + 1])); index += 2;
            this.MaxPontosClasse2 = Convert.ToInt32(dr[index + startOffset]); index++;
            this.Classe3 = new MinMax(Convert.ToDouble(dr[index + startOffset]), Convert.ToDouble(dr[index + startOffset + 1])); index += 2;
            this.MaxPontosClasse3 = Convert.ToInt32(dr[index + startOffset]); index++;

            this.MaxPontosNC = Convert.ToInt32(dr[index + startOffset]); index++;

            this.SpTempoMaxPontosNC = TimeSpan.FromSeconds(Convert.ToInt32(dr[index + startOffset])); index++;

        }

        public Classificacao GetClassificacao(double value)
        {
            if (this.Conforme.InRange(value, true, true))
                return Classificacao.Conforme;
            else if (this.Classe2.InRange(value, true, false))
                return Classificacao.Classe2;
            else if (this.Classe3.InRange(value, false, false))
                return Classificacao.Classe3;
            else
                return Classificacao.NaoConforme;
        }

        public struct MinMax
        {
            public double Min;
            public double Max;

            public MinMax(double min, double max)
            {
                this.Min = Math.Round(min, 3);
                this.Max = Math.Round(max, 3);
            }

            public bool InRange(double value, bool incMin, bool incMax)
            {
                if (incMin && incMax)
                    return Diversos.InRange(Math.Round(value, 3), this.Min, this.Max);
                else if (!incMin && incMax)
                    return Math.Round(value, 3) > this.Min && Math.Round(value, 3) <= this.Max;
                else if (incMin && !incMax)
                    return Math.Round(value, 3) >= this.Min && Math.Round(value, 3) < this.Max;
                else
                    return Math.Round(value, 3) > this.Min && Math.Round(value, 3) < this.Max;

            }
        }

        public enum Classificacao
        {
            Undefined = 0,
            Conforme = 1,
            Classe2 = 2,
            Classe3 = 3,
            NaoConforme = 4
        }

    }


}