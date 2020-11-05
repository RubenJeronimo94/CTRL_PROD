using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebPage
{
    #region Enums

    public enum TipoIP
    {
        IPExterno,
        IPInterno
    }

    public enum TipoMsg
    {
        Informacao,
        Aviso,
        Erro,
        Questao
    }

    public enum ArrayOrder
    {
        ascendente,
        descendente
    }

    #endregion

    public static class SQLHelper
    {
        public static string GetSafeStringValue(SqlDataReader dr, int index)
        {
            if (!dr.IsDBNull(index))
                return Convert.ToString(dr[index]);
            else
                return string.Empty;
        }

        public static double GetSafeDblValue(SqlDataReader dr, int index)
        {
            if (!dr.IsDBNull(index))
                return Convert.ToDouble(dr[index]);
            else
                return double.NaN;
        }

        public static ushort GetSafeUInt16Value(SqlDataReader dr, int index)
        {
            if (!dr.IsDBNull(index))
                return Convert.ToUInt16(dr[index]);
            else
                return ushort.MinValue;
        }

        public static int GetSafeIntValue(SqlDataReader dr, int index)
        {
            if (!dr.IsDBNull(index))
                return Convert.ToInt32(dr[index]);
            else
                return 0;
        }

        public static object CalcInsertValue(double valueIn)
        {
            if (!double.IsNaN(valueIn))
                return valueIn;
            else
                return DBNull.Value;
        }

        public static DataTable GetDataTable(SqlConnection sqlConn, string sql, List<SqlParameter> list)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    for (int i = 0; i < list.Count; i++)
                        sqlCmd.Parameters.Add(list[i]);


                    sqlConn.Open();

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        dt.Load(reader);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - GetDataTable(): " + ex.Message);

                return null;
            }
            finally
            {
                sqlConn.Close();
            }
            return dt;
        }

        public static int ExecuteNonQuery(string sql, string strConexao)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    sqlConn.Open();

                    return sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteNonQuery(): " + ex.Message);

                return -1;
            }
        }

        public static int ExecuteNonQuery(string sql, string strConexao, List<SqlParameter> list)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    for (int i = 0; i < list.Count; i++)
                        sqlCmd.Parameters.Add(list[i]);

                    sqlConn.Open();

                    return sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteNonQuery(): " + ex.Message);



                return -1;
            }
        }


        public static int[] ExecuteNonQuery(string[] sql, string strConexao)
        {
            List<int> numOfRows = new List<int>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                    foreach (string str in sql)
                        using (SqlCommand sqlCmd = new SqlCommand(str, sqlConn))
                        {
                            sqlConn.Open();

                            numOfRows.Add(sqlCmd.ExecuteNonQuery());

                            sqlConn.Close();
                        }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteNonQuery(): " + ex.Message);

            }

            return numOfRows.ToArray();
        }

        public static int[] ExecuteNonQuery(string[] sql, string strConexao, List<SqlParameter> list)
        {
            List<int> numOfRows = new List<int>();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                    foreach (string str in sql)
                        using (SqlCommand sqlCmd = new SqlCommand(str, sqlConn))
                        {
                            for (int i = 0; i < list.Count; i++)
                                sqlCmd.Parameters.Add(list[i]);

                            sqlConn.Open();

                            numOfRows.Add(sqlCmd.ExecuteNonQuery());

                            sqlConn.Close();
                        }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteNonQuery(): " + ex.Message);


            }

            return numOfRows.ToArray();
        }


        public static object ExecuteScalar(string sql, string strConexao)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    sqlConn.Open();

                    return sqlCmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteScalar(): " + ex.Message);


                return -1;
            }
        }
        public static object ExecuteScalar(string sql, string strConexao, List<SqlParameter> list)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConexao))
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    for (int i = 0; i < list.Count; i++)
                        sqlCmd.Parameters.Add(list[i]);

                    sqlConn.Open();

                    return sqlCmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQLHelper - ExecuteScalar(): " + ex.Message);

                return -1;
            }

        }
    }

    public class CycleMultiplier
    {
        private int numberOfTimes = 0;
        private int counter = 0;
        private bool isReached = false;

        public bool IsReached
        {
            get
            {
                return this.isReached;
            }
        }

        public CycleMultiplier(int _numberOfTimes)
        {
            this.numberOfTimes = _numberOfTimes;
        }

        public void UpdateCycleCount(bool enabled)
        {
            if (enabled)
            {
                this.counter++;

                if (this.counter >= this.numberOfTimes)
                {
                    this.isReached = true;
                    this.counter = 0;
                }
                else
                    this.isReached = false;
            }
            else
            {
                this.counter = 0;
                this.isReached = false;
            }
        }

    }

    public class GereEstadoComunicacao
    {
        private ulong contadorConexoes = 0;
        private bool ultimoEstadoLigacao = false;
        private int tempoWatchDog = 0;
        private DateTime ultimoTempoMedido = DateTime.MinValue;

        public bool EstadoLigacao
        {
            get
            {
                bool estado = this.TempoPassadoDesdeUltimoHeartBeat < this.tempoWatchDog;

                if (estado && !this.ultimoEstadoLigacao)
                    this.contadorConexoes++;

                this.ultimoEstadoLigacao = estado;

                return estado;
            }
        }

        /// <summary>
        /// Retorna o tempo passado desde o último heartbeat recebido
        /// </summary>
        public double TempoPassadoDesdeUltimoHeartBeat
        {
            get
            {
                return (DateTime.Now - this.ultimoTempoMedido).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Retorna o total de conexões efetuadas
        /// </summary>
        public ulong TotalConexoes
        {
            get
            {
                return this.contadorConexoes;
            }
        }

        /// <summary>
        /// Retorna o total de conexões perdidas
        /// </summary>
        public ulong TotalConexoesPerdidas
        {
            get
            {
                return (this.contadorConexoes > 0) ? this.contadorConexoes - 1 : 0;
            }
        }

        public GereEstadoComunicacao(int _tempoWatchDog)
        {
            this.tempoWatchDog = _tempoWatchDog;
        }

        public void HeartBeat()
        {
            this.ultimoTempoMedido = DateTime.Now;
        }

        private void LimpaValores()
        {
            this.contadorConexoes = 0;
            this.ultimoEstadoLigacao = false;
            this.ultimoTempoMedido = DateTime.MinValue;
        }
    }

    //Class universal com diversas funções
    public static class Diversos
    {

        #region Buscar do config

        //Retorna true /false apartir de uma string 0/1
        public static bool RetornaTrueFalseDeStringFicheiroINI(FicheiroINI iniFile, string seccao, string campo, bool valorDefeito)
        {
            if (iniFile != null)
            {
                if (iniFile.LeFicheiroINI(seccao, campo) == "")
                    iniFile.EscreveFicheiroINI(seccao, campo, Convert.ToString(Convert.ToInt32(valorDefeito)));

                switch (Convert.ToInt32(iniFile.LeFicheiroINI(seccao, campo)))
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        return true;
                }
            }
            else
                return false;
        }


        #endregion

        public static Image DownloadImagemFromWeb(string src)
        {
            try
            {
                using (WebClient wc = new WebClient())
                    return System.Drawing.Image.FromStream(new MemoryStream(wc.DownloadData(src)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DownloadImagemFromWeb(): " + ex.Message);
                return null;
           }
        }

        public static Bitmap DrawFilledRectangle(int x, int y, Brush brush)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(brush, ImageSize);
            }
            return bmp;
        }

        #region Funções Temporais
        public static DateTime GetInternetTime()
        {
            using (WebResponse response = WebRequest.Create("http://www.google.com").GetResponse())
                return DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
        }

        //Obtem a hora no seguinte formato 00:00:00
        public static string ObtemHora()
        {
            try
            {
                string hora = DateTime.Now.Hour.ToString();
                string minuto = DateTime.Now.Minute.ToString();
                string segundo = DateTime.Now.Second.ToString();

                if (hora.Length == 1)
                    hora = "0" + hora;

                if (minuto.Length == 1)
                    minuto = "0" + minuto;

                if (segundo.Length == 1)
                    segundo = "0" + segundo;

                return hora + ":" + minuto + ":" + segundo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }

        //Função para calcular tempo de s para 00:00:00
        public static string CalculaTempo(int TempoEmSegundos)
        {
            try
            {
                int tempoSegundos = TempoEmSegundos;
                string dias = "";
                int interacoes = 0;

                while (tempoSegundos > 86400)
                {
                    interacoes++;
                    tempoSegundos = tempoSegundos - 86400;
                }

                if (interacoes > 0)
                    dias = Convert.ToString(interacoes) + "d ";

                string horas = Convert.ToString(tempoSegundos / (60 * 60));
                string minutos = Convert.ToString(((tempoSegundos / 60) - (tempoSegundos / (60 * 60) * 60)));
                string segundos = Convert.ToString(tempoSegundos % 60);

                if (Convert.ToInt32(horas) < 10)
                    horas = "0" + horas;

                if (Convert.ToInt32(minutos) < 10)
                    minutos = "0" + minutos;

                if (Convert.ToInt32(segundos) < 10)
                    segundos = "0" + segundos;

                return dias + horas + ":" + minutos + ":" + segundos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CalculaTempo(): " + ex.Message);
                return "error date";
            }
        }

        //Função para contar dias úteis entre duas datas
        public static int ObtemDiasUteis(DateTime initialDate, DateTime finalDate)
        {
            try
            {
                int days = 0;
                int daysCount = 0;
                days = initialDate.Subtract(finalDate).Days;

                if (days < 0)

                    days = days * -1;
                for (int i = 1; i <= days; i++)
                {
                    initialDate = initialDate.AddDays(1);
                    //Conta apenas dias da semana.
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday && initialDate.DayOfWeek != DayOfWeek.Saturday)
                        daysCount++;
                }
                return daysCount;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        //Função que obtem o tempo unix atual
        public static int ObterTempoUnixAtual()
        {
            return Convert.ToInt32(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        //Converte uma data para tempo unix
        public static int ConvertDatetimeParaUnix(DateTime data)
        {
            return Convert.ToInt32(data.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }

        //Converte um tempo unix para uma datetime
        public static DateTime ConvertUnixParaDatetime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return new DateTime(1970, 1, 1).AddSeconds(unixTimeStamp);
        }

        //Obtem por extenso o dia da semana em PT-PT
        public static string ObtemDiaSemana(DateTime _datetime)
        {
            return _datetime.ToString("dddd", new CultureInfo("pt-PT"));
        }

        #endregion

        #region Manipulação de Strings/Números

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static string RemoveAcentos(string strIn)
        {
            string returnString = strIn;

            char[] toReplace = ("àèìòùÀÈÌÒÙ äëïöüÄËÏÖÜ âêîôûÂÊÎÔÛ áéíóúÁÉÍÓÚðÐýÝ ãñõÃÑÕšŠžŽçÇåÅøØ").ToCharArray();
            char[] replaceChars = ("aeiouAEIOU aeiouAEIOU aeiouAEIOU aeiouAEIOUdDyY anoANOsSzZcCaAoO").ToCharArray();
            for (int i = 0; i < toReplace.Length; i++)
                returnString = returnString.Replace(toReplace[i], replaceChars[i]);

            return returnString;
        }

        public static bool ProcessIsRunning(string process)
        {
            return Process.GetProcessesByName(process).Length > 0;
        }

        public static string GetClientIpAddress()
        {
            return GetIPAddress(new HttpRequestWrapper(HttpContext.Current.Request));
        }

        public static string ObtemPrimeiroUltimoNome(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                return string.Empty;

            string[] words = nomeCompleto.Split();

            if (words == null)
                return string.Empty;

            if (words.Length == 1)
                return words[0];
            else
                return words[0] + " " + words[words.Length - 1];
        }
        internal static string GetIPAddress(HttpRequestBase request)
        {
            // handle standardized 'Forwarded' header
            string forwarded = request.Headers["Forwarded"];
            if (!String.IsNullOrEmpty(forwarded))
            {
                foreach (string segment in forwarded.Split(',')[0].Split(';'))
                {
                    string[] pair = segment.Trim().Split('=');
                    if (pair.Length == 2 && pair[0].Equals("for", StringComparison.OrdinalIgnoreCase))
                    {
                        string ip = pair[1].Trim('"');

                        // IPv6 addresses are always enclosed in square brackets
                        int left = ip.IndexOf('['), right = ip.IndexOf(']');
                        if (left == 0 && right > 0)
                        {
                            return ip.Substring(1, right - 1);
                        }

                        // strip port of IPv4 addresses
                        int colon = ip.IndexOf(':');
                        if (colon != -1)
                        {
                            return ip.Substring(0, colon);
                        }

                        // this will return IPv4, "unknown", and obfuscated addresses
                        return ip;
                    }
                }
            }

            // handle non-standardized 'X-Forwarded-For' header
            string xForwardedFor = request.Headers["X-Forwarded-For"];
            if (!String.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0];
            }

            return request.UserHostAddress;
        }

        public static void EnableNetworkAdapter(string interfaceName)
        {
            System.Diagnostics.ProcessStartInfo psi =
                   new System.Diagnostics.ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
        }

        public static void DisableNetworkAdapter(string interfaceName)
        {
            System.Diagnostics.ProcessStartInfo psi =
                new System.Diagnostics.ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
        }

        public static bool InRange(int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        public static bool InRange(double value, double minimum, double maximum)
        {
            return value >= minimum && value <= maximum;
        }

        public static string NumberToString(double number, int numberOfDecimals, bool commaAsDecimalSeparator)
        {
            try
            {
                string returnString = string.Empty;
                string decimals = string.Empty;

                for (int i = 0; i < numberOfDecimals; i++)
                    decimals += "0";

                number = Math.Round(number, numberOfDecimals);

                if (number == 0)
                    returnString = "0" + (commaAsDecimalSeparator ? "," : ".") + decimals;
                else
                {
                    returnString = number.ToString("#." + decimals); //Faz o string.format ao double

                    if (!commaAsDecimalSeparator)
                        returnString = returnString.Replace(",", ".");

                    if (number > 0 && number < 1) //Caso o número seja maior do que 0 e menor do que 1 vamos adicionar o 0 automaticamente antes do separador decimal, pois o mesmo é ocultado na função do STRING.FORMAT()
                        returnString = "0" + returnString;
                }

                return returnString;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("NumberToString(): " + ex.Message);
                return "####";
            }
        }


        /// <summary>
        /// Calcula a percentagem de 1 determinado número
        /// </summary>
        /// <param name="_valMaximo">Valor Máximo (Correspondente a 100%)</param>
        /// <param name="_valAtual">Valor Atual a calcular</param>
        /// <returns></returns>
        public static double CalculaPercentagem(double _valMaximo, double _valAtual)
        {
            return ((_valAtual * 100) / _valMaximo);
        }

        /// <summary>
        /// Calcula o valor absoluto apartir da percentagem
        /// </summary>
        /// <param name="_valorMaximo"></param>
        /// <param name="_valorEmPercentagem"></param>
        /// <returns></returns>
        public static double CalculaValorApartirDaPercentagem(double _valorMaximo, double _valorEmPercentagem)
        {
            return ((_valorMaximo * _valorEmPercentagem) / 100);
        }

        /// <summary>
        /// Função que descobre se os numeros são multiplos
        /// </summary>
        /// <param name="valA"></param>
        /// <param name="valB"></param>
        /// <returns></returns>
        public static bool DescobreMultiplo(int valA, int valB)
        {
            return (valA % valB) == 0;
        }



        //Função para verificar se valor é numerico
        public static bool IsNumeric(object value)
        {
            try
            {
                if (value == null || value is DateTime)
                    return false;

                if (value is Int16 || value is Int32 || value is Int64 || value is Decimal || value is Single || value is Double || value is System.Boolean)
                    return true;

                if (value is string)
                    Double.Parse(value as string);
                else
                    Double.Parse(value.ToString());
                return true;
            }
            catch { return false; }
        }

        //Função que percorre todo uma string e retira todos os acentos/caracteres especiais
        public static string TirarAcentos(string texto)
        {
            try
            {
                string textor = "";

                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto[i].ToString() == "ã")
                        textor += "a";
                    else if (texto[i].ToString() == "á")
                        textor += "a";
                    else if (texto[i].ToString() == "à")
                        textor += "a";
                    else if (texto[i].ToString() == "â")
                        textor += "a";
                    else if (texto[i].ToString() == "ä")
                        textor += "a";
                    else if (texto[i].ToString() == "é")
                        textor += "e";
                    else if (texto[i].ToString() == "è")
                        textor += "e";
                    else if (texto[i].ToString() == "ê")
                        textor += "e";
                    else if (texto[i].ToString() == "ë")
                        textor += "e";
                    else if (texto[i].ToString() == "í")
                        textor += "i";
                    else if (texto[i].ToString() == "ì")
                        textor += "i";
                    else if (texto[i].ToString() == "ï")
                        textor += "i";
                    else if (texto[i].ToString() == "õ")
                        textor += "o";
                    else if (texto[i].ToString() == "ó")
                        textor += "o";
                    else if (texto[i].ToString() == "ò")
                        textor += "o";
                    else if (texto[i].ToString() == "ö")
                        textor += "o";
                    else if (texto[i].ToString() == "ú")
                        textor += "u";
                    else if (texto[i].ToString() == "ù")
                        textor += "u";
                    else if (texto[i].ToString() == "ü")
                        textor += "u";
                    else if (texto[i].ToString() == "ç")
                        textor += "c";
                    else if (texto[i].ToString() == "Ã")
                        textor += "A";
                    else if (texto[i].ToString() == "Á")
                        textor += "A";
                    else if (texto[i].ToString() == "À")
                        textor += "A";
                    else if (texto[i].ToString() == "Â")
                        textor += "A";
                    else if (texto[i].ToString() == "Ä")
                        textor += "A";
                    else if (texto[i].ToString() == "É")
                        textor += "E";
                    else if (texto[i].ToString() == "È")
                        textor += "E";
                    else if (texto[i].ToString() == "Ê")
                        textor += "E";
                    else if (texto[i].ToString() == "Ë")
                        textor += "E";
                    else if (texto[i].ToString() == "Í")
                        textor += "I";
                    else if (texto[i].ToString() == "Ì")
                        textor += "I";
                    else if (texto[i].ToString() == "Ï")
                        textor += "I";
                    else if (texto[i].ToString() == "Õ")
                        textor += "O";
                    else if (texto[i].ToString() == "Ó")
                        textor += "O";
                    else if (texto[i].ToString() == "Ò")
                        textor += "O";
                    else if (texto[i].ToString() == "Ö")
                        textor += "O";
                    else if (texto[i].ToString() == "Ú")
                        textor += "U";
                    else if (texto[i].ToString() == "Ù")
                        textor += "U";
                    else if (texto[i].ToString() == "Ü")
                        textor += "U";
                    else if (texto[i].ToString() == "Ç")
                        textor += "C";
                    else
                        textor += texto[i];
                }
                return textor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro função retira acentos: " + ex.ToString());
                return null;
            }

        }

        //Obter número inteiro32 aleatóriamente
        public static int ObterIntAleatorio(int rangeMin, int rangeMax)
        {
            return new Random().Next(rangeMin, rangeMax);
        }

        //Ordenar array de inteiros
        public static int[] OrdenarArrayInt(int[] arrayInt, ArrayOrder arrayorder)
        {
            try
            {
                if (Convert.ToInt32(arrayorder) == 0)
                    Array.Sort(arrayInt);
                else if (Convert.ToInt32(arrayorder) == 1)
                    Array.Reverse(arrayInt);
                return arrayInt;
            }
            catch (Exception)
            {
                int[] arr = { };
                return arr;
            }

        }

        //Remover valores repetidos ArrayList
        public static ArrayList RemoverRepetidosArrayList(ArrayList items)
        {
            ArrayList noDups = new ArrayList();

            foreach (string strItem in items)
            {

                if (!noDups.Contains(strItem.Trim()))
                {

                    noDups.Add(strItem.Trim());
                }
            }

            noDups.Sort();

            return noDups;

        }

        //Converter decimal para binário
        public static string DecimalParaBinario(string data)
        {
            string result = string.Empty;
            int rem = 0;
            try
            {
                if (!IsNumeric(data))
                    return "Invalid Value - This is not a numeric value";
                else
                {
                    int num = int.Parse(data);
                    while (num > 0)
                    {
                        rem = num % 2;
                        num = num / 2;
                        result = rem.ToString() + result;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
            return result;
        }

        //Converter binário para decimal
        public static int BinarioParaDecimal(string valorBinario)
        {
            int expoente = 0;
            int numero;
            int soma = 0;

            string numeroInvertido = InverterString(valorBinario);

            try
            {
                for (int i = 0; i < numeroInvertido.Length; i++)
                {
                    //pega dígito por dígito do número digitado
                    numero = Convert.ToInt32(numeroInvertido.Substring(i, 1));

                    //multiplica o dígito por 2 elevado ao expoente, e armazena o resultado em soma
                    soma += numero * (int)Math.Pow(2, expoente);

                    // incrementa o expoente
                    expoente++;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return soma;

        }

        //Inverte uma string
        public static string InverterString(string str)
        {

            int tamanho = str.Length;

            char[] caracteres = new char[tamanho];

            for (int i = 0; i < tamanho; i++)
            {

                caracteres[i] = str[tamanho - 1 - i];

            }

            return new string(caracteres);

        }

        //Encontra número máximo num array
        public static double EncontraMaximoArray(double[] array)
        {
            return array.Max();
        }

        //Encontra número minimo num array
        public static double EncontraMinimoArray(double[] array)
        {
            return array.Min();
        }

        //Função que faz uma regra de 3 simples e retorna o valor pretendido
        public static double FazRegra3Simples(double valMaxSemUnidade, double valAtualsemUnidade, double valMaxEmMM)
        {
            try
            {
                double x = 0;
                x = (valAtualsemUnidade * valMaxEmMM);
                x = (x / valMaxSemUnidade);
                return x;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro função FazRegra3Simples: " + ex.ToString());
                return 0;
            }


        }

        //Função para fazer interpolação de pontos
        public static double InterpolacaoPontos(double x0, double y0, double x1, double y1, bool Ordem)
        {
            try
            {

                if (Ordem)
                {
                    //Se true salta em ordem a Y
                    Debug.WriteLine("(((" + y1 + "-" + y0 + ") / (" + x1 + "-" + x0 + ")) * (" + x1 + "-" + x0 + ") + " + y0 + ")");
                    return (((y1 - y0) / (x1 - x0)) * (x1 - x0) + y0);
                }

                else
                {
                    //Se falso salta em ordem a X
                    return (((x1 - x0) / (y1 - y0)) * (y1 - y0) + x0);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro função InterpolacaoPontos: " + ex.ToString());
                return 0;
            }
        }

        //Retorna o tamanho de A4 conforme a resolução pretendida
        public static Size A4SizeInPixels(int dpi)
        {
            int width = (int)Math.Round(210 / 25.4 * dpi);
            int height = (int)Math.Round(297 / 25.4 * dpi);
            return new Size(width, height);
        }


        /// <summary>
        /// Função que quebra uma string em várias quebras de linhas após x caracteres
        /// </summary>
        public static string DivideString(string _strIn, int _lineLength)
        {
            try
            {
                int counter = 0;
                StringBuilder strTratada = new StringBuilder();
                foreach (var c in _strIn)
                {
                    counter++;
                    if (char.IsWhiteSpace(c))
                        if (counter >= _lineLength)
                        {
                            counter = 0;
                            strTratada.Append(c + Environment.NewLine);
                        }
                        else
                            strTratada.Append(c);
                    else
                        strTratada.Append(c);
                }
                return strTratada.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return "Erro ao tratar string";
            }

        }
        #endregion

        #region Manipulação de Ficheiros

        public static Bitmap ByteToImage(byte[] blob)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                mStream.Write(blob, 0, blob.Length);
                mStream.Seek(0, SeekOrigin.Begin);

                return new Bitmap(mStream);
            }
        }


        /// <summary>
        /// Check if a directory is Windows valid
        /// </summary>
        /// <param name="path">Directory to check</param>
        /// <returns>Return TRUE if is valid</returns>
        public static bool IsValidPath(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (!driveCheck.IsMatch(path.Substring(0, 3)))
                return false;
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                return false;

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            if (!dir.Exists)
                dir.Create();
            return true;
        }

        //Copia uma pasta e respetivos sub folders de um diretorio para outro
        public static bool CopiaDiretorio(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDirName);
                var dirs = dir.GetDirectories();


                // If the destination directory does not exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                //Checks if last character is "\"
                if (destDirName.Substring(destDirName.Length - 1, 1) != @"\")
                {
                    destDirName += @"\";
                }

                //Gets folder name
                string nomePasta = sourceDirName.Substring(sourceDirName.LastIndexOf(@"\"), sourceDirName.Length - sourceDirName.LastIndexOf(@"\"));

                //Create the folder in the directory
                destDirName += nomePasta;

                //create the directory
                Directory.CreateDirectory(destDirName);

                // Get the file contents of the directory to copy.
                var files = dir.GetFiles();

                foreach (var file in files)
                {
                    // Create the path to the new copy of the file.
                    var temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    file.CopyTo(temppath, true);
                }

                // If copySubDirs is true, copy the subdirectories.
                if (!copySubDirs)
                    return true;

                foreach (var subdir in dirs)
                {
                    // Create the subdirectory.
                    var temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    CopiaDiretorio(subdir.FullName, temppath, copySubDirs);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //Abre Ficheiro especificado


        //Obtem a informação do ficheiro mais recente num directório
        public static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            try
            {
                return directory.GetFiles()
                    .Union(directory.GetDirectories().Select(d => GetNewestFile(d)))
                    .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao obter ficheiro mais recente:" + ex.Message);
                //     FuncoesLogFile.EscreveLog("Erro ao obter ficheiro mais recente. Mensagem original: " + ex.Message, true);
                return null;
            }
        }

        //Função que abre o Explorer do Windows num determinado diretório
        public static bool AbreExplorador(string filePath)
        {
            try
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        //Lê todo o texto de um arquivo
        public static string LerArquivoTexto(string nomeArquivo)
        {
            try
            {
                if (File.Exists(nomeArquivo))
                    return File.ReadAllText(nomeArquivo);
                else
                    return String.Empty;
            }
            catch (Exception)
            {
                return null;
            }

        }

        //Escreve texto num arquivo
        public static bool EscreveArquivoTexto(string diretorio, string texto)
        {
            try
            {
                if (!File.Exists(diretorio))
                {
                    File.Create(diretorio);
                    using (TextWriter tw = new StreamWriter(diretorio))
                    {
                        tw.WriteLine(texto);
                        tw.Close();
                    }
                    return true;
                }
                else if (File.Exists(diretorio))
                {
                    using (TextWriter tw = new StreamWriter(diretorio, true))
                    {
                        tw.WriteLine(texto);
                        tw.Close();
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao escrever em ficheiro: " + ex.ToString());
                return false;
            }
        }

        //Apaga um diretório e todo o seu conteudo
        public static bool ApagaDiretorio(string path)
        {
            try
            {
                string caminho = @path;
                DirectoryInfo diretorio = new DirectoryInfo(caminho);
                diretorio.Delete(true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao eliminar diretorio: " + ex.ToString());
                return false;
            }

        }

        //Redimensiona uma imagem para o size especificado
        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        #endregion

        #region Obter Dados PC/Internet

        public static string GetIpRemoto(string url)
        {
            try
            {
                //Cria uma requisição para a URL
                WebRequest rq = WebRequest.Create(url);

                //obtém o response a partir do request
                HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                //obtém um stream contendo a resposta retornada pelo servidor
                Stream ds = rp.GetResponseStream();
                //Cria um StreamReader para leitura
                StreamReader rd = new StreamReader(ds);
                //Lê os dados
                string responseFromServer = rd.ReadToEnd();
                //fecha os objetos
                rd.Close();
                ds.Close();
                rp.Close();
                //procura por indexafor fixo no resultado 
                string URL = "IP";
                int i = responseFromServer.IndexOf(URL) + URL.Length + 2;
                //captura o IP descoberto
                URL = string.Empty;
                while (!(responseFromServer[i].ToString() == "<"))
                {
                    URL += responseFromServer[i];
                    i += 1;
                }

                return URL.Trim();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao obter ip remoto: " + ex.ToString());
                return null;
            }

        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ObtemNomeUtilizadorLoggado()
        {
            try
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static string ObtemConteudoPaginaWeb(string url)
        {
            string result = "";
            try
            {
                System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                wreq.Method = "GET";
                wreq.Timeout = 3000;
                System.Net.HttpWebResponse wr = (System.Net.HttpWebResponse)wreq.GetResponse();

                if (wr.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.Stream s = wr.GetResponseStream();
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                    System.IO.StreamReader readStream = new System.IO.StreamReader(s, enc);
                    result = readStream.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Função obtem conteudo pagina web: " + ex.ToString());
                return null;
            }

        }

        public static string[] ListarTodosOsDrivesPC()
        {
            //Listagem de todos os drivers DVD/CD existentes no PC
            try
            {
                string[] drives = Directory.GetLogicalDrives();
                return drives;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static long VerMemoriaUsadaPrograma()
        {
            try
            {
                return GC.GetTotalMemory(true);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static string GetOSName()
        {
            System.OperatingSystem os = System.Environment.OSVersion;
            string osName = "Unknown";


            switch (os.Platform)
            {
                case System.PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0:
                            osName = "Windows 95";
                            break;
                        case 10:
                            osName = "Windows 98";
                            break;
                        case 90:
                            osName = "Windows ME";
                            break;
                    }
                    break;
                case System.PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3:
                            osName = "Windws NT 3.51";
                            break;
                        case 4:
                            osName = "Windows NT 4";
                            break;
                        case 5:
                            if (os.Version.Minor == 0)
                                osName = "Windows 2000";
                            else if (os.Version.Minor == 1)
                                osName = "Windows XP";
                            else if (os.Version.Minor == 2)
                                osName = "Windows Server 2003";
                            break;
                        case 6:
                            osName = "Windows Vista";
                            break;
                    }
                    break;
            }

            return osName + ", " + os.Version.ToString();
        }

        #endregion


        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="isLog">Is logaritmic?</param>
        /// <param name="rsquared">The r^2 value of the line.</param>
        /// <param name="yintercept">The y-intercept value of the line (i.e. y = ax + b, yintercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        public static void LinearRegression(double[] xVals, double[] yVals, bool isLog, int startIndex, int numOfRecords, out double rsquared, out double yintercept, out double slope)
        {
            double sumOfX = 0;
            double sumOfY = 0;

            double sumOfXSq = 0;
            double sumOfYSq = 0;

            double ssX = 0;
            double ssY = 0;

            double sumCodeviates = 0;
            double sCo = 0;

            for (int ctr = startIndex; ctr < startIndex + numOfRecords; ctr++)
            {

                double x = !isLog ? xVals[ctr] : Math.Log(xVals[ctr]);
                double y = yVals[ctr];

                sumCodeviates += x * y;

                sumOfX += x;
                sumOfY += y;

                sumOfXSq += x * x;
                sumOfYSq += y * y;

            }

            ssX = sumOfXSq - ((sumOfX * sumOfX) / numOfRecords);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / numOfRecords);

            double RNumerator = (numOfRecords * sumCodeviates) - (sumOfX * sumOfY);

            double RDenom = (numOfRecords * sumOfXSq - (sumOfX * sumOfX)) * (numOfRecords * sumOfYSq - (sumOfY * sumOfY));

            sCo = sumCodeviates - ((sumOfX * sumOfY) / numOfRecords);


            double meanX = sumOfX / numOfRecords;
            double meanY = sumOfY / numOfRecords;

            double dblR = RNumerator / Math.Sqrt(RDenom);

            rsquared = dblR * dblR;

            yintercept = meanY - ((sCo / ssX) * meanX);

            slope = sCo / ssX;
        }

        public static void ShowMsg(Exception ex)
        {
            //do nothing
            Debug.WriteLine("Diversos.ShowMsg(): " + ex.Message);
        }

        public static void AdicionaLog(Exception ex)
        {
            Debug.WriteLine("[LOG_ERROR]:" + ex.Message);
        }


        public static string GetFirstNCharsOfString(string str, int length)
        {
            return str.Length <= length ? str : str.Substring(0, length);
        }

        public static SqlParameter GetNumericParameter(string param, byte intPart, byte decPart, object value)
        {
            SqlParameter parameter = new SqlParameter(param, SqlDbType.Decimal);
            parameter.Precision = intPart;
            parameter.Scale = decPart;
            parameter.Value = value;
            return parameter;
        }

        public static bool ExecuteQuery(SqlCommand sqlCmd, bool updateConnectionState)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(VARS.ConnectionString))
                {
                    sqlCmd.Connection = sqlConn;

                    sqlConn.Open();

                    //if (updateConnectionState)
                    //    VARIAVEIS.DB_CONNECTION_STATE = true;

                    return sqlCmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExecuteQuery(): " + ex.Message);
                //if (updateConnectionState)
                //    VARIAVEIS.DB_CONNECTION_STATE = false;
                return false;
            }
        }

        public static int ExecutaQuery(SqlConnection sqlConn, string sqlQuery, SqlParameter[] parametros)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                try
                {
                    if (sqlConn == null)
                        throw new Exception("SqlConn == NULL");


                    foreach (SqlParameter param in parametros)
                        sqlCmd.Parameters.Add(param);

                    sqlConn.Open();

                    return sqlCmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExecutaQuery(): " + ex.Message);
                    return 0;
                }
                finally
                {
                    sqlConn.Close();

                    sqlCmd.Parameters.Clear();
                }
        }

        public static int ExecutaQuery(SqlConnection sqlConn, string sqlQuery, SqlParameter parametro)
        {
            return ExecutaQuery(sqlConn, sqlQuery, new SqlParameter[] { parametro });
        }

        public static object ExecuteScalar(SqlConnection sqlConn, string sqlQuery, SqlParameter parametro)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                try
                {
                    if (sqlConn == null)
                        throw new Exception("SqlConn == NULL");

                    sqlCmd.Parameters.Add(parametro);

                    sqlConn.Open();

                    return sqlCmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExecutaQuery(): " + ex.Message);
                    return null;
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Parameters.Clear();
                }
        }

        public static object ExecuteScalar(SqlConnection sqlConn, string sqlQuery, SqlParameter[] parametros)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                try
                {
                    if (sqlConn == null)
                        throw new Exception("SqlConn == NULL");

                    foreach (SqlParameter param in parametros)
                        sqlCmd.Parameters.Add(param);

                    sqlConn.Open();

                    return sqlCmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExecutaQuery(): " + ex.Message);
                    return null;
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Parameters.Clear();
                }
        }


        public static object ExecuteScalar(SqlConnection sqlConn, string sqlQuery)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                try
                {
                    if (sqlConn == null)
                        throw new Exception("SqlConn == NULL");


                    sqlConn.Open();

                    return sqlCmd.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ExecutaQuery(): " + ex.Message);
                    return null;
                }
                finally
                {
                    sqlConn.Close();
                    sqlCmd.Parameters.Clear();
                }
        }


        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                throw new NullReferenceException("The string cannot be null or empty");

            hexString = hexString.Replace(":", "");
            hexString = hexString.Replace("-", "");

            if (hexString.Length % 2 != 0)
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));

            byte[] HexAsBytes = new byte[hexString.Length / 2];

            for (int index = 0; index < HexAsBytes.Length; index++)
                HexAsBytes[index] = byte.Parse(hexString.Substring(index * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);


            return HexAsBytes;
        }

        public static string ConvertBytesToString(byte[] DataIn, int SByte, int NBytes)
        {
            string sRetVal = "";
            for (int i = SByte; i < SByte + NBytes; i++)
            {
                if (DataIn[SByte] != 00)
                {
                    if (DataIn[i] != 00)
                    {
                        sRetVal += DataIn[i].ToString("X").PadLeft(2, '0');
                    }
                }
                else
                {
                    sRetVal = "";
                }
            }

            return ConvertHex(sRetVal).ToString();
        }

        /// <summary>
        /// Função do programa antigo FornosDelta
        /// </summary>
        /// <param name="DataIn"></param>
        /// <param name="SByte"></param>
        /// <returns></returns>
        public static short ConvertBytesToShort(byte[] DataIn, int SByte)
        {
            try
            {
                return Convert.ToInt16(DataIn[SByte].ToString("X").PadLeft(2, '0') + DataIn[SByte + 1].ToString("X").PadLeft(2, '0'), 16);
            }
            catch
            {
                return 0;
            }
        }

        public static short MyConvertBytesToInt16(byte[] DataIn, int SByte)
        {
            return BitConverter.ToInt16(new byte[] { DataIn[SByte + 1], DataIn[SByte] }, 0);
        }
        public static int MyConvertBytesToInt32(byte[] DataIn, int SByte)
        {
            return BitConverter.ToInt32(new byte[] { DataIn[SByte + 1], DataIn[SByte], DataIn[SByte + 3], DataIn[SByte + 2] }, 0);
        }

        public static short ConvertBytesToInt16_old(byte[] DataIn, int SByte)
        {
            try
            {
                string sRetVal = "";
                sRetVal = DataIn[SByte].ToString("X").PadLeft(2, '0') + DataIn[SByte + 1].ToString("X").PadLeft(2, '0');
                return Convert.ToInt16(sRetVal, 16);
            }
            catch
            {
                return 0;
            }
        }

        public static int ConvertBytesToInt32(byte[] DataIn, int SByte)
        {
            try
            {
                string sRetVal = "";
                sRetVal = DataIn[SByte + 2].ToString("X").PadLeft(2, '0') + DataIn[SByte + 3].ToString("X").PadLeft(2, '0') + DataIn[SByte].ToString("X").PadLeft(2, '0') + DataIn[SByte + 1].ToString("X").PadLeft(2, '0');
                return Convert.ToInt32(sRetVal);
            }
            catch
            {
                return 0;
            }
        }

        public static float GetRealValue(short value)
        {
            try
            {
                return Convert.ToSingle(Convert.ToInt32(value.ToString("X"), 10) / 10.0f);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static object GetVarCharDbField(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return DBNull.Value;
            else
                return str;
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static string ConvertHex(string hexString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            String ascii = sb.ToString();
            return ascii;
        }



        /// <summary>
        /// Executa um ping ao IP especificado
        /// </summary>
        /// <param name="_enderecoIP">Retorna TRUE se ping for efetuada, FALSE se não</param>
        /// <returns></returns>
        public static bool FazPing(string _enderecoIP, bool mostraInfoDebug = true)
        {
            try
            {
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = pingSender.Send(IPAddress.Parse(_enderecoIP));

                    if (reply.Status == IPStatus.Success)
                    {
                        if (mostraInfoDebug)
                        {
                            Debug.WriteLine("Função Ping-> IP Address: " + reply.Address.ToString());
                            Debug.WriteLine("Função Ping-> Trip Time : " + reply.RoundtripTime + "ms");
                            Debug.WriteLine("Função Ping-> Time to live: " + reply.Options.Ttl);
                            Debug.WriteLine("Função Ping-> Don't fragment: " + reply.Options.DontFragment);
                            Debug.WriteLine("Função Ping-> Buffer Size: " + reply.Buffer.Length);
                        }
                        return true;
                    }
                    else
                    {
                        if (mostraInfoDebug)
                            Debug.WriteLine("Função Ping-> IP Address: " + reply.Address.ToString() + " sem resposta");
                        throw new Exception(Convert.ToString(reply.Status));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }



        /// <summary>
        /// Comparar 2 datatables 
        /// </summary>
        /// <param name="DT1">DataTable 1</param>
        /// <param name="DT2">DataTable 2</param>
        /// <returns></returns>
        public static bool CompareDataTables(DataTable DT1, DataTable DT2)
        {
            try
            {
                if ((DT1 == null) && (DT2 == null))
                    return true;
                else if ((DT1 != null) && (DT2 != null))
                {
                    if (DT1.Rows.Count == DT2.Rows.Count)
                    {
                        if (DT1.Columns.Count == DT2.Columns.Count)
                        {
                            for (int i = 0; i < DT1.Rows.Count; i++)
                            {
                                for (int j = 0; j < DT1.Columns.Count; j++)
                                {
                                    if (DT1.Rows[i][j].ToString() != DT2.Rows[i][j].ToString())
                                        return false;
                                }
                            }
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

        }


        public static void openOnScreenKeyboard()
        {
            try
            {
                Process.Start("osk.exe");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error opening OSKeyboard: " + ex.Message);
            }

        }

        public static void killOnScreenKeyboard()
        {
            if (Process.GetProcessesByName("osk").Count() > 0)
                Process.GetProcessesByName("osk").First().Kill();

        }

        /// <summary>
        /// Generates a percentage, formatted with "places" decimal places.
        /// </summary>
        /// <param name="value">Value for which a percentage is needed</param>
        /// <param name="total">Total from which to generate a percentage</param>
        /// <param name="places">Decimal places to return in the percentage string</param>
        /// <returns>string with the percentage value</returns>
        public static string GetPercentage(int value, int total, int places)
        {
            decimal percent = 0;
            string retval = string.Empty;
            string strplaces = new string('0', places);

            if (value == 0 || total == 0)
                percent = 0;
            else
            {
                percent = decimal.Divide(value, total) * 100;

                if (places > 0)
                    strplaces = "." + strplaces;
            }

            retval = percent.ToString("#" + strplaces);

            return retval;
        }

        public static bool SqlDataReaderToCsv(SqlDataReader dataReader, string fileName, bool includeHeaderAsFirstRow, bool sepFirst)
        {
            try
            {
                const string Separator = ",";
                StringBuilder sb = null;

                using (StreamWriter streamWriter = new StreamWriter(fileName, true, Encoding.Default))
                {
                    if (sepFirst)
                        streamWriter.WriteLine("sep=" + Separator);

                    if (includeHeaderAsFirstRow)
                    {
                        sb = new StringBuilder();
                        for (int index = 0; index < dataReader.FieldCount; index++)
                        {
                            if (dataReader.GetName(index) != null)
                                sb.Append(dataReader.GetName(index));

                            if (index < dataReader.FieldCount - 1)
                                sb.Append(Separator);
                        }
                        streamWriter.WriteLine(sb.ToString());
                    }

                    while (dataReader.Read())
                    {
                        sb = new StringBuilder();
                        for (int index = 0; index < dataReader.FieldCount - 1; index++)
                        {
                            if (!dataReader.IsDBNull(index))
                            {
                                string value = dataReader.GetValue(index).ToString();
                                if (dataReader.GetFieldType(index) == typeof(String))
                                {
                                    if (value.IndexOf("\"") >= 0)
                                        value = value.Replace("\"", "\"\"");

                                    if (value.IndexOf(Separator) >= 0)
                                        value = "\"" + value + "\"";
                                }
                                sb.Append(value);
                            }

                            if (index < dataReader.FieldCount - 1)
                                sb.Append(Separator);
                        }

                        if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
                            sb.Append(dataReader.GetValue(dataReader.FieldCount - 1).ToString().Replace(Separator, " "));

                        streamWriter.WriteLine(sb.ToString());
                    }
                    dataReader.Close();
                    streamWriter.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ToCsv(): " + ex.Message);
                return false;
            }
        }



        /// <summary>
        /// Retorna o nome do membro que foi chamado (variavel)
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static string GetCurrentMemberName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }


    /// <summary>
    /// Objecto para gerar uma cor aleatória
    /// </summary>
    public class RandomColor
    {
        Type colorType = typeof(Color);
        PropertyInfo[] proInfos;


        public RandomColor()
        {
            proInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.Public);
        }

        /// <summary>
        /// Gera uma cor aleatória
        /// </summary>
        /// <returns></returns>
        public Color NextColor()
        {
            return Color.FromName(proInfos[new Random().Next(0, proInfos.Length)].Name);
        }
    }


    /// <summary>
    /// Class do Ficheiro INI
    /// </summary>
    public class FicheiroINI : IDisposable
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public FicheiroINI(string INIPath)
        {
            //Alteramos todas as \ que possam estar ao contrário
            INIPath.Replace(@"/", @"\");
            //Associação do caminho
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void EscreveFicheiroINI(string Secao, string Campo, string Valor)
        {
            WritePrivateProfileString(Secao, Campo, Valor, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string LeFicheiroINI(string Secao, string Campo)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Secao, Campo, "", temp, 255, this.path);
            return temp.ToString();
        }

        /// <summary>
        ///  //Retorna true /false apartir de uma string 0/1
        /// </summary>
        /// <param name="seccao"></param>
        /// <param name="campo"></param>
        /// <param name="valorDefeito"></param>
        /// <returns></returns>
        public bool RetornaTrueFalseDeStringFicheiroINI(string seccao, string campo, bool valorDefeito)
        {
            try
            {
                if (LeFicheiroINI(seccao, campo) == "")
                    EscreveFicheiroINI(seccao, campo, Convert.ToString(Convert.ToInt32(valorDefeito)));

                switch (Convert.ToInt32(LeFicheiroINI(seccao, campo)))
                {
                    case 0:
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RetornaTrueFalseDeStringFicheiroINI(): " + ex.Message);
                EscreveFicheiroINI(seccao, campo, Convert.ToString(Convert.ToInt32(valorDefeito)));
                return valorDefeito;
            }

        }

        /// <summary>
        /// Retorna texto de um ficheiro INI e caso o campo nao exista no ficheiro cria-o
        /// </summary>
        /// <param name="_seccao"></param>
        /// <param name="_campo"></param>
        /// <param name="_strAPrencherCasoNulo"></param>
        /// <returns></returns>
        public string RetornaINI(string _seccao, string _campo, string _strAPrencherCasoNulo = "")
        {
            if (LeFicheiroINI(_seccao, _campo) != "")
                return Convert.ToString(LeFicheiroINI(_seccao, _campo));
            else
                if (_strAPrencherCasoNulo != "")
            {
                EscreveFicheiroINI(_seccao, _campo, _strAPrencherCasoNulo);
                return _strAPrencherCasoNulo;
            }
            else
            {
                EscreveFicheiroINI(_seccao, _campo, "");
                return "";
            }
        }

        public void ApagaSeccao(string _seccao)
        {
            WritePrivateProfileString(_seccao, null, null, this.path);
        }

        #region Disposing Methods

        private bool disposed = false; // to detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        GC.Collect();
                    }

                    disposed = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    //Class só com funções de encriptação (MD5)
    public class GetKeys
    {
        private const string Password = "NXLczn2M1b"; //Salt usado na encriptação - Não mexer

        public GetKeys()
        {
        }

        private byte[] Encrypt(byte[] clearText, byte[] Key, byte[] IV)
        {
            try
            {
                byte[] encryptedData;
                using (MemoryStream ms = new MemoryStream())
                using (Rijndael alg = Rijndael.Create())
                {
                    alg.Key = Key;
                    alg.IV = IV;
                    using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearText, 0, clearText.Length);
                        cs.Close();
                    }
                    encryptedData = ms.ToArray();
                }
                return encryptedData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao encriptar: " + ex.Message);
                return null;
            }
        }

        public string Encripta(string stringParaEncriptar)
        {
            try
            {
                byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(stringParaEncriptar);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao encriptar string: " + ex.Message);
                return null;
            }
        }

        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                byte[] decryptedData;
                using (MemoryStream ms = new MemoryStream())
                using (Rijndael alg = Rijndael.Create())
                {
                    alg.Key = Key;
                    alg.IV = IV;
                    using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherData, 0, cipherData.Length);
                        cs.Close();
                    }
                    decryptedData = ms.ToArray();
                }
                return decryptedData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao recuperar a string erncriptada: " + ex.Message);
                return null;
            }
        }

        public string Desencripta(string stringParaDesencriptar)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(stringParaDesencriptar);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro ao desencriptar string: " + ex.Message);
                return null;
            }
        }

    }


}
