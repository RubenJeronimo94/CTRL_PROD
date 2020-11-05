using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using CTRL_PROD.Properties;
using System.Threading;
using System.Net;
using System.Globalization;
using static CTRL_PROD.VarsAuxiliares;

namespace CTRL_PROD
{
    public partial class CtrlMaquina : UserControl
    {
        private string strConexao = string.Empty;

        private bool isAlive = true;

        private bool modoSimulacaoLeitura = false;

        public int Id { get; private set; } = 0;


        public double Diametro { get; private set; } = 0;
        public DateTime LastTimeStamp { get; private set; } = DateTime.MinValue;
        public DateTime LastDbInsertTime { get; private set; } = DateTime.MinValue;

        public Classificacoes Classe { get; private set; }

        public bool ReadStatus { get; private set; } = false;
        public bool DbStatus { get; private set; } = false;

        public string Observacoes { get; private set; } = string.Empty;

        public int DgvSpRows { get; set; } = 100;

        private int ReadFrequency = 500;

        private int NumRegistosRepetidosAlarme = 20;


        public int TotalTime { get; private set; } = 0;
        public int TotalReadTime { get; private set; } = 0;
        public int DbInsertTime { get; private set; } = 0;

        private string ftpDir = @"ftp://192.168.1.101/usb1/trend/Diametro/";
        private string ftpUsername = "root";
        private string ftpPassword = "888888";

        private bool useAuthentication = true;


        private string dataTable = string.Empty;

        private List<DataPoint> points = new List<DataPoint>();
        private object lockPoints = new object();

        private FalhaConexao falhaConexao = null;


        private EnviaEmail mail = null;

        public CtrlMaquina()
        {
            InitializeComponent();
        }

        public CtrlMaquina(int idDb, string strConexao, string iniPath)
        {
            InitializeComponent();

            this.strConexao = strConexao;
            this.Id = idDb;

            if (!DesignMode)
            {
                #region Load Data from ini
                using (FicheiroINI ini = new FicheiroINI(iniPath))
                {
                    string machineName = "Maquina" + this.Id.ToString();

                    this.ftpDir = ini.RetornaINI(machineName, "ftpDir", this.ftpDir);
                    this.ftpUsername = ini.RetornaINI(machineName, "ftpUsername", this.ftpUsername);
                    this.ftpPassword = ini.RetornaINI(machineName, "ftpPassword", this.ftpPassword);
                    this.ReadFrequency = Convert.ToInt32(ini.RetornaINI(machineName, "ReadFrequency", this.ReadFrequency.ToString()));
                    this.DgvSpRows = Convert.ToInt32(ini.RetornaINI(machineName, "DgvSpRows", this.DgvSpRows.ToString()));
                    this.useAuthentication = ini.RetornaTrueFalseDeStringFicheiroINI(machineName, "useAuthentication", this.useAuthentication);

                    this.NumRegistosRepetidosAlarme = Convert.ToInt32(ini.RetornaINI(machineName, "NumRegistosRepetidosAlarme", this.NumRegistosRepetidosAlarme.ToString()));

                    this.falhaConexao = new FalhaConexao(Convert.ToInt32(ini.RetornaINI(machineName, "msToCommFailureAlarm", "5000")));
                }

                this.mail = new EnviaEmail(iniPath);

                #endregion

                this.AtualizaDados();

                tabControl1.TabPages[1].Text = "Últimos Pontos (" + Convert.ToInt32(this.Classe.SpTempoMaxPontosNC.TotalSeconds) + " sec)";

                new Thread(this.CicloTrabalho).Start();

                timer1.Enabled = true;
                timer1.Start();
            }

        }

        public void AtualizaDados()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this.strConexao))
                using (SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Maquinas WHERE Id = @Id", sqlConn))
                {
                    sqlCmd.Parameters.AddWithValue("@Id", this.Id);

                    sqlConn.Open();

                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                        if (dr.Read())
                        {
                            int index = 1;
                            txtNomeMaquina.Text = Convert.ToString(dr[index]); index++;

                            int auxIndex = 0;
                            this.Classe = new Classificacoes(dr, index, out auxIndex); index += auxIndex;
                            this.dataTable = Convert.ToString(dr[index]); index++;
                            this.Observacoes = Convert.ToString(dr[index]); index++;

                        }
                        else
                            throw new Exception("AtualizaDados(): sem dados lidos em base de dados para id '" + this.Id + "' !");
                }
            }
            catch (Exception ex)
            {
                this.AdicionaLog(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        public void AdicionaLog(string msg)
        {
            try
            {
                if (txtLog.InvokeRequired)
                    this.txtLog.Invoke((MethodInvoker)delegate
                    {
                        if (!string.IsNullOrWhiteSpace(txtLog.Text))
                            txtLog.AppendText(Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ": " + msg);
                        else
                            txtLog.AppendText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ": " + msg);
                    });
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtLog.Text))
                        txtLog.AppendText(Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ": " + msg);
                    else
                        txtLog.AppendText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ": " + msg);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AdicionaLog(): " + ex.Message);
            }
        }
        public void AdicionaLog(Exception ex)
        {
            this.AdicionaLog(ex.Message);
        }

        private void limparLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            this.AdicionaLog("Log clear by the user!");
        }

        private void exportarPFicheiroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFile = new SaveFileDialog())
                try
                {
                    saveFile.Title = "Guardar logs como";
                    saveFile.Filter = "Text File|.txt";
                    saveFile.FilterIndex = 0;
                    saveFile.FileName = "log";
                    saveFile.DefaultExt = ".txt";

                    if (saveFile.ShowDialog(this) == DialogResult.OK)
                        File.WriteAllText(saveFile.FileName, txtLog.Text);

                    if (File.Exists(saveFile.FileName))
                        MessageBox.Show("Dados gravados com sucesso em '" + saveFile.FileName + "'");
                    else
                        throw new Exception("Erro ao gravar ficheiro!");
                }
                catch (Exception ex)
                {
                    string strError = "ExportarTabela(): " + ex.Message + " ('" + saveFile.FileName + "')";
                    Debug.WriteLine(strError);
                    this.AdicionaLog(strError);
                    MessageBox.Show(ex.Message);
                }
        }

        private Classificacoes.Classificacao lastClass = Classificacoes.Classificacao.Undefined;
        private bool timerFpError = false;
        private bool newValueToInsert = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.ReadStatus && ledPLC.Image != Resources.ledGreen)
                    ledPLC.Image = Resources.ledGreen;
                else if (!this.ReadStatus && ledPLC.Image != Resources.ledRed)
                    ledPLC.Image = Resources.ledRed;

                if (this.DbStatus && ledDB.Image != Resources.ledGreen)
                    ledDB.Image = Resources.ledGreen;
                else if (!this.DbStatus && ledDB.Image != Resources.ledRed)
                    ledDB.Image = Resources.ledRed;


                txtDiametro.Text = this.ReadStatus ? this.Diametro.ToString("0.000") : "####";

                #region Classificação
                if (this.ReadStatus)
                {
                    Classificacoes.Classificacao classificacao = this.Classe.GetClassificacao(this.Diametro);

                    switch (classificacao)
                    {
                        case Classificacoes.Classificacao.Conforme:
                            txtClassificacao.Text = "Conforme";
                            txtClassificacao.ForeColor = Color.LimeGreen;
                            break;
                        case Classificacoes.Classificacao.Classe2:
                            txtClassificacao.Text = "Classe 2";
                            txtClassificacao.ForeColor = Color.Yellow;
                            break;
                        case Classificacoes.Classificacao.Classe3:
                            txtClassificacao.Text = "Classe 3";
                            txtClassificacao.ForeColor = Color.OrangeRed;
                            break;
                        case Classificacoes.Classificacao.NaoConforme:
                            txtClassificacao.Text = "Não Conforme";
                            txtClassificacao.ForeColor = Color.Red;
                            break;
                        default:
                            txtClassificacao.Text = "----";
                            txtClassificacao.ForeColor = Color.White;
                            break;
                    }

                    if (classificacao != Classificacoes.Classificacao.Conforme)
                    {
                        if (classificacao != this.lastClass)
                        {
                            if (dgvPoints.RowCount == this.DgvSpRows)
                                dgvPoints.Rows.RemoveAt(this.DgvSpRows - 1);

                            dgvPoints.Rows.Insert(0, this.Diametro.ToString("0.000"),
                                               DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                                txtClassificacao.Text);

                            dgvPoints.Rows[0].DefaultCellStyle.BackColor = txtClassificacao.ForeColor;

                            //     dgvPoints.Rows[dgvPoints.RowCount - 1].DefaultCellStyle.BackColor = txtClassificacao.ForeColor;

                            dgvPoints.ClearSelection();
                        }

                        this.lastClass = classificacao;
                    }
                }
                else
                {
                    txtClassificacao.ForeColor = txtDiametro.ForeColor = Color.White;
                    txtClassificacao.Text = "####";
                }
                #endregion


                if (tabControl1.SelectedIndex == 1 && this.points.Count > 0)
                    lock (this.lockPoints)
                    {
                        this.UpdateValueTextbox(lblMedia, this.points.Select(x => x.Value).Average());
                        this.UpdateValueTextbox(lblMin, this.points.Select(x => x.Value).Min());
                        this.UpdateValueTextbox(lblMax, this.points.Select(x => x.Value).Max());

                        lblTempoComm.Text = this.TotalTime.ToString() + " ms";
                        lblTempoComm.ForeColor = this.TotalReadTime < 250 ? Color.LimeGreen : Color.Red;

                        int pointsNC = this.points.Count(x => x.Classe != Classificacoes.Classificacao.Conforme);

                        lblNumPontosNC.Text = pointsNC.ToString();

                        if (pointsNC > 0 && pointsNC < this.Classe.MaxPontosNC)
                            lblNumPontosNC.ForeColor = Color.Orange;
                        else if (pointsNC >= this.Classe.MaxPontosNC)
                            lblNumPontosNC.ForeColor = Color.Red;
                        else
                            lblNumPontosNC.ForeColor = Color.White;

                        dgvHistLastPoints.Rows.Clear();


                        foreach (DataPoint point in this.points)
                        {
                            dgvHistLastPoints.Rows.Add(point.Value.ToString("0.000"),
                                                 point.DataHora.ToString("dd/MM/yyyy HH:mm:ss"));

                            dgvHistLastPoints.Rows[dgvHistLastPoints.RowCount - 1].DefaultCellStyle.BackColor = this.GetClassColor(point.Classe);

                            if (dgvHistLastPoints.RowCount == this.DgvSpRows)
                                break;
                        }

                    }
                else
                {
                    dgvHistLastPoints.Rows.Clear();

                    lblMedia.Text = lblMin.Text = lblMax.Text = lblTempoComm.Text = lblNumPontosNC.Text = "---";
                    lblMedia.ForeColor = lblMin.ForeColor = lblMax.ForeColor = lblTempoComm.ForeColor = lblNumPontosNC.ForeColor = Color.White;
                }

                this.timerFpError = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("timer1_Tick(): " + ex.Message);

                if (!this.timerFpError)
                    this.AdicionaLog("timer1_Tick(): " + ex.Message);

                this.timerFpError = true;
            }
        }

        private void UpdateValueTextbox(TextBox txt, double value)
        {
            Classificacoes.Classificacao classe = this.Classe.GetClassificacao(value);

            if (classe != Classificacoes.Classificacao.Undefined)
                txt.Text = value.ToString("0.000");
            else
                txt.Text = "----";

            txt.ForeColor = this.GetClassColor(classe);
        }

        private void TaskParalelaInsertAlarm()
        {
            while (this.falhaConexao.EmFalha && !this.falhaConexao.AlertaEmitido && this.isAlive)
            {
                if (this.falhaConexao.NecessarioEnviarAlarme && this.InsertNewAlarm(Alertas.ParagemMonitorizacao))
                    this.falhaConexao.AlertaEmitido = true;
                else
                    Thread.Sleep(100);
            }
        }

        public Color GetClassColor(Classificacoes.Classificacao classificacao)
        {
            switch (classificacao)
            {
                case Classificacoes.Classificacao.Conforme:
                    return Color.LimeGreen;
                case Classificacoes.Classificacao.Classe2:
                    return Color.Yellow;
                case Classificacoes.Classificacao.Classe3:
                    return Color.OrangeRed;
                case Classificacoes.Classificacao.NaoConforme:
                    return Color.Red;
                default:
                    return Color.White;
            }
        }

        private void CicloTrabalho()
        {
            bool firstCycle = true;
            DateTime lastReadTime = DateTime.MinValue;

            TimeSpan lastValuesTs = TimeSpan.Zero;
            int countNumOfRepeatedTS = 0;

            bool fpDadosDescontrolados = false;

            NetworkCredential credentials = new NetworkCredential(this.ftpUsername, this.ftpPassword);

            Random rnd = new Random();

            while (this.isAlive)
                try
                {
                    if ((DateTime.Now - lastReadTime).TotalMilliseconds >= this.ReadFrequency)
                    {
                        lastReadTime = DateTime.Now;


                        if (this.modoSimulacaoLeitura)
                        {
                            this.Diametro = Math.Round(rnd.Next(1600, 1800) / 1000.0, 3);

                            this.LastTimeStamp = DateTime.Now;
                            this.ReadStatus = !this.ReadStatus;
                        }
                        else
                        {
                            //fazer magia para ler as tags

                            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(this.GetLatestFile(credentials, this.ftpDir)));
                            //FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(@"ftp://192.168.1.101/usb1/trend/Diametro/20200909.csv");

                            DateTime dtStart = DateTime.Now;

                            //reqFTP.UsePassive = false;
                            reqFTP.UseBinary = true;
                            if (this.useAuthentication) reqFTP.Credentials = credentials;
                            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                            reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();

                            using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                try
                                {
                                    string line = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last();

                                    int auxStart = line.IndexOf(",");

                                    TimeSpan valueTS = TimeSpan.Parse(line.Substring(0, auxStart));

                                    if (lastValuesTs == valueTS)
                                        countNumOfRepeatedTS++;
                                    else
                                        countNumOfRepeatedTS = 0;

                                    lastValuesTs = valueTS;


                                    this.Diametro = Convert.ToDouble(line.Substring(auxStart + 1, line.Length - auxStart - 3).Replace(".", ","));

                                    if (countNumOfRepeatedTS >= this.NumRegistosRepetidosAlarme)
                                    {
                                        string msg = "Número de registos repetidos lidos foi ultrapassado!";
                                        if (countNumOfRepeatedTS == this.NumRegistosRepetidosAlarme)
                                        {
                                            this.InsertNewAlarm(Alertas.NumeroRegistosLidosUltrapassado);
                                            throw new Exception(msg);

                                        }
                                    }
                                    else
                                    {
                                        if (!this.ReadStatus)
                                        {
                                            if (this.falhaConexao.AlertaEmitido || firstCycle)
                                                this.InsertNewAlarm(Alertas.InicioMonitorizacao);

                                            this.AdicionaLog("Leitura iniciada com sucesso!");
                                        }
                                        this.ReadStatus = true;
                                        this.falhaConexao.LimpaDados();
                                    }


                                }
                                catch (Exception ex)
                                {
                                    if (this.ReadStatus && !this.falhaConexao.EmFalha)
                                    {
                                        this.falhaConexao.FalhaLeitura();

                                        //iniciar thread para só dar o alarme algum tempo depois
                                        new Thread(this.TaskParalelaInsertAlarm).Start();

                                        //    this.InsertNewAlarm(Alertas.ParagemMonitorizacao);
                                    }


                                    this.AdicionaLog("Falha de leitura: " + ex.Message);

                                    this.ReadStatus = false;
                                }
                                finally
                                {

                                    this.LastTimeStamp = DateTime.Now;

                                    if (this.ReadStatus) //adiciona ao histório dos pontos para fazer a analise de valores descontrolados
                                    {
                                        int numOfNCPoints = 0;

                                        lock (this.lockPoints)
                                        {
                                            this.points.Insert(0, new DataPoint(this.Diametro, this.LastTimeStamp, this.Classe.GetClassificacao(this.Diametro)));

                                            //remove os pontos anteriores a x sec
                                            this.points.RemoveAll(x => x.DataHora < DateTime.Now.Subtract(this.Classe.SpTempoMaxPontosNC));

                                            numOfNCPoints = this.points.Count(x => x.Classe != Classificacoes.Classificacao.Conforme);
                                        }

                                        bool dadosDescontrolados = numOfNCPoints > this.Classe.MaxPontosNC;

                                        if (dadosDescontrolados && !fpDadosDescontrolados)
                                        {
                                            this.InsertNewAlarm(Alertas.DadosDescontrolados);
                                            this.AdicionaLog("Dados de produção descontrolados. Existem " + numOfNCPoints.ToString() + " pontos fora da especificação em menos de " + Convert.ToInt32(this.Classe.SpTempoMaxPontosNC.TotalSeconds) + " segundos.");
                                        }

                                        fpDadosDescontrolados = dadosDescontrolados;
                                    }
                                    else
                                    {
                                        lock (this.lockPoints) this.points.Clear(); //**TODO** AVALIAR
                                        fpDadosDescontrolados = false;
                                    }

                                    this.TotalReadTime = Convert.ToInt32((this.LastTimeStamp - dtStart).TotalMilliseconds);
                                    Debug.WriteLine("ReadTime(): Elapsed time " + this.TotalReadTime + " ms");

                                    firstCycle = false;
                                }
                        }

                        if (this.ReadStatus || modoSimulacaoLeitura)
                            this.InsertDB();

                        this.TotalTime = Convert.ToInt32((DateTime.Now - lastReadTime).TotalMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("CicloTrabalho():" + ex.Message);


                    if (this.ReadStatus && !this.falhaConexao.EmFalha)
                    {
                        this.falhaConexao.FalhaLeitura();
                        this.AdicionaLog("Paragem de leitura: " + ex.Message);

                        //iniciar thread para só dar o alarme algum tempo depois
                        new Thread(this.TaskParalelaInsertAlarm).Start();

                        //    this.InsertNewAlarm(Alertas.ParagemMonitorizacao);
                    }
                    else
                        this.AdicionaLog(ex.Message);


                    this.ReadStatus = false;

                }
                finally
                {
                    Thread.Sleep(1); //Faz uma espera de 1ms entre interações
                }

            if (this.ReadStatus)
                this.InsertNewAlarm(Alertas.ParagemMonitorizacao);


        }

        private string GetLatestFile(NetworkCredential credentials, string ftpDir)
        {
            DateTime startDt = DateTime.Now;

            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpDir));
                //    reqFTP.UsePassive = false;
                reqFTP.UseBinary = true;
                if (this.useAuthentication) reqFTP.Credentials = credentials;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();

                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        // use the stream to read file from remote location

                        List<DateTime> listOfDts = new List<DateTime>();

                        string[] lines = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines)
                            if (line.Length == 12)
                            {
                                string filename = line.Substring(0, line.Length - 4);

                                if (Diversos.IsNumeric(filename))
                                    listOfDts.Add(DateTime.ParseExact(filename,
                                                 "yyyyMMdd",
                                                  CultureInfo.InvariantCulture));
                            }


                        return ftpDir + listOfDts.Max().ToString("yyyyMMdd") + ".csv";
                    }
                }

            }
            catch (Exception ex)
            {
                string error = "GetLatestFile(): " + ex.Message;

                Debug.WriteLine(error);
                this.AdicionaLog(error);

                return string.Empty;
            }
            finally
            {
                Debug.WriteLine("GetLatestFile(): Elapsed time " + Convert.ToInt32((DateTime.Now - startDt).TotalMilliseconds) + " ms");
            }
        }

        public new void Dispose()
        {
            this.isAlive = false;
        }

        private bool InsertDB()
        {
            DateTime dtStart = DateTime.Now;
            try
            {
                if (string.IsNullOrEmpty(this.dataTable))
                    throw new Exception("tablename cannot be empty!");

                using (SqlConnection sqlConn = new SqlConnection(this.strConexao))
                using (SqlCommand sqlCmd = new SqlCommand("INSERT INTO " + this.dataTable + " (Diametro) VALUES (@Diametro)", sqlConn))
                {
                    sqlCmd.Parameters.Add("@Diametro", System.Data.SqlDbType.Real).Value = this.Diametro;

                    sqlConn.Open();

                    if (sqlCmd.ExecuteNonQuery() != 1)
                        throw new Exception("nº de resultados diferente de 1!");
                    else
                        this.DbStatus = true;
                }
            }
            catch (Exception ex)
            {
                this.AdicionaLog("InsertDB(): " + ex.Message);

                if (this.DbStatus)
                    this.AdicionaLog("Falha de escrita na base de dados: " + ex.Message);

                this.DbStatus = false;
            }

            if (this.DbStatus)
            {
                this.LastDbInsertTime = DateTime.Now;

                this.DbInsertTime = Convert.ToInt32((this.LastDbInsertTime - dtStart).TotalMilliseconds);

                Debug.WriteLine("InsertDB(): Elapsed time " + this.DbInsertTime + " ms");
            }

            return this.DbStatus;
        }

        private void dgvPoints_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                dgvPoints.ClearSelection();
        }

        public bool InsertNewAlarm(Alertas alerta)
        {
            try
            {
                bool sucesso = false;

                using (SqlConnection sqlConn = new SqlConnection(this.strConexao))
                using (SqlCommand sqlCmd = new SqlCommand("INSERT INTO Alertas (IdMaquina, IdAlerta, Diametro) VALUES (@IdMaquina, @IdAlerta, @Diametro)", sqlConn))
                {
                    sqlCmd.Parameters.Add("@IdMaquina", System.Data.SqlDbType.TinyInt).Value = this.Id;
                    sqlCmd.Parameters.Add("@IdAlerta", System.Data.SqlDbType.TinyInt).Value = (int)alerta;
                    sqlCmd.Parameters.Add("@Diametro", System.Data.SqlDbType.Real).Value = this.Diametro;

                    sqlConn.Open();

                    sucesso = sqlCmd.ExecuteNonQuery() == 1;
                }

                if (sucesso) //ve se necessita de enviar email
                {
                    InfoTipoAlerta infAlerta = Forms.MainForm.VariaveisAuxiliares.ListagemAlertas.FirstOrDefault(x => x.Alertas == alerta);

                    if (infAlerta.Alertas > 0 && infAlerta.EmitirAlerta && this.mail != null)
                    {
                        //envia o email

                        string machineName = txtNomeMaquina.Text;

                        StringBuilder strMensagem = new StringBuilder("Início da mensagem automática: ");
                        strMensagem.AppendLine(Environment.NewLine);

                        switch (alerta)
                        {
                            case Alertas.DadosDescontrolados:
                                strMensagem.AppendLine("Os dados de produção encontram-se DESCONTROLADOS. O número de pontos no tempo definido ultrapassou o especificado!");
                                break;
                            case Alertas.InicioMonitorizacao:
                                strMensagem.AppendLine("Deu início ao registo e monitorização dos valores da linha.");
                                break;
                            case Alertas.ParagemMonitorizacao:
                                strMensagem.AppendLine("Ocorreu uma paragem do registo e monitorização dos valores da linha. Poderá dever-se a diversas causas tais como: máquina desligada; aplicação de registo desligada; falha de comunicação com a máquina;");
                                break;
                            case Alertas.NumeroRegistosLidosUltrapassado:
                                strMensagem.AppendLine("O número de registos lidos repetidos foi ultrapassado. Poderá dever-se a uma falha de registo de novos valores pela máquina.");
                                break;
                        }

                        strMensagem.AppendLine(Environment.NewLine);
                        strMensagem.AppendLine(Environment.NewLine);

                        strMensagem.AppendLine("Registo efetuado em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " pelo posto " + Environment.MachineName.ToUpper());

                        strMensagem.AppendLine(Environment.NewLine);
                        strMensagem.AppendLine(Environment.NewLine);
                        strMensagem.AppendLine("**********************************************************************");
                        strMensagem.AppendLine("***** Esta é uma mensagem automática e não deverá ser respondida *****");
                        strMensagem.AppendLine("**********************************************************************");
                        strMensagem.AppendLine(Environment.NewLine);

                        Task.Factory.StartNew(() =>
                        {
                            this.mail.SendEmail(machineName, strMensagem.ToString(), Forms.MainForm.VariaveisAuxiliares.ListagemEmails);
                        });
                    }
                }

                return sucesso;

            }
            catch (Exception ex)
            {
                this.AdicionaLog("InsertNewAlarm(): " + ex.Message);
                return false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset últimos pontos?", "Reset", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                lock (this.lockPoints) this.points.Clear();
                dgvHistLastPoints.Rows.Clear();
            }
        }

        private void dgvHistLastPoints_SelectionChanged(object sender, EventArgs e)
        {
            dgvHistLastPoints.ClearSelection();
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
                this.Min = min;
                this.Max = max;
            }

            public bool InRange(double value, bool incMin, bool incMax)
            {
                if (incMin && incMax)
                    return Diversos.InRange(value, this.Min, this.Max);
                else if (!incMin && incMax)
                    return value > this.Min && value <= this.Max;
                else if (incMin && !incMax)
                    return value >= this.Min && value < this.Max;
                else
                    return value > this.Min && value < this.Max;

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

    class FalhaConexao
    {
        public DateTime startDt { get; private set; } = DateTime.MinValue;
        public bool EmFalha { get; private set; } = false;
        public bool AlertaEmitido { get; set; } = false;

        public int SpMsAlarme { get; private set; } = 5000;

        public bool TempoExpirado
        {
            get
            {
                return this.EmFalha && DateTime.Now > startDt.AddMilliseconds(this.SpMsAlarme);
            }
        }

        public bool NecessarioEnviarAlarme
        {
            get
            {
                return !this.AlertaEmitido && this.TempoExpirado;
            }
        }


        public FalhaConexao(int spAlarme)
        {
            this.SpMsAlarme = spAlarme;
        }

        public void FalhaLeitura()
        {
            if (!this.EmFalha)
            {
                this.startDt = DateTime.Now;
                this.EmFalha = true;
            }

        }


        public void LimpaDados()
        {
            this.startDt = DateTime.MinValue;
            this.EmFalha = this.AlertaEmitido = false;
        }

    }


    public enum Alertas
    {
        Undefined = 0,
        InicioMonitorizacao = 1,
        ParagemMonitorizacao = 2,
        DadosDescontrolados = 3,
        NumeroRegistosLidosUltrapassado = 4
    }
}
