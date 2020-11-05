<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebPage.Index" MasterPageFile="MasterPage.Master" Title="Página Inicial" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <%@ Register TagPrefix="uc" TagName="ControloDiametro" Src="/ControloDiametro.ascx" %>

    <div class="row quick-action-toolbar" id="divSelectMaquinas" runat="server">
        <div class="col-12 grid-margin">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <h4 class="card-title">Dashboard</h4>
                            <p class="card-description">Selecionar a máquina para visualizar registos em tempo real</p>
                            <div class="form-group row" id="divRadioButtons" runat="server">

                                <div class="form-check">
                                    <label class="form-check-label">
                                        <input type="radio" class="form-check-input" name="choice" value="1" onclick="RadioClick();">
                                        Máquina 1
                                    </label>
                                </div>

                                <div class="form-check">
                                    &nbsp  &nbsp  &nbsp
                           
                                </div>
                                <div class="form-check">
                                    <label class="form-check-label">
                                        <input type="radio" class="form-check-input" name="choice" value="2" onclick="RadioClick();" checked>
                                        Máquina 2
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="pnlEspacoMaquinas" runat="server">
    </div>


    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () { //corre script no load
            LoadingVisibility(false);
        }, false);



        function RadioClick() {
            const rbs = document.querySelectorAll('input[name="choice"]');

            for (const rb of rbs) {
                if (rb.checked) {
                  
                    window.location.href = "/index.aspx?id=" + rb.value;
                    break;
                }
            }

        };
    </script>

</asp:Content>
