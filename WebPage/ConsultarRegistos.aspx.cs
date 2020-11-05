using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Data;
using System.Text;
using OfficeOpenXml.Drawing.Chart;
using System.Net;
using System.IO;

namespace WebPage
{
    public partial class ConsultarRegistos : System.Web.UI.Page
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

        public DateTime[] dateTimes = new DateTime[2];

        public static List<Maquina.PontosGraph.DataPoint> LAST_SEARCH_STATS
        {
            get
            {
                if (HttpContext.Current.Session["LAST_SEARCH_STATS"] == null)
                    HttpContext.Current.Session["LAST_SEARCH_STATS"] = new List<Maquina.PontosGraph.DataPoint>();

                return (List<Maquina.PontosGraph.DataPoint>)HttpContext.Current.Session["LAST_SEARCH_STATS"];
            }
            set
            {
                HttpContext.Current.Session["LAST_SEARCH_STATS"] = value;
            }
        }

        public static List<Maquina.PontosGraph.DataPoint> LAST_CLASS_TABLE
        {
            get
            {
                if (HttpContext.Current.Session["LAST_CLASS_TABLE"] == null)
                    HttpContext.Current.Session["LAST_CLASS_TABLE"] = new List<Maquina.PontosGraph.DataPoint>();

                return (List<Maquina.PontosGraph.DataPoint>)HttpContext.Current.Session["LAST_CLASS_TABLE"];
            }
            set
            {
                HttpContext.Current.Session["LAST_CLASS_TABLE"] = value;
            }
        }

        public static Classificacoes.Classificacao LAST_CLASS
        {
            get
            {
                if (HttpContext.Current.Session["LAST_CLASS"] == null)
                    HttpContext.Current.Session["LAST_CLASS"] = Classificacoes.Classificacao.Undefined;

                return (Classificacoes.Classificacao)HttpContext.Current.Session["LAST_CLASS"];
            }
            set
            {
                HttpContext.Current.Session["LAST_CLASS"] = value;
            }
        }


        public bool OrdUpdateChart = false;

        public Maquina Maquina
        {
            get
            {
                return VARS.Maquinas.FirstOrDefault(x => x.Id == ID);
            }
        }

        public bool IsViewModeOnly
        {
            get
            {
                return (Master as MasterPage).IsViewModeOnly;
            }
        }
        public bool SessaoIniciada
        {
            get
            {
                return VARS.UserSession.SessaoIniciada;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Debug.WriteLine("ConsultarRegistos Page_Load");

            string id = Request.QueryString["ID"];

            if (!string.IsNullOrEmpty(id) && Diversos.IsNumeric(id))
                ID = Convert.ToInt32(id);

            if (this.Maquina == null || this.Maquina.Id != ID)
                Response.Redirect("/notfound.html", true);

            if (!IsPostBack)
            {

                this.Title = "Controlo de Produção - " + this.Maquina.Nome;
                this.lblNomeMaquina.InnerText = this.Maquina.Nome;

                //vai buscar as datas 
                try
                {
                    string dtInicio = Request.QueryString["DtInicio"];
                    if (!string.IsNullOrEmpty(dtInicio))
                        if (!DateTime.TryParse(dtInicio, out dateTimes[0]))
                            throw new Exception("DtInicio parse error");

                    string dtFim = Request.QueryString["DtFim"];
                    if (!string.IsNullOrEmpty(dtFim))
                        if (!DateTime.TryParse(dtFim, out dateTimes[1]))
                            throw new Exception("DtFim parse error");

                    if (dateTimes[0] == DateTime.MinValue || dateTimes[1] == DateTime.MinValue)
                        throw new Exception("Datetime == MinValue");

                    if (dateTimes[0] >= dateTimes[1])
                        throw new Exception("DtInicio >= DtFim");

                    string textOfst = Request.QueryString["Offset"];
                    if (string.IsNullOrEmpty(textOfst))
                        textOfst = "0";

                    if (Diversos.IsNumeric(textOfst))
                    {
                        double value = Convert.ToDouble(textOfst.Replace(".", ","));

                        if (value != 0)
                            txtOffset.Value = Convert.ToDouble(textOfst.Replace(".", ",")).ToString("0.000");
                    }

                    if (Diversos.CheckForInternetConnection())
                    {
                        idQrCode.Src = @"https://chart.googleapis.com/chart?chs=190x190&cht=qr&chl=" + VARS.DOMINIO +
                             "ConsultarRegistos.aspx?id=" + id + "%26DtInicio=" + dtInicio + "%26DtFim=" + dtFim + "%26Offset=" + textOfst + "%26key=" +
                              new GetKeys().Encripta(dtFim + id.ToString() + textOfst.Replace(".", ",") + dtInicio).Replace("+", "X").Replace("=", "Y"); ;
                        idDisplayQrCode.Style.Add("display", "block");
                    }

                    if (!this.SessaoIniciada)
                    { //se nao tem sessao iniciada esconde o txt do offset e botao de refresh
                        txtOffset.Disabled = true;
                        txtOffset.Style.Add("display", "none");
                        btnRefresh.HRef = "#";
                        btnRefresh.Style.Add("display", "none");
                    }


                    this.OrdUpdateChart = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Erro ao obter datas: " + ex.Message);

                    dateTimes[0] = DateTime.Today;
                    dateTimes[1] = DateTime.Today.AddDays(1).AddSeconds(-1);
                    //   idQrCode.Style.Add("display", "none");

                }
            }
        }

        [WebMethod]
        public static string GetGraph(string id, string DtInicio, string DtFim, string Offset)
        {
            //Populating a DataTable from database.
            DataTable dt = new DataTable();
            dt.Columns.Add("Diametro", typeof(double));
            dt.Columns.Add("Classificacao", typeof(int));
            dt.Columns.Add("Timestamp", typeof(string));

            int maxOfRecords = 500;

            if (string.IsNullOrWhiteSpace(Offset) || !Diversos.IsNumeric(Offset))
                Offset = "0";

            try
            {
                Maquina maquina = VARS.Maquinas.First(x => x.Id == Convert.ToInt32(id));

                //   int lastSec = -1;

                if (maquina == null)
                    throw new Exception("máquina não encontrada!");
                else
                    if (maquina.GraficoPontos.GetPoints(DateTime.Parse(DtInicio), DateTime.Parse(DtFim).AddMinutes(1).AddSeconds(-1), Convert.ToDouble(Offset.Replace(".", ","))))
                {
                    //get stats
                    LAST_SEARCH_STATS.Clear();
                    //get class
                    LAST_CLASS_TABLE.Clear();

                    LAST_CLASS = Classificacoes.Classificacao.Undefined;

                    long numPointsClasse2 = 0;
                    long numPointsClasse3 = 0;
                    long numPointsNC = 0;

                    Classificacoes.Classificacao lastClass = Classificacoes.Classificacao.Undefined;

                    int increment = (int)Math.Ceiling((double)maquina.GraficoPontos.Pontos.Count / maxOfRecords);

                    for (int i = 0; i < maquina.GraficoPontos.Pontos.Count; i++)
                    {
                        if (i % increment == 0)
                            dt.Rows.Add(Math.Round(maquina.GraficoPontos.Pontos[i].Value, 3), (int)maquina.GraficoPontos.Pontos[i].Classe, maquina.GraficoPontos.Pontos[i].DataHora.ToString("dd/MM/yyyy HH:mm:ss"));

                        if (LAST_CLASS_TABLE.Count < 1000)
                            if (maquina.GraficoPontos.Pontos[i].Classe != Classificacoes.Classificacao.Conforme)
                            {
                                if (maquina.GraficoPontos.Pontos[i].Classe != lastClass)
                                    LAST_CLASS_TABLE.Add(maquina.GraficoPontos.Pontos[i]);

                                lastClass = maquina.GraficoPontos.Pontos[i].Classe;
                            }

                        if (maquina.GraficoPontos.Pontos[i].Classe == Classificacoes.Classificacao.Classe2)
                            numPointsClasse2++;
                        else if (maquina.GraficoPontos.Pontos[i].Classe == Classificacoes.Classificacao.Classe3)
                            numPointsClasse3++;
                        else if (maquina.GraficoPontos.Pontos[i].Classe == Classificacoes.Classificacao.NaoConforme)
                            numPointsNC++;
                    }

                    //analise a conformidade da bobine
                    if (numPointsNC > 0 || numPointsClasse3 > maquina.Classe.MaxPontosClasse3 || numPointsClasse2 > maquina.Classe.MaxPontosClasse2)
                        LAST_CLASS = Classificacoes.Classificacao.NaoConforme;
                    else if (numPointsClasse3 > 0)
                        LAST_CLASS = Classificacoes.Classificacao.Classe3;
                    else if (numPointsClasse2 > 0)
                        LAST_CLASS = Classificacoes.Classificacao.Classe2;
                    else
                        LAST_CLASS = Classificacoes.Classificacao.Conforme;

                    //get stats
                    double aux = Math.Round(maquina.GraficoPontos.Pontos.Select(x => x.Value).Average(), 3);
                    LAST_SEARCH_STATS.Add(new Maquina.PontosGraph.DataPoint(aux, DateTime.Now, maquina.Classe.GetClassificacao(aux)));
                    LAST_SEARCH_STATS.Add(maquina.GraficoPontos.Minimum);
                    LAST_SEARCH_STATS.Add(maquina.GraficoPontos.Maximum);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetGraph(): " + ex.Message);

                return string.Empty;
            }

            return VARS.ConvertDataTabletoString(dt);
        }

        [WebMethod]
        public static string GetKey(string DtInicio, string DtFim, string Offset)
        {
            if (string.IsNullOrWhiteSpace(Offset))
                Offset = "0";

            return new GetKeys().Encripta(DtFim + ID.ToString() + Offset.Replace(".", ",") + DtInicio).Replace("+", "X").Replace("=", "Y");
        }

        [WebMethod]
        public static string GetLastAVG()
        {
            //Populating a DataTable from database.
            DataTable dt = new DataTable();
            dt.Columns.Add("Media_D", typeof(double));
            dt.Columns.Add("Media_C", typeof(int));
            dt.Columns.Add("Media_T", typeof(string));

            dt.Columns.Add("Min_D", typeof(double));
            dt.Columns.Add("Min_C", typeof(int));
            dt.Columns.Add("Min_T", typeof(string));

            dt.Columns.Add("Max_D", typeof(double));
            dt.Columns.Add("Max_C", typeof(int));
            dt.Columns.Add("Max_T", typeof(string));

            if (LAST_SEARCH_STATS.Count == 3)
                dt.Rows.Add(LAST_SEARCH_STATS[0].Value, (int)LAST_SEARCH_STATS[0].Classe, LAST_SEARCH_STATS[0].DataHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    LAST_SEARCH_STATS[1].Value, (int)LAST_SEARCH_STATS[1].Classe, LAST_SEARCH_STATS[1].DataHora.ToString("dd/MM/yyyy HH:mm:ss"),
                    LAST_SEARCH_STATS[2].Value, (int)LAST_SEARCH_STATS[2].Classe, LAST_SEARCH_STATS[2].DataHora.ToString("dd/MM/yyyy HH:mm:ss"));

            return VARS.ConvertDataTabletoString(dt);

        }
        [WebMethod]
        public static string GetLastClassificacoes()
        {
            //Populating a DataTable from database.
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Diametro", typeof(double));
            dt.Columns.Add("DataHora", typeof(string));

            for (int i = 0; i < LAST_CLASS_TABLE.Count; i++)
                dt.Rows.Add((i + 1), LAST_CLASS_TABLE[i].Value, LAST_CLASS_TABLE[i].DataHora.ToString("dd/MM/yyyy HH:mm:ss"));

            return VARS.ConvertDataTabletoString(dt);

        }

        [WebMethod]
        public static int GetLastClasse()
        {
            return (int)LAST_CLASS;
        }

        [WebMethod]
        public static string GetAlertasProducao(string id, string DtInicio, string DtFim, string Offset)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Offset) || !Diversos.IsNumeric(Offset))
                    Offset = "0";

                Maquina maquina = VARS.Maquinas.First(x => x.Id == Convert.ToInt32(id));

                //Populating a DataTable from database.
                DataTable dt = new DataTable();

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT TOP 1000 Alertas.Id, TipoAlerta.Tipo, (Alertas.Diametro + @OFFSET) AS 'Diametro', FORMAT(Alertas.DataHora, 'dd/MM/yyyy HH:mm:ss') AS 'DataHora' FROM Alertas ");
                strQuery.Append("INNER JOIN TipoAlerta ON Alertas.IdAlerta = TipoAlerta.Id ");
                strQuery.Append("WHERE Alertas.IdMaquina = @IdMaquina AND Alertas.DataHora BETWEEN @DATA1 AND @DATA2 ");
                strQuery.Append("ORDER BY Alertas.Id");

                if (maquina == null)
                    throw new Exception("máquina não encontrada!");
                else
                    using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                    using (SqlCommand sqlCmd = new SqlCommand(strQuery.ToString(), sqlConn))
                    {
                        sqlCmd.Parameters.Add("@IdMaquina", SqlDbType.TinyInt).Value = Convert.ToInt32(id);
                        sqlCmd.Parameters.AddWithValue("@OFFSET", Convert.ToDouble(Offset.Replace(".", ",")));

                        sqlCmd.Parameters.Add("@DATA1", SqlDbType.DateTime).Value = DateTime.Parse(DtInicio);
                        sqlCmd.Parameters.Add("@DATA2", SqlDbType.DateTime).Value = DateTime.Parse(DtFim);

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

        protected void btnExportExcel_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage pck = new ExcelPackage();

                List<double> columnWiths = new List<double>() { 15.43, 17.86, 15.43, 9.29, 13.43, 17.86, 0.83, 12.71, 9.29, 13.43, 11, 0.83, 13.43, 13.71, 15, 16, 0.83, 11, 9.29, 13.43 };

                DateTime lastDate = DateTime.MinValue.Date;

                int linha = 8;
                int wsIndex = 0;

                DateTime startDt = DateTime.MinValue;
                DateTime endDt = DateTime.MinValue;

                foreach (Maquina.PontosGraph.DataPoint ponto in this.Maquina.GraficoPontos.Pontos)
                {
                    ExcelWorksheet ws = null;

                    #region é nova data
                    if (ponto.DataHora.Date != lastDate)
                    {

                        // Faz os cálculos da folha anterior antes de transicionar para a seguinte
                        if (pck.Workbook.Worksheets.Count > 0)
                            this.DoWorksheetCalcs(pck.Workbook.Worksheets[wsIndex], linha, startDt, endDt);

                        #region Cria uma nova folha

                        ws = pck.Workbook.Worksheets.Add(ponto.DataHora.Date.ToString("dd/MM/yyyy"));
                        wsIndex = pck.Workbook.Worksheets.Count - 1;

                        ws.DefaultRowHeight = 15;

                        startDt = ponto.DataHora;

                        // Tamanho das Colunas
                        for (int i = 0; i < columnWiths.Count; i++)
                            ws.Column(i + 1).Width = columnWiths[i];


                        var pic = ws.Drawings.AddPicture("QR Code", Diversos.DownloadImagemFromWeb(idQrCode.Src));
                        pic.SetPosition(0, 1, 0, 18);

                        //  qr code
                        var cell = ws.Cells["A11:B12"];
                        cell.Value = "Este relatório pode ser consultado acedendo ao QR code acima";
                        cell.Merge = true;
                        cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        //Titulo

                        cell = ws.Cells["C1:T4"];
                        this.SetCellStyle(cell, "Carta de Controlo de Produção", true, 22, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center);
                        cell.Merge = true;

                        //detalhes relatorio
                        cell = ws.Cells["C6:F6"];
                        this.SetCellStyle(cell, "Detalhes do Relatório", true, 12, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center);
                        cell.Merge = true;

                        ws.Cells["C7"].Value = "Máquina";
                        ws.Cells["C8"].Value = "Primeiro Registo";
                        ws.Cells["C9"].Value = "Último Registo";
                        ws.Cells["C10"].Value = "Total Registos";
                        ws.Cells["C11"].Value = "Classificação";

                        cell = ws.Cells["D7:F7"];
                        this.SetCellStyle(cell, Maquina.Nome, true, 11, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center);
                        cell.Merge = true;

                        //calculos classe
                        for (int i = 0; i < 2; i++)
                        {
                            ws.Cells["E" + (8 + i).ToString()].Formula = "=IF(AND(D" + (8 + i).ToString() + " >= O8,D" + (8 + i).ToString() + " <= P8),\"Conforme\", IF(AND(D" + (8 + i).ToString() + " >= O9,D" + (8 + i).ToString() + " < P9),\"Classe 2\", IF(AND(D" + (8 + i).ToString() + " >= O10, D" + (8 + i).ToString() + " < P10),\"Classe 3\",\"Não Conforme\")))";
                            ws.Cells["E" + (8 + i).ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        ws.Cells["D11:F11"].Merge = true;

                        cell = ws.Cells["C6:F11"];
                        SetBorderCells(cell, ExcelBorderStyle.Thin);  //border around table
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);


                        //medias moveis
                        cell = ws.Cells["H6:K6"];
                        this.SetCellStyle(cell, "Médias Móveis", true, 12, ExcelHorizontalAlignment.Center);
                        cell.Merge = true;

                        this.SetCellStyle(ws.Cells["I7"], "Valor", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["J7"], "Classe", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["K7"], "Hora", true, 12, ExcelHorizontalAlignment.Center);

                        ws.Cells["H8"].Value = "Valor Médio";
                        ws.Cells["H9"].Value = "Valor Mínimo";
                        ws.Cells["H10"].Value = "Valor Máximo";
                        ws.Cells["H11"].Value = "Amplitude";

                        //calculos classe
                        for (int i = 0; i < 3; i++)
                        {
                            ws.Cells["J" + (8 + i).ToString()].Formula = "=IF(AND(I" + (8 + i).ToString() + " >= O8,I" + (8 + i).ToString() + " <= P8),\"Conforme\", IF(AND(I" + (8 + i).ToString() + " >= O9,I" + (8 + i).ToString() + " < P9),\"Classe 2\", IF(AND(I" + (8 + i).ToString() + " >= O10, I" + (8 + i).ToString() + " < P10),\"Classe 3\",\"Não Conforme\")))";
                            ws.Cells["J" + (8 + i).ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }

                        cell = ws.Cells["H6:K11"];
                        SetBorderCells(cell, ExcelBorderStyle.Thin);  //border around table
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                        //Pontos fora de especificacao
                        cell = ws.Cells["M6:P6"];
                        this.SetCellStyle(cell, "Pontos Fora de Especificação", true, 12, ExcelHorizontalAlignment.Center);
                        cell.Merge = true;

                        this.SetCellStyle(ws.Cells["N7"], "Nº Pontos", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["O7"], "Limite Mínimo", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["P7"], "Limite Máximo", true, 12, ExcelHorizontalAlignment.Center);

                        ws.Cells["M8"].Value = "Conforme";
                        ws.Cells["M9"].Value = "Classe 2";
                        ws.Cells["M10"].Value = "Classe 3";
                        ws.Cells["M11"].Value = "Não Conforme";

                        //limit conform
                        ws.Cells["O8"].Value = Maquina.Classe.Conforme.Min;
                        ws.Cells["P8"].Value = Maquina.Classe.Conforme.Max;

                        //limit CLASSE 2
                        ws.Cells["O9"].Value = Maquina.Classe.Classe2.Min;
                        ws.Cells["P9"].Value = Maquina.Classe.Classe2.Max;

                        //limit CLASS 3
                        ws.Cells["O10"].Value = Maquina.Classe.Classe3.Min;
                        ws.Cells["P10"].Value = Maquina.Classe.Classe3.Max;

                        ////limit NC
                        //this.SetCellStyle(ws.Cells["O11"], "---", false, 11, ExcelHorizontalAlignment.Center);
                        //this.SetCellStyle(ws.Cells["P11"], "---", false, 11, ExcelHorizontalAlignment.Center);

                        cell = ws.Cells["M6:P11"];
                        SetBorderCells(cell, ExcelBorderStyle.Thin);  //border around table
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                        //Listagem de valores
                        cell = ws.Cells["R6:T6"];
                        this.SetCellStyle(cell, "Listagem de Pontos", true, 12, ExcelHorizontalAlignment.Center);
                        cell.Merge = true;

                        this.SetCellStyle(ws.Cells["R7"], "Hora", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["S7"], "Valor", true, 12, ExcelHorizontalAlignment.Center);
                        this.SetCellStyle(ws.Cells["T7"], "Classe", true, 12, ExcelHorizontalAlignment.Center);

                        linha = 8;
                        #endregion
                    }
                    lastDate = ponto.DataHora.Date;
                    #endregion

                    //add a new point

                    ws = pck.Workbook.Worksheets[wsIndex];

                    //data hora
                    var cells = ws.Cells["R" + linha.ToString()];
                    cells.Value = ponto.DataHora;
                    cells.Style.Numberformat.Format = "hh:mm:ss";

                    //VALUE
                    ws.Cells["S" + linha.ToString()].Value = ponto.Value;

                    //CLASSIFICACAO
                    cells = ws.Cells["T" + linha.ToString()];
                    cells.Formula = "=IF(AND(S" + linha.ToString() + " >= O8,S" + linha.ToString() + " <= P8),\"Conforme\", IF(AND(S" + linha.ToString() + ">= O9,S" + linha.ToString() + "< P9),\"Classe 2\", IF(AND(S" + linha.ToString() + ">= O10, S" + linha.ToString() + "< P10),\"Classe 3\",\"Não Conforme\")))";
                    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    Color backColor = Color.White;

                    switch (ponto.Classe)
                    {
                        case Classificacoes.Classificacao.Conforme: backColor = Color.PaleGreen; break;
                        case Classificacoes.Classificacao.Classe2: backColor = Color.Yellow; break;
                        case Classificacoes.Classificacao.Classe3: backColor = Color.Orange; break;
                        case Classificacoes.Classificacao.NaoConforme: backColor = Color.Red; break;
                    }

                    if (backColor != Color.White)
                        this.SetCellsBackcolor(ws.Cells["R" + linha.ToString() + ":T" + linha.ToString()], backColor);

                    endDt = ponto.DataHora;


                    linha++;
                }

                //obrigatoriamente faz os calculos na ultima folha
                this.DoWorksheetCalcs(pck.Workbook.Worksheets[wsIndex], linha, startDt, endDt);


                #region EXPORT FILE TO CLIENT

                //EXPORT FILE TO CLIENT


                Response.Clear();
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=carta controlo.xlsx");
                Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                Response.End();
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine("btnExportExcel_ServerClick(): " + ex.Message);
            }
        }

        private void DoWorksheetCalcs(ExcelWorksheet ws, int numOfLines, DateTime startDt, DateTime endDt)
        {
            //gerar gráfico

            //1º registo | ultimo registo
            var cell = ws.Cells["D8"];

            //1º registo
            ws.Cells["F8"].Formula = "=R8"; //dt
            ws.Cells["F8"].Style.Numberformat.Format = "dd-MM-yyyy hh:mm:ss";
            ws.Cells["D8"].Formula = "=S8"; //valor


            //ULTIMO REGISTO
            ws.Cells["F9"].Formula = "=R" + (numOfLines - 1).ToString(); //dt
            ws.Cells["F9"].Style.Numberformat.Format = "dd-MM-yyyy hh:mm:ss";

            ws.Cells["D9"].Formula = "=S" + (numOfLines - 1).ToString(); //valor

            List<string> auxL = new List<string> { "D", "E", "F" };

            for (int i = 8; i <= 9; i++)
                foreach (string letra in auxL)
                {
                    cell = ws.Cells[letra + i.ToString()];
                    this.ApplyBackColorConditionFormating(ws, cell, "=E" + i.ToString() + "=\"Conforme\"", Color.PaleGreen);
                    this.ApplyBackColorConditionFormating(ws, cell, "=E" + i.ToString() + "=\"Classe 2\"", Color.Yellow);
                    this.ApplyBackColorConditionFormating(ws, cell, "=E" + i.ToString() + "=\"Classe 3\"", Color.Orange);
                    this.ApplyBackColorConditionFormating(ws, cell, "=E" + i.ToString() + "=\"Não Conforme\"", Color.Red);
                }


            //total
            ws.Cells["D10"].Formula = "=COUNT(S8:S" + (numOfLines - 1).ToString() + ")";


            //medias MOVEIS
            ws.Cells["I8"].Formula = "=AVERAGE(S8:S" + (numOfLines - 1).ToString() + ")";
            ws.Cells["I9"].Formula = "=MIN(S8:S" + (numOfLines - 1).ToString() + ")";
            ws.Cells["I10"].Formula = "=MAX(S8:S" + (numOfLines - 1).ToString() + ")";
            ws.Cells["I11"].Formula = "=ABS(I10-I9)"; //range

            //INDEX(F3: F11, MATCH(MAX(C3: C11), C3: C11, 0))
            ws.Cells["K9"].Formula = "=INDEX(R8:R" + (numOfLines - 1).ToString() + ",MATCH(I9,S8:S" + (numOfLines - 1).ToString() + ",0))";
            ws.Cells["K9"].Style.Numberformat.Format = "hh:mm:ss";
            ws.Cells["K10"].Formula = "=INDEX(R8:R" + (numOfLines - 1).ToString() + ",MATCH(I10,S8:S" + (numOfLines - 1).ToString() + ",0))";
            ws.Cells["K10"].Style.Numberformat.Format = "hh:mm:ss";

            auxL = new List<string> { "I", "J", "K" };

            for (int i = 8; i <= 10; i++)
                foreach (string letra in auxL)
                {
                    cell = ws.Cells[letra + i.ToString()];
                    this.ApplyBackColorConditionFormating(ws, cell, "=J" + i.ToString() + "=\"Conforme\"", Color.PaleGreen);
                    this.ApplyBackColorConditionFormating(ws, cell, "=J" + i.ToString() + "=\"Classe 2\"", Color.Yellow);
                    this.ApplyBackColorConditionFormating(ws, cell, "=J" + i.ToString() + "=\"Classe 3\"", Color.Orange);
                    this.ApplyBackColorConditionFormating(ws, cell, "=J" + i.ToString() + "=\"Não Conforme\"", Color.Red);
                }

            //pontos fora de especificcao
            ws.Cells["N8"].Formula = "=COUNTIF(T8:T" + (numOfLines - 1).ToString() + ",\"Conforme\")";
            ws.Cells["N9"].Formula = "=COUNTIF(T8:T" + (numOfLines - 1).ToString() + ",\"Classe 2\")";
            ws.Cells["N10"].Formula = "=COUNTIF(T8:T" + (numOfLines - 1).ToString() + ",\"Classe 3\")";
            ws.Cells["N11"].Formula = "=COUNTIF(T8:T" + (numOfLines - 1).ToString() + ",\"Não Conforme\")";

            //classificacao
            cell = ws.Cells["D11"];
            cell.Formula = "=IF(N11>0,\"Não Conforme\",IF(OR(N10>=1,N9>1),\"Classe 3\", IF(N9=1,\"Classe 2\",\"Conforme\")))";
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Bold = true;

            //Green
            this.ApplyForeColorConditionFormating(ws, cell, "=D11=\"Conforme\"", Color.Green);

            //red
            this.ApplyForeColorConditionFormating(ws, cell, "=D11=\"Não Conforme\"", Color.Red);

            //DarkOrange
            this.ApplyForeColorConditionFormating(ws, cell, "=D11=\"Classe 3\"", Color.DarkOrange);

            //orange
            this.ApplyForeColorConditionFormating(ws, cell, "=D11=\"Classe 2\"", Color.Orange);

            //"=IF(AND(I" + (8 + i).ToString() + " >= O8,I" + (8 + i).ToString() + " <= P8),\"Conforme\", IF(AND(I" + (8 + i).ToString() + " >= O9,I" + (8 + i).ToString() + " < P9),\"Classe 2\", IF(AND(I" + (8 + i).ToString() + " >= O10, I" + (8 + i).ToString() + " < P10),\"Classe 3\",\"Não Conforme\")))";

            cell = ws.Cells["R6:T" + (numOfLines - 1).ToString()];
            SetBorderCells(cell, ExcelBorderStyle.Thin);  //border around table
            cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);


            //add chart 
            //create a new piechart of type Line
            ExcelLineChart lineChart = ws.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;

            if (!string.IsNullOrWhiteSpace(txtGraphTitle.Value))
                lineChart.Title.Text = txtGraphTitle.Value;
            else
                lineChart.Title.Text = this.Maquina.Nome + " - Produção de " + startDt.ToString("dd/MM/yyyy HH:mm") + " a " + endDt.ToString("dd/MM/yyyy HH:mm");

            lineChart.Title.Font.Size = 14;
            lineChart.Title.Font.Bold = true;

            lineChart.Series.Add(ws.Cells["S8:S" + (numOfLines - 1).ToString()], ws.Cells["R8:R" + (numOfLines - 1).ToString()]);

            //size of the chart
            lineChart.SetSize(1300, 500);

            //set the names of the legend
            lineChart.Series[0].Header = "Diâmetro";
            //position of the legend
            lineChart.Legend.Position = eLegendPosition.Bottom;

            //add the chart at cell B6
            lineChart.SetPosition(13, 0, 0, 25);


            #region Alertas de Produção


            try
            {
                int line = 42;

                bool firstRow = true;

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT Alertas.Id, TipoAlerta.Tipo, Alertas.Diametro, Alertas.DataHora FROM Alertas ");
                strQuery.Append("INNER JOIN TipoAlerta ON Alertas.IdAlerta = TipoAlerta.Id ");
                strQuery.Append("WHERE Alertas.IdMaquina = @IdMaquina AND Alertas.DataHora BETWEEN @DATA1 AND @DATA2 ");
                strQuery.Append("ORDER BY Alertas.Id");

                if (this.Maquina == null)
                    throw new Exception("máquina não encontrada!");
                else
                    using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                    using (SqlCommand sqlCmd = new SqlCommand(strQuery.ToString(), sqlConn))
                    {
                        sqlCmd.Parameters.Add("@IdMaquina", SqlDbType.TinyInt).Value = Convert.ToInt32(this.Maquina.Id);

                        sqlCmd.Parameters.Add("@DATA1", SqlDbType.DateTime).Value = startDt;
                        sqlCmd.Parameters.Add("@DATA2", SqlDbType.DateTime).Value = endDt;

                        sqlConn.Open();

                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                            while (dr.Read())
                            {
                                if (firstRow)
                                #region 1º registo
                                {
                                    cell = ws.Cells["A40:O40"];
                                    this.SetCellStyle(cell, "Alertas de Produção", true, 12, ExcelHorizontalAlignment.Center);
                                    cell.Merge = true;

                                    this.SetCellStyle(ws.Cells["A41"], "ID", true, 12, ExcelHorizontalAlignment.Center);

                                    cell = ws.Cells["B41:L41"];
                                    this.SetCellStyle(cell, "Descrição do Alerta", true, 12, ExcelHorizontalAlignment.Center);
                                    cell.Merge = true;

                                    this.SetCellStyle(ws.Cells["M41"], "Diâmetro", true, 12, ExcelHorizontalAlignment.Center);
                                    this.SetCellStyle(ws.Cells["N41"], "Classe", true, 12, ExcelHorizontalAlignment.Center);
                                    this.SetCellStyle(ws.Cells["O41"], "Hora", true, 12, ExcelHorizontalAlignment.Center);

                                    firstRow = false;
                                }
                                #endregion

                                ws.Cells["A" + line.ToString()].Value = Convert.ToInt32(dr[0]);

                                cell = ws.Cells["B" + line.ToString() + ":L" + line.ToString()];
                                cell.Value = Convert.ToString(dr[1]);
                                cell.Merge = true;

                                cell = ws.Cells["M" + line.ToString()];
                                cell.Value = Convert.ToDouble(dr[2]);
                                cell.Style.Numberformat.Format = "0.000";

                                switch (this.Maquina.Classe.GetClassificacao(Convert.ToDouble(dr[2])))
                                {
                                    case Classificacoes.Classificacao.Conforme:
                                        ws.Cells["N" + line.ToString()].Value = "Conforme";
                                        break;
                                    case Classificacoes.Classificacao.Classe2:
                                        ws.Cells["N" + line.ToString()].Value = "Classe 2";
                                        break;
                                    case Classificacoes.Classificacao.Classe3:
                                        ws.Cells["N" + line.ToString()].Value = "Classe 3";
                                        break;
                                    case Classificacoes.Classificacao.NaoConforme:
                                        ws.Cells["N" + line.ToString()].Value = "Não Conforme";
                                        break;
                                }

                                cell = ws.Cells["O" + line.ToString()];
                                cell.Value = Convert.ToDateTime(dr[3]);
                                cell.Style.Numberformat.Format = "hh:mm";

                                if (line % 2 == 0)
                                    this.SetCellsBackcolor(ws.Cells["A" + line.ToString() + ":O" + line.ToString()], Color.LightGray);

                                line++;
                            }


                    }

                if (!firstRow)
                {
                    //fecha a tabela
                    cell = ws.Cells["A40:O" + (line - 1).ToString()];
                    SetBorderCells(cell, ExcelBorderStyle.Thin);  //border around table
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("AlertasdeProducao(): " + ex.Message);

            }
            #endregion
        }

        private void ApplyForeColorConditionFormating(ExcelWorksheet ws, ExcelRange cell, string formula, Color color)
        {
            var conditionalFormattingRule01 = ws.ConditionalFormatting.AddExpression(cell);
            conditionalFormattingRule01.Formula = formula;
            conditionalFormattingRule01.Style.Font.Color.Color = color;
        }

        private void ApplyBackColorConditionFormating(ExcelWorksheet ws, ExcelRange cell, string formula, Color color)
        {
            var conditionalFormattingRule01 = ws.ConditionalFormatting.AddExpression(cell);
            conditionalFormattingRule01.Formula = formula;
            conditionalFormattingRule01.Style.Fill.PatternType = ExcelFillStyle.Solid;
            conditionalFormattingRule01.Style.Fill.BackgroundColor.SetColor(color);
        }

        private void SetBorderCells(ExcelRange cells, ExcelBorderStyle excelBorder)
        {
            cells.Style.Border.Top.Style = excelBorder;
            cells.Style.Border.Left.Style = excelBorder;
            cells.Style.Border.Right.Style = excelBorder;
            cells.Style.Border.Bottom.Style = excelBorder;

        }
        private void SetCellsBackcolor(ExcelRange cells, Color backColor)
        {
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(backColor);
        }

        private void SetCellStyle(ExcelRange cells, object value, bool bold = false, float size = 11, ExcelHorizontalAlignment excelHorizontalAlignment = ExcelHorizontalAlignment.Left, ExcelVerticalAlignment excelVerticalAlignment = ExcelVerticalAlignment.Distributed)
        {
            cells.Value = value;
            cells.Style.Font.Bold = bold;
            cells.Style.Font.Size = size;
            cells.Style.HorizontalAlignment = excelHorizontalAlignment; // Alignment  
            cells.Style.VerticalAlignment = excelVerticalAlignment; // Alignment  
        }

        public static string FormatDt(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd") + "T" + dt.ToString("HH:mm");
        }


    }

}