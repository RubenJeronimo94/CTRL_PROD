<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultarRegistos.aspx.cs" Inherits="WebPage.ConsultarRegistos" MasterPageFile="MasterPage.Master" Title="Consultar Registos2" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div style="display: none;">
        <label id="lblId" style="display: none;"><% Response.Write(ID); %> </label>
        <label id="lblConformeMin" style="display: none;"><% Response.Write(this.Maquina.Classe.Conforme.Min); %> </label>
        <label id="lblConformeMax" style="display: none;"><% Response.Write(this.Maquina.Classe.Conforme.Max); %> </label>
        <label id="lblClasse2Min" style="display: none;"><% Response.Write(this.Maquina.Classe.Classe2.Min); %> </label>
        <label id="lblClasse2Max" style="display: none;"><% Response.Write(this.Maquina.Classe.Classe2.Max); %> </label>
        <label id="lblClasse3Min" style="display: none;"><% Response.Write(this.Maquina.Classe.Classe3.Min); %> </label>
        <label id="lblClasse3Max" style="display: none;"><% Response.Write(this.Maquina.Classe.Classe3.Max); %> </label>

        <label id="lblDtInicio" style="display: block;"><% Response.Write(this.dateTimes[0].ToString("dd/MM/yyyy HH:mm:ss")); %> </label>
        <label id="lblDtFim" style="display: block;"><% Response.Write(this.dateTimes[1].ToString("dd/MM/yyyy HH:mm:ss")); %> </label>


        <input type="datetime-local" class="form-control" id="txtDtInicial" runat="server" style="display: block;">
        <input type="datetime-local" class="form-control" id="txtDtFinal" runat="server" style="display: block;">
    </div>

    <div class="row quick-action-toolbar">
        <div class="col-md-12 grid-margin">
            <div class="card">
                <div class="card-header d-block d-md-flex">
                    <h5 class="mb-0" id="lblNomeMaquina" runat="server">NOME DA MAQUINA</h5>
                </div>
                <div class="card-body">
                    <div class="row income-expense-summary-chart-text">
                        <div class="col-xl-3">
                            <h5>Gráfico do Diâmetro</h5>
                            <p class="small text-muted">Obter gráfico dos valores registados. Especificar o intervalo de tempo!</p>
                        </div>
                        <div class="col-xl-9">
                            <div class="form-group row">

                                <div class="col-xl-8" style="border: 0px solid red;">
                                    <div id="reportrange" class="btn btn-light btn-lg btn-block" style="cursor: pointer;">
                                        <span></span><i class="icon-calendar float-right"></i>
                                    </div>
                                </div>


                                <div class="col-xl-4">
                                    <div class="form-group row">
                                        <div class="col-xl-6">
                                            <div style="float: right;">
                                                <input type="text" class="form-control" id="txtOffset" runat="server" placeholder="Aplicar offset" style="max-width: 135px; text-align: center;" onchange="setDecimal(this);">
                                            </div>
                                        </div>
                                        <div class="col-xl-6">
                                            <div style="float: right;">
                                                <a href="javascript: UpdateChart();" type="button" class="btn btn-outline-primary btn-icon-text" id="btnRefresh" runat="server"><i class="icon-reload btn-icon-prepend"></i>Atualizar</a>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="position: relative; height: 500px; width: 100%; background-color: white;">
                        <canvas id="myChart"></canvas>
                    </div>
                </div>

                <div class="row">

                    <div class="col-lg-6 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body" style="border: 0px solid red;">
                                <h6 style="text-align: center;">Classificação Atribuída</h6>
                                <div style="width: 100%; height: 100%; border: 0px solid blue;">
                                    <b id="lblClassificacao" style="background-color: black; text-align: center; width: 100%; height: 72px; color: white; font-size: 48px; display: block;">---</b>

                                    <div id="idDisplayQrCode" runat="server" style="display: none;">
                                        <%--<img style="display: block; margin-left: auto; margin-right: auto;" runat="server" id="idQrCode" />--%>
                                        <img style="display: block; margin-left: auto; margin-right: auto;" runat="server" id="idQrCode" />
                                        <ul class="list-ticked" style="text-align: center;">
                                            <li>Usar o QR Code para partilhar os dados da bobine!</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h6 style="text-align: center;">Médias Moveis</h6>
                                <table class="table table-striped" style="text-align: center;">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th><b>Valor</b></th>
                                            <th><b>Classe</b></th>
                                            <th><b>Data/Hora</b></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="text-align: left;">Valor Médio: </td>
                                            <td id="lblValorMedio">0 </td>
                                            <td id="lblClasseValorMedio">---</td>
                                            <td id="lblDtValorMedio">---</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">Valor Mínimo: </td>
                                            <td id="lblValorMin">0</td>
                                            <td id="lblClasseValorMin">---</td>
                                            <td id="lblDtValorMin">---</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">Valor Máximo: </td>
                                            <td id="lblValorMax">0</td>
                                            <td id="lblClasseValorMax">---</td>
                                            <td id="lblDtValorMax">---</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-lg-6 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h6 style="text-align: center;">Alertas de Produção</h6>
                                <table id="tblAlertasProducao" class="display" style="width: 100%; font-size: 14px;">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Descrição do Alerta</th>
                                            <th>Diâmetro</th>
                                            <th>Classificação</th>
                                            <th>Data/Hora</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%--      <tr>
                                            <td>18875</td>
                                            <td>Dados de produção descontrolados</td>
                                            <td>1.895</td>
                                            <td>Não Conforme</td>
                                            <td>2020/08/20 23:55:02</td>
                                        </tr>
                                         <tr>
                                            <td>18876</td>
                                            <td>Número de registos repetidos lidos foi ultrapassado!</td>
                                            <td>1.895</td>
                                            <td>Não Conforme</td>
                                            <td>2020/08/20 23:55:02</td>
                                        </tr>--%>
                                    </tbody>
                                </table>
                                <blockquote class="blockquote" id="lblMaxRecords1" style="display: none;">
                                    <p class="mb-0">A mostrar os primeiros <b>1000</b> registos! Exportar para Excel para ver o relatório completo.</p>
                                </blockquote>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-6 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h6 style="text-align: center;">Pontos Fora de Especificação</h6>
                                <table id="tblClassificacoes" class="display" style="width: 100%; font-size: 14px;">
                                    <thead>
                                        <tr>
                                            <th>Registo</th>
                                            <th>Diâmetro</th>
                                            <th>Classificação</th>
                                            <th>Data/Hora</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                                <blockquote class="blockquote" id="lblMaxRecords2" style="display: none;">
                                    <p class="mb-0">A mostrar os primeiros <b>1000</b> registos! Exportar para Excel para ver o relatório completo.</p>
                                </blockquote>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-xl-3">
                        <div class="form-group row">
                            <div class="col-sm-1"></div>
                            <div class="col-sm-11">
                                <input type="text" class="form-control" id="txtGraphTitle" runat="server" placeholder="Inserir um título para o gráfico">
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-9">
                        <a href="javascript: return false;" type="button" class="btn btn-outline-info btn-icon-text" runat="server" id="btnExportCSV" onserverclick="btnExportExcel_ServerClick" style="width: 98%" onclick="LoadingVisibility(false);"><i class="icon-cloud-download btn-icon-prepend"></i>Descarregar Relatório em Excel </a>
                    </div>

                </div>

            </div>

        </div>
    </div>

    <script type="text/javascript">

        document.addEventListener('DOMContentLoaded', function () { //corre script no load

            try {

                let start = formatDateTime(document.getElementById('lblDtInicio').innerText);
                let end = formatDateTime(document.getElementById('lblDtFim').innerText);

                let isView = <%=IsViewModeOnly ? "true" : "false" %>;
                let sessaoIniciada = <%=SessaoIniciada ? "true" : "false" %>;


                if (!isView || sessaoIniciada) {

                    $('#reportrange').daterangepicker({
                        startDate: start,
                        endDate: end,
                        timePicker24Hour: true,
                        showWeekNumbers: true,
                        timePicker: true,
                        locale: {
                            "format": "DD/MM/YYYY",
                            "separator": " - ",
                            "customRangeLabel": "Personalizar",
                            "cancelLabel": "Cancelar",
                            "applyLabel": "Aplicar",
                            "fromLabel": "de",
                            "toLabel": "até",
                            "daysOfWeek": [
                                "Dom",
                                "Seg",
                                "Ter",
                                "Qua",
                                "Qui",
                                "Sex",
                                "Sáb"
                            ]
                        },
                        "monthNames": [
                            "janeiro",
                            "fevereiro",
                            "março",
                            "abril",
                            "maio",
                            "junho",
                            "julho",
                            "agosto",
                            "setembro",
                            "outubro",
                            "novembro",
                            "dezembro"
                        ],
                        ranges: {
                            'Hoje': [moment(), moment()],
                            'Ontem': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                            'Últimos 7 Dias': [moment().subtract(6, 'days'), moment()],
                            'Últimos 30 Dias': [moment().subtract(29, 'days'), moment()],
                            'Este Mês': [moment().startOf('month'), moment().endOf('month')],
                            'Último Mês': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                        }
                    }, cb);
                }

                cb(start, end);

                $.noConflict();
                $('#tblAlertasProducao').DataTable({
                    dom: 'lfrtpB',
                    buttons: [
                        { extend: 'excel', text: '<img src="images/excelIcon1.png"/>' },
                        { extend: 'print', text: '<i class="icon-printer"></i>' }
                    ],
                    "language": {
                        "lengthMenu": '<div class="form-inline">Mostrar:&nbsp;<select class="form-control form-control-sm" style="height:32px;">' +
                            '<option value="10">10</option>' +
                            '<option value="25">25</option>' +
                            '<option value="50">50</option>' +
                            '<option value="100">100</option>' +
                            '<option value="-1">Tudo</option>' +
                            '</select></div>',
                        "zeroRecords": "Não existem alertas de produção!",
                        "info": "Página _PAGE_ de _PAGES_",
                        "infoEmpty": "Sem registos disponíveis",
                        "infoFiltered": "(filtered from _MAX_ total records)",
                        "sSearch": "",
                        "searchPlaceholder": "Pesquisar...",
                        "paginate": {
                            "sPrevious": "Anterior",
                            "sNext": "Próximo",
                            "sFirst": "Primeiro",
                            "sLast": "Último"
                        }
                    }
                });
                $('#tblClassificacoes').DataTable({
                    dom: 'lfrtpB',
                    buttons: [
                        { extend: 'excel', text: '<img src="images/excelIcon1.png"/>' },
                        { extend: 'print', text: '<i class="icon-printer"></i>' }
                    ],
                    "language": {
                        "lengthMenu": '<div class="form-inline">Mostrar:&nbsp;<select class="form-control form-control-sm" style="height:32px;">' +
                            '<option value="10">10</option>' +
                            '<option value="25">25</option>' +
                            '<option value="50">50</option>' +
                            '<option value="100">100</option>' +
                            '<option value="-1">Tudo</option>' +
                            '</select></div>',
                        "zeroRecords": "Não existem pontos fora de especificação!",
                        "info": "Página _PAGE_ de _PAGES_",
                        "infoEmpty": "Sem registos disponíveis",
                        "infoFiltered": "(filtered from _MAX_ total records)",
                        "sSearch": "",
                        "searchPlaceholder": "Pesquisar...",
                        "paginate": {
                            "sPrevious": "Anterior",
                            "sNext": "Próximo",
                            "sFirst": "Primeiro",
                            "sLast": "Último"
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }



            if (<%= OrdUpdateChart ? "true" : "false" %>) {

                setTimeout(GetGraph, 500);
            }
            else
                LoadingVisibility(false);
        }, false);

        function formatDateTime(date) {
            return moment(date, 'DD/MM/YYYY HH:mm:ss');
        }

        function GetClass(value) {
            if (value >= parseFloat(document.getElementById('lblConformeMin').innerText.replace(',', '.')) && value <= parseFloat(document.getElementById('lblConformeMax').innerText.replace(',', '.')))
                return 1;
            else if (value >= parseFloat(document.getElementById('lblClasse2Min').innerText.replace(',', '.')) && value < parseFloat(document.getElementById('lblClasse2Max').innerText.replace(',', '.')))
                return 2
            else if (value > parseFloat(document.getElementById('lblClasse3Min').innerText.replace(',', '.')) && value < parseFloat(document.getElementById('lblClasse3Max').innerText.replace(',', '.')))
                return 3;
            else
                return 4;
        }

        function GetClassColor(value) {
            switch (GetClass(value)) {
                case 1: return 'limegreen'; break;
                case 2: return 'yellow'; break;
                case 3: return 'orange'; break;
                case 4: return 'red'; break;
                default: return 'white'; break;
            }
        }

        function GetClassText(index) {
            switch (index) {
                case 1: return 'Conforme'; break;
                case 2: return 'Classe 2'; break;
                case 3: return 'Classe 3'; break;
                case 4: return 'Não Conforme'; break;
                default: return '---'; break;
            }
        }

        var ctx = document.getElementById('myChart').getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: [{
                    label: 'Diâmetro',
                    fill: false,
                    borderColor: 'black',
                    lineTension: 0.25,
                    borderColor: 'rgba(0, 0, 0, 0.8)',
                    pointRadius: 3,
                    backgroundColor: "#33AEEF",
                    pointBackgroundColor: function (context) {
                        return GetClassColor(parseFloat(context.dataset.data[context.dataIndex].y));
                    }
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                legend: {
                    display: false,
                },
                scales: {
                    xAxes: [{
                        display: true,
                        autoSkip: false,
                        type: 'time',
                        distribution: 'series',
                        time: {
                            minUnit: 'second',
                            displayFormats: {
                                millisecond: 'mm:ss.SSS',
                                second: 'DD/MM/YY HH:mm:ss',
                                minute: 'DD/MM/YY HH:mm:ss',
                                hour: 'DD/MM/YY HH:mm',
                                day: 'DD/MM/YY HH:mm',
                                week: 'DD/MM HH',
                                month: 'DD/MM',
                                quarter: 'DD/MM/YY',
                                year: 'DD/MM/YY',
                            }
                        },
                        scaleLabel: {
                            display: false,
                        },
                        gridLines: { borderDash: [4, 4], }
                    }],
                    yAxes: [{
                        display: true,
                        scaleLabel: {
                            display: true,
                            labelString: 'Diâmetro (mm)'
                        },
                        ticks: {
                            stepSize: 0.01,
                            min: parseFloat(document.getElementById('lblConformeMin').innerText.replace(',', '.')) - 0.1,
                            max: parseFloat(document.getElementById('lblConformeMax').innerText.replace(',', '.')) + 0.1
                        },
                        gridLines: { borderDash: [4, 4], }
                    }]
                },
                annotation: {
                    annotations: [{
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: 0,
                        yMax: parseFloat(document.getElementById('lblClasse3Min').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(255, 0, 0, 0.35)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblClasse3Min').innerText.replace(',', '.')),
                        yMax: parseFloat(document.getElementById('lblClasse2Min').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(255, 69, 0, 0.25)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblClasse2Min').innerText.replace(',', '.')),
                        yMax: parseFloat(document.getElementById('lblConformeMin').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(255, 255, 0, 0.15)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblConformeMin').innerText.replace(',', '.')),
                        yMax: parseFloat(document.getElementById('lblConformeMax').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(50, 205, 50, 0.1)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblConformeMax').innerText.replace(',', '.')),
                        yMax: parseFloat(document.getElementById('lblClasse2Max').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(255, 255, 0, 0.15)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblClasse2Max').innerText.replace(',', '.')),
                        yMax: parseFloat(document.getElementById('lblClasse3Max').innerText.replace(',', '.')),
                        backgroundColor: 'rgba(255, 69, 0, 0.25)'
                    },
                    {
                        type: 'box',
                        drawTime: 'beforeDatasetsDraw',
                        yScaleID: 'y-axis-0',
                        yMin: parseFloat(document.getElementById('lblClasse3Max').innerText.replace(',', '.')),
                        yMax: 2,
                        backgroundColor: 'rgba(255, 0, 0, 0.35)'
                    }
                    ]
                }
            }
        });

        function GetGraph() {

            LoadingVisibility(true);

            //Atualização do gráfico
            try {

                let points = [];

                $.ajax({
                    type: "POST",
                    url: "/ConsultarRegistos.aspx/GetGraph",
                    data: '{id: "' + document.getElementById('lblId').innerHTML + '", DtInicio: "' + document.getElementById('<%=txtDtInicial.ClientID%>').value + '", DtFim: "' + document.getElementById('<%=txtDtFinal.ClientID%>').value + '", Offset: "' + document.getElementById('<%=txtOffset.ClientID%>').value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {
                            points.push({ x: formatDateTime(item.Timestamp), y: item.Diametro });
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

                myChart.data.datasets[0].data = points;
                myChart.update();

            } catch (e) {
                alert('Error updating chart: ' + e);
            }

            //atualiza medias
            try {
                $.ajax({
                    type: "POST",
                    url: "/ConsultarRegistos.aspx/GetLastAVG",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {
                            document.getElementById('lblValorMedio').innerText = item.Media_D.toFixed(3);
                            document.getElementById('lblClasseValorMedio').innerText = GetClassText(item.Media_C);

                            document.getElementById('lblValorMin').innerText = item.Min_D.toFixed(3);
                            document.getElementById('lblClasseValorMin').innerText = GetClassText(item.Min_C);
                            document.getElementById('lblDtValorMin').innerText = item.Min_T;

                            document.getElementById('lblValorMax').innerText = item.Max_D.toFixed(3);
                            document.getElementById('lblClasseValorMax').innerText = GetClassText(item.Max_C);
                            document.getElementById('lblDtValorMax').innerText = item.Max_T;

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
            }
            catch (e) {
                alert('Error updating stats: ' + e);
            }

         

            //atualiza tabelas
            try {

                $.ajax({
                    type: "POST",
                    url: "/ConsultarRegistos.aspx/GetAlertasProducao",
                    data: '{id: "' + document.getElementById('lblId').innerHTML + '", DtInicio: "' + document.getElementById('<%=txtDtInicial.ClientID%>').value + '", DtFim: "' + document.getElementById('<%=txtDtFinal.ClientID%>').value + '", Offset: "' + document.getElementById('<%=txtOffset.ClientID%>').value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        let table = $('#tblAlertasProducao').DataTable();

                        //limpar a tabela atual
                        table.clear().draw();

                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {

                            table.row.add([item.Id, item.Tipo, item.Diametro, GetClassText(GetClass(item.Diametro)), item.DataHora]);
                        });

                        if (table.rows().count() >= 1000)
                            document.getElementById("lblMaxRecords1").style.display = 'block';

                        table.draw();

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
                alert('Error updating tables: ' + e);
            }

            //listagem automatica
            try {

                $.ajax({
                    type: "POST",
                    url: "/ConsultarRegistos.aspx/GetLastClassificacoes",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        let table = $('#tblClassificacoes').DataTable();

                        //limpar a tabela atual
                        table.clear().draw();

                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {

                            table.row.add([item.Id, item.Diametro, GetClassText(GetClass(item.Diametro)), item.DataHora]);

                        });

                        if (table.rows().count() >= 1000)
                            document.getElementById("lblMaxRecords2").style.display = 'block';

                        table.draw();

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
                alert('Error updating tables: ' + e);
            }

            //classificacao
            try {
                let result = synchronusPageMethod(PageMethods.GetLastClasse);

                let label = document.getElementById("lblClassificacao");

                label.innerText = GetClassText(result);

                switch (result) {
                    case 1: label.style.color = 'limegreen'; break;
                    case 2: label.style.color = 'yellow'; break;
                    case 3: label.style.color = 'orange'; break;
                    case 4: label.style.color = 'red'; break;
                    default: label.style.color = 'white'; break;
                }


            } catch (e) {
                alert('Error getting last classification: ' + e);
            }

            LoadingVisibility(false);
        }

        function UpdateChart() {
            try {
                let dtInicial = new Date(document.getElementById('<%=txtDtInicial.ClientID%>').value);
                let dtFinal = new Date(document.getElementById('<%=txtDtFinal.ClientID%>').value);

                if (isNaN(dtInicial)) throw "A data inicial é inválida!";
                if (isNaN(dtFinal)) throw "A data final é inválida!";

                if (dtInicial >= dtFinal) throw "A data inicial não pode ser superior à inicial!";


                let diffDays = parseInt((dtFinal - dtInicial) / (1000 * 60 * 60 * 24), 10);

                if (diffDays >= 2) throw "o limite máximo de registos por ficheiro é de 2 dias!";


                if (monthDiff(dtInicial, dtFinal) > 1)
                    throw "Não deverão exceder registos de 2 meses!";

                let offset = document.getElementById('<%=txtOffset.ClientID%>').value;

                if (empty(offset))
                    offset = "0";

                let newURL = UpdateQueryString('DtInicio', document.getElementById('<%=txtDtInicial.ClientID%>').value);
                newURL = UpdateQueryString('DtFim', document.getElementById('<%=txtDtFinal.ClientID%>').value, newURL);
                newURL = UpdateQueryString('Offset', offset, newURL);
                newURL = UpdateQueryString('key', synchronusPageMethod(PageMethods.GetKey, document.getElementById('<%=txtDtInicial.ClientID%>').value, document.getElementById('<%=txtDtFinal.ClientID%>').value, document.getElementById('<%=txtOffset.ClientID%>').value), newURL);

                window.location.href = newURL;

            } catch (e) {
                alert('Error: ' + e);
            }
        }

        function monthDiff(dateFrom, dateTo) {
            return dateTo.getMonth() - dateFrom.getMonth() +
                (12 * (dateTo.getFullYear() - dateFrom.getFullYear()))
        }

        function cb(start, end) {
            $('#reportrange span').html(start.format('DD/MM/YYYY HH:mm') + ' - ' + end.format('DD/MM/YYYY HH:mm'));


            document.getElementById('<%=txtDtInicial.ClientID%>').value = start.format('YYYY-MM-DD') + 'T' + start.format('HH:mm');
            document.getElementById('<%=txtDtFinal.ClientID%>').value = end.format('YYYY-MM-DD') + 'T' + end.format('HH:mm');

        }




    </script>
</asp:Content>
