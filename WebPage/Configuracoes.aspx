<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuracoes.aspx.cs" Inherits="WebPage.Configuracoes" MasterPageFile="MasterPage.Master" Title="Configurações Avançadas" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () { //corre script no load

            LoadingVisibility(false);
        }, false);
    </script>


    <div class="row quick-action-toolbar">

        <div class="col-md-12 grid-margin">
            <div class="card">
                <div class="card-header d-block d-md-flex">
                    <h5 class="mb-0" id="lblNomeMaquina" runat="server">NOME DA MAQUINA</h5>
                </div>
                <div class="card-body">
                    <form class="form-sample">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-3 col-form-label">Nome da Linha:</label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" id="txtNomeMaquina" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Conforme Minimo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimConformeMin" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Conforme Máximo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimConformeMax" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Classe 2 Minimo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimClasse2Min" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Classe 2 Máximo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimClasse2Max" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Máximo Pontos Classe 2:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtMaxPointsclasse2" runat="server" onchange="setInt(this);" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Classe 3 Minimo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimClasse3Min" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Limite Classe 3 Máximo:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtLimClasse3Max" runat="server" onchange="setDecimal(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Máximo Pontos Classe 3:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtMaxPointsclasse3" runat="server" onchange="setInt(this);" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">Máximo Pontos Não Conforme:</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtMaxPointsNC" runat="server" onchange="setInt(this);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label class="col-sm-6 col-form-label">SP Tempo para pontos Não Conforme (sec):</label>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control" id="txtSptempoMaxPointsNC" runat="server" onchange="setInt(this);" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <button type="button" class="btn btn-primary btn-icon-text" id="btnSave" runat="server" onserverclick="btnSave_ServerClick" style="float: right;">
                            <i class="icon-paper-plane btn-icon-prepend"></i>Guardar Alterações
                        </button>

                    </form>
                </div>
            </div>
        </div>


    </div>


</asp:Content>
