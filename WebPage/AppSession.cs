using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebPage
{
    public class AppSession
    {
        public string NomeOperador
        {
            get
            {
                if (!this.SessaoIniciada)
                    return this.NivelSessao;
                else
                {
                    var obj = HttpContext.Current.Request.Cookies["nome"].Value;

                    if (obj != null)
                        return Convert.ToString(obj);
                    else
                        return string.Empty;
                }
            }
        }


        public int ID
        {
            get
            {
                var obj = HttpContext.Current.Request.Cookies["id"];

                if (obj != null)
                    return Convert.ToInt32(obj.Value);
                else
                    return 0;
            }
        }

        public int Nivel
        {
            get
            {
                var obj = HttpContext.Current.Request.Cookies["nivel"];

                if (obj != null)
                    return Convert.ToInt32(obj.Value);
                else
                    return 0;
            }
        }

        public int Avatar
        {
            get
            {
                var obj = HttpContext.Current.Request.Cookies["avatar"];

                if (obj != null)
                    return Convert.ToInt32(obj.Value);
                else
                    return 0;
            }
        }

        public bool AdministratorPermission
        {
            get { return this.Nivel >= 2; }
        }

        public bool OperatorPermission
        {
            get { return this.Nivel >= 1; }
        }

        public DateTime DtInicioSessao
        {
            get
            {
                var obj = HttpContext.Current.Request.Cookies["unixTime"];

                if (obj != null)
                    return Diversos.ConvertUnixParaDatetime(Convert.ToInt32(obj.Value));
                else
                    return DateTime.MinValue;

            }
        }


        public string NivelSessao
        {
            get
            {
                if (this.SessaoIniciada)
                    switch (this.Nivel)
                    {
                        case 0: return "Sem sessão";
                        case 1: return "Operador";
                        case 2: return "Administrador";
                    }

                return string.Empty;
            }
        }

        public string GetAvatarLink
        {
            get
            {
                if (this.SessaoIniciada)
                    switch (this.Avatar)
                    {
                        case 1: return "/images/faces-clipart/pic-1.png";
                        case 2: return "/images/faces-clipart/pic-2.png";
                        case 3: return "/images/faces-clipart/pic-3.png";
                        case 4: return "/images/faces-clipart/pic-4.png";
                    }

                return string.Empty;
            }
        }

        public bool SessaoIniciada { get { return this.ID > 0; } }

        public AppSession()
        {

        }

        public bool FazLogin(int id, string nome, int nivel, int avatar)
        {
            try
            {
                if (this.SessaoIniciada)
                    throw new Exception("Já existe uma sessão iniciada!");

                HttpContext.Current.Response.Cookies["id"].Value = id.ToString();
                HttpContext.Current.Response.Cookies["nome"].Value = nome;
                HttpContext.Current.Response.Cookies["nivel"].Value = nivel.ToString();
                HttpContext.Current.Response.Cookies["avatar"].Value = avatar.ToString();
                HttpContext.Current.Response.Cookies["unixTime"].Value = Diversos.ObterTempoUnixAtual().ToString();

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("FazLogin(): " + ex.Message);
                this.LimpaDados();

                return false;
            }
        }

        public bool FazLogout()
        {
            this.LimpaDados();

            return true;
        }

        private void LimpaDados()
        {
            HttpContext.Current.Response.Cookies["id"].Value = "0";
            HttpContext.Current.Response.Cookies["nome"].Value = "";
            HttpContext.Current.Response.Cookies["nivel"].Value = "0";
            HttpContext.Current.Response.Cookies["unixTime"].Value = "0";
            HttpContext.Current.Response.Cookies["avatar"].Value = "0";
        }
    }

}