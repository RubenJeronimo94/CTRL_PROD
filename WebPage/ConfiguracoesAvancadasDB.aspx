<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracoesAvancadasDB.aspx.cs" Inherits="WebPage.ConfiguracoesAvancadasDB" MasterPageFile="MasterPage.Master" Title="Configurações Base de Dados" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () { //corre script no load
            //   alert(newDate(2).toString());
            GetGraph();
            LoadingVisibility(false);
        }, false);


        function GetGraph() {

            try {
                $.ajax({
                    type: "POST",
                    url: "/ConfiguracoesAvancadasDB.aspx/GetDbUsedSpace",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {


                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {

                            document.getElementById('lblCenterEspacoUsado').innerText = item.EspacoUsado + " mb";
                            document.getElementById('lblEspacoUsado').innerHTML = 'Espaço Usado <b>' + item.EspacoUsado + 'mb</b';
                            document.getElementById('lblEspacoLivre').innerHTML = 'Espaço Livre <b>' + item.EspacoLivre + 'mb</b';

                            var doughnutChartCanvas = $("#mychart").get(0).getContext("2d");
                            var doughnutPieData = {
                                datasets: [{
                                    data: [item.PercUsado, item.PercLivre],
                                    backgroundColor: [
                                        '#38ce3c',
                                        '#ff4d6b'
                                    ],
                                    borderColor: [
                                        '#38ce3c',
                                        '#ff4d6b'
                                    ],
                                }],

                                // These labels appear in the legend and in the tooltips when hovering different arcs
                                labels: [
                                    'Espaço Livre',
                                    'Espaço Usado'
                                ]
                            };
                            var doughnutPieOptions = {
                                cutoutPercentage: 75,
                                animationEasing: "easeOutBounce",
                                animateRotate: true,
                                animateScale: false,
                                responsive: true,
                                maintainAspectRatio: true,
                                showScale: true,
                                legend: {
                                    display: false
                                },
                                layout: {
                                    padding: {
                                        left: 0,
                                        right: 0,
                                        top: 0,
                                        bottom: 0
                                    }
                                }
                            };
                            var doughnutChart = new Chart(doughnutChartCanvas, {
                                type: 'doughnut',
                                data: doughnutPieData,
                                options: doughnutPieOptions
                            });



                        });
                    },
                    error:
                        function (response) {
                            throw (response);
                        },
                    failure:
                        function (response) {
                            throw (response);
                        }
                });
            } catch (e) {
                alert(e);
            }
        }

    </script>

    <div class="main-panel">
        <div class="content-wrapper">


            <div class="row">

                <div class="col-md-4 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Espaço usado em base de dados</h4>
                            <div class="aligner-wrapper">
                                <canvas id="mychart" height="210"></canvas>
                                <div class="wrapper d-flex flex-column justify-content-center absolute absolute-center">
                                    <h2 class="text-center mb-0 font-weight-bold" id="lblCenterEspacoUsado">350 mb</h2>
                                    <small class="d-block text-center text-muted  font-weight-semibold mb-0">Espaço Usado</small>
                                </div>
                            </div>
                            <div class="wrapper mt-4 d-flex flex-wrap align-items-cente">
                                <div class="d-flex">
                                    <span class="square-indicator bg-danger ml-2"></span>
                                    <p class="mb-0 ml-2" id="lblEspacoUsado">Espaço Usado <b>350mb</b></p>
                                </div>
                                <div class="d-flex">
                                    <span class="square-indicator bg-success ml-2"></span>
                                    <p class="mb-0 ml-2" id="lblEspacoLivre">Espaço Livre <b>9250mb</b></p>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-8 grid-margin stretch-card">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">Limpeza de Espaço em Base de Dados</h4>

                            <blockquote class="blockquote">
                                <p class="mb-0">Caso existam demasiados registos em base de dados, os mesmo deverão ser eliminados. Deverá selecionar a linha a limpar e confirmar.</p>
                            </blockquote>


                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <div class="form-group" style="max-width: 300px;">
                                            <label>Selecionar Linha</label>
                                            <select class="js-example-basic-single" style="width: 100%" id="selectionLinha" runat="server">
                                                <option value="1">28D EXT-30-03</option>
                                                <option value="2">Linha 2</option>

                                            </select>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group row">
                                        <div class="form-group" style="max-width: 300px;">
                                            <div style="display: none;">
                                                <label>Especificar Data</label>
                                                <input type="date" id="txtDt" style="width: 200px;" runat="server" />
                                            </div>
                                            <button type="button" class="btn btn-danger btn-icon-text btn-block" id="btnDelete" runat="server" onclick="if (!confirm('Deseja realmente eliminar os registos em base de dados para a linha selecionada? Esta operação poderá ser demorada!')) return false;" onserverclick="btnDelete_ServerClick">
                                                <i class="icon-trash btn-icon-prepend"></i>Eliminar Registos
                                            </button>
                                        </div>


                                    </div>
                                </div>
                            </div>
                            <div class="card">
                                <div class="card-body">
                                    <div class="media">
                                        <i class="icon-info icon-md text-danger d-flex align-self-center mr-3"></i>
                                        <div class="media-body">
                                            <p class="card-text">Depois de confirmar já não é possível recuperar registos eliminados.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
