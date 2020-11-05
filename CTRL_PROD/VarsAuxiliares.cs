using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTRL_PROD
{
    public class VarsAuxiliares
    {
        /// <summary>
        /// Ficheiro de log da aplicação
        /// </summary>
        public LogFile LogDebug;


        /// <summary>
        /// Caminho para o ficheiro INI
        /// </summary>
        public string iniPath = Application.StartupPath + @"\Configs.ini";


        /// <summary>
        /// Tempo, em formato unix, do arranque da aplicação
        /// </summary>
        public int horaDeArranque = 0;

        public bool FLAG_WHILE_CYCLE { get; private set; } = true;
        public bool FORM_LOADED = false;

        public string DB_CONNECTION_STRING { get; private set; } = string.Empty;

        public int DB_UPDATE_RATE = 100; //refrescamento db


        public Point LAST_LOCATION = new Point(0, 0);


        public string IpDb { get; private set; } = string.Empty;

        public int SETTINGS_UPDATE_RATE = 60000; //refrescamento settings


        public List<Destinatarios> ListagemEmails = new List<Destinatarios>();
        public List<InfoTipoAlerta> ListagemAlertas = new List<InfoTipoAlerta>();

        public VarsAuxiliares()
        {
            this.horaDeArranque = Diversos.ObterTempoUnixAtual();

            //Variáveis do LOG da aplicação
            string logDir = Application.StartupPath + @"\Logs\Log.txt";
            int logMaxSize = 5120;
            bool logEnabled = true;

            #region Load configurations from INI File
            try
            {
                using (FicheiroINI ini = new FicheiroINI(iniPath))
                {

                    // Load Database configurations
                    this.DB_CONNECTION_STRING = @"data source=" + ini.RetornaINI("Database", "Address", ".") + @"\" + ini.RetornaINI("Database", "Instance", "SQLEXPRESS") + "; Initial Catalog=" + ini.RetornaINI("Database", "DBName", "CTRL_PROD") + "; MultipleActiveResultSets=True; persist security info=False; UID=" + ini.RetornaINI("Database", "User", "streak") + "; PWD=" + ini.RetornaINI("Database", "Pass", "streak") + ";";

                    this.IpDb = ini.RetornaINI("Database", "Address", ".");

                    #region Log
                    //Log da aplicação
                    logDir = ini.RetornaINI("AppLog", "logDir", logDir);
                    logMaxSize = Convert.ToInt32(ini.RetornaINI("AppLog", "logMaxSize", Convert.ToString(logMaxSize)));
                    logEnabled = ini.RetornaTrueFalseDeStringFicheiroINI("AppLog", "logEnabled", logEnabled);
                    #endregion

                    // Diversos
                    this.LAST_LOCATION = new Point(Convert.ToInt32(ini.RetornaINI("App", "LocX", "0")), Convert.ToInt32(ini.RetornaINI("App", "Locy", "0")));

                    this.SETTINGS_UPDATE_RATE = Convert.ToInt32(ini.RetornaINI("App", "SETTINGS_UPDATE_RATE", this.SETTINGS_UPDATE_RATE.ToString()));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Configs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            #endregion

            this.LogDebug = new LogFile(logDir, logMaxSize, logEnabled);

        }

        private void SaveSomeSettings()
        {
            using (FicheiroINI ini = new FicheiroINI(iniPath))
            {
                ini.EscreveFicheiroINI("App", "LocX", this.LAST_LOCATION.X.ToString());
                ini.EscreveFicheiroINI("App", "LocY", this.LAST_LOCATION.Y.ToString());
            }
        }

        public void Dispose()
        {
            this.FLAG_WHILE_CYCLE = false;
            this.SaveSomeSettings();
            this.FORM_LOADED = false;
        }

        public bool UpdateSomeInfos()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this.DB_CONNECTION_STRING))
                {
                    this.ListagemEmails.Clear();

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT Nome, Email FROM Emails WHERE Id > 0 AND Ativo = 1 ORDER BY Id", sqlConn))
                    {
                        sqlConn.Open();

                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                            while (dr.Read())
                                this.ListagemEmails.Add(new Destinatarios(Convert.ToString(dr[0]), Convert.ToString(dr[1])));

                        sqlConn.Close();
                    }

                    this.ListagemAlertas.Clear();

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT Id, EmitirAlerta FROM TipoAlerta WHERE Id > 0 ORDER BY Id", sqlConn))
                    {
                        sqlConn.Open();

                        using (SqlDataReader dr = sqlCmd.ExecuteReader())
                            while (dr.Read())
                                this.ListagemAlertas.Add(new InfoTipoAlerta((Alertas)Convert.ToInt32(dr[0]), Convert.ToBoolean(dr[1])));

                        sqlConn.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateSomeInfos(): " + ex.Message);
                this.ListagemEmails.Clear();
                this.ListagemAlertas.Clear();
                return false;
            }
        }

        public class EnviaEmail
        {
            public NetworkCredential Credentials = new NetworkCredential();
            public string Host { get; private set; } = "smtp.gmail.com";
            public int Port { get; private set; } = 587;
            public bool EnableSsl { get; private set; } = true;
            public int Timeout { get; private set; } = 30000;


            public EnviaEmail()
            {

            }

            public EnviaEmail(string iniFile)
            {
                this.LoadConfigs(iniFile);
            }

            public void LoadConfigs(string iniFile)
            {
                using (FicheiroINI ini = new FicheiroINI(iniFile))
                {
                    this.Credentials = new NetworkCredential(ini.RetornaINI("Email", "Username", ""), ini.RetornaINI("Email", "Password", ""));
                    this.Host = ini.RetornaINI("Email", "Host", "smtp.gmail.com");
                    this.Port = Convert.ToInt32(ini.RetornaINI("Email", "Port", "587"));
                    this.EnableSsl = ini.RetornaTrueFalseDeStringFicheiroINI("Email", "EnableSsl", true);
                    this.Timeout = Convert.ToInt32(ini.RetornaINI("Email", "Timeout", "30000"));
                }
            }

            public bool SendEmail(string maquina, string mensagem, List<Destinatarios> destinatarios)
            {
                try
                {
                    var smtp = new SmtpClient
                    {
                        Host = this.Host,
                        Port = this.Port,
                        EnableSsl = this.EnableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = this.Credentials
                    };
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(this.Credentials.UserName, "Controlo de Produção");

                        foreach (Destinatarios destinatario in destinatarios)
                            message.To.Add(new MailAddress(destinatario.Email, destinatario.Nome));

                        message.Subject = "Controlo de Produção '" + maquina + "'";
                        message.Body = mensagem;
                        smtp.Send(message);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SendEmail(): " + ex.Message);
                    return false;
                }
            }
        }

        public struct Destinatarios
        {
            public string Email;
            public string Nome;

            public Destinatarios(string nome, string email)
            {
                this.Nome = nome;
                this.Email = email;
            }
        }

        public struct InfoTipoAlerta
        {
            public Alertas Alertas;
            public bool EmitirAlerta;

            public InfoTipoAlerta(Alertas alertas, bool emitirAlerta)
            {
                this.Alertas = alertas;
                this.EmitirAlerta = emitirAlerta;
            }
        }

    }

}
