using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace WebPage
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public AppSession SessaoOperador
        {
            get
            {
                return VARS.UserSession;
            }
        }

        public bool IsViewModeOnly { get; private set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                #region update lateral menu

                //update lateral menu
                foreach (Maquina maquina in VARS.Maquinas)
                {
                    divConsultaRegistos.InnerHtml += "<li class=\"nav-item\">";
                    divConsultaRegistos.InnerHtml += "<a class=\"nav-link\" href=\"/ConsultarRegistos.aspx?id=" + maquina.Id.ToString() + "\">";
                    divConsultaRegistos.InnerHtml += "<span class=\"menu-title\">" + maquina.Nome + "</span>";
                    divConsultaRegistos.InnerHtml += "<i class=\"icon-graph menu-icon\"></i>";
                    divConsultaRegistos.InnerHtml += "</a></li>";

                    if (VARS.UserSession.AdministratorPermission)
                    {
                        divConfiguraMaquinas.InnerHtml += "<li class=\"nav-item\">";
                        divConfiguraMaquinas.InnerHtml += "<a class=\"nav-link\" href=\"/Configuracoes.aspx?id=" + maquina.Id.ToString() + "\">";
                        divConfiguraMaquinas.InnerHtml += "<span class=\"menu-title\">" + maquina.Nome + "</span>";
                        divConfiguraMaquinas.InnerHtml += "<i class=\"icon-wrench menu-icon\"></i>";
                        divConfiguraMaquinas.InnerHtml += "</a></li>";
                    }
                }
                #endregion
            }


            #region Verifica se está na pagina de consulta de registos com autenticacao externa
            if (Page.TemplateControl.AppRelativeVirtualPath.IndexOf("ConsultarRegistos.aspx") != -1)
            {
                string key = Request.QueryString["key"];
                string id = Request.QueryString["id"];
                string dtInicio = Request.QueryString["DtInicio"];
                string dtFim = Request.QueryString["DtFim"];
                string txtOffset = Request.QueryString["Offset"];


                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(dtInicio) && !string.IsNullOrEmpty(dtFim) && !string.IsNullOrEmpty(txtOffset))
                {
                    DateTime[] dateTimes = new DateTime[2];

                    if (string.IsNullOrEmpty(txtOffset))
                        txtOffset = "0";


                    if (DateTime.TryParse(dtInicio, out dateTimes[0]) && DateTime.TryParse(dtFim, out dateTimes[1]))
                    {
                        string encriptacao = new GetKeys().Encripta(dtFim + id + txtOffset.Replace(".", ",") + dtInicio).Replace("+", "X").Replace("=", "Y");
                        Debug.WriteLine("Chave Encriptacao: " + encriptacao);
                        this.IsViewModeOnly = encriptacao == key;

                        if (this.IsViewModeOnly)
                            Debug.WriteLine("******** CHAVE VALIDADA *************");
                        else
                            Debug.WriteLine("======== CHAVE INVALIDA =============");

                    }
                }
            }
            #endregion

            if (this.IsViewModeOnly && !VARS.UserSession.SessaoIniciada)
            {
                sidebar.Style.Add("display", "none");
                topBar.Style.Add("display", "none");

                hiperlink1.HRef = hiperlink2.HRef = "#";
            }


            if (!VARS.UserSession.SessaoIniciada && !VARS.DebugMode && !this.IsViewModeOnly)
                Response.Redirect("/Login.aspx");
            else
            {
                if (!VARS.UserSession.AdministratorPermission)
                {
                    btnSepConfiguracoes.Visible = false;

                    btnConfigsAvancadasDB.HRef = "#";
                    btnConfigsAvancadasDB.Visible = false;
                    btnGestaoAcessos.HRef = "#";
                    btnGestaoAcessos.Visible = false;


                    btnGestaoAcessos.HRef = "#";
                    btnGestaoAcessos.Visible = false;
                    btnGestaoEmails.HRef = "#";
                    btnGestaoEmails.Visible = false;
                }
            }


        }

        protected void btnLogout_ServerClick(object sender, EventArgs e)
        {
            int idCopy = VARS.UserSession.ID;

            bool fezLogout = false;

            if (VARS.UserSession.SessaoIniciada)
                fezLogout = VARS.UserSession.FazLogout();

            //if (fezLogout)
            //{
            //    VARS.AdicionaLog(TipoLog.Logout, idCopy, TipoRegistoLog.NaoEspecificado, 0);
            Response.Redirect("/index.aspx");
            //}
        }
    }
}