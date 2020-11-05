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


    public partial class ControloDiametro : System.Web.UI.UserControl
    {
        public int IdLinha { get; set; } = 0;

        public Maquina Maquina
        {
            get
            {
                return VARS.Maquinas.First(x => x.Id == this.IdLinha);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblNomeMaquina.InnerText = this.Maquina.Nome;
        }
    }
}