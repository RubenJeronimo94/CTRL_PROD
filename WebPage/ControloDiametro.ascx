<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControloDiametro.ascx.cs" Inherits="WebPage.ControloDiametro" %>

<div style="display:none;">
<label id="lblId" style="display: none;"><% Response.Write(this.IdLinha); %> </label>
<label id="lblConformeMin" style="display: block;"><% Response.Write(this.Maquina.Classe.Conforme.Min); %> </label>
<label id="lblConformeMax" style="display: block;"><% Response.Write(this.Maquina.Classe.Conforme.Max); %> </label>
<label id="lblClasse2Min" style="display: block;"><% Response.Write(this.Maquina.Classe.Classe2.Min); %> </label>
<label id="lblClasse2Max" style="display: block;"><% Response.Write(this.Maquina.Classe.Classe2.Max); %> </label>
<label id="lblClasse3Min" style="display: block;"><% Response.Write(this.Maquina.Classe.Classe3.Min); %> </label>
<label id="lblClasse3Max" style="display: block;"><% Response.Write(this.Maquina.Classe.Classe3.Max); %> </label>

<a id="lblDoTest" class="btn">TEST</a>
    </div>
<div class="row quick-action-toolbar">
    <div class="col-md-12 grid-margin">
        <div class="card">
            <div class="card-header d-block d-md-flex">
                <h5 class="mb-0" id="lblNomeMaquina" runat="server">NOME MAQUINA</h5>
            </div>

            <div class="card-body">
                <p class="card-description">É possivel ver o valor do diâmetro em tempo real, assim como a classificação instantânea do filamento. </p>

                <div class="row">
                    <div class="col-md-12 d-flex align-items-center">
                        <div class="d-flex flex-row align-items-center">
                            <i class="icon-speedometer icon-md text-success"></i>
                            <p class="mb-0 ml-1">Diâmetro: </p>
                            &nbsp;&nbsp;
                            <%--<Label   BackColor="Black" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Names="Arial" Font-Overline="False" Font-Size="Larger" ForeColor="LimeGreen" Height="26px" Width="120px" Style="text-align: center;"></Label>--%>
                            <b id="lblValue" style="background-color: black; text-align: center; height: 26px; width: 80px; color: white;">---</b>
                        </div>
                        &nbsp;&nbsp;
                            &nbsp;&nbsp;
                           <div class="d-flex flex-row align-items-center">
                               <i class="icon-calculator icon-md text-secondary"></i>
                               <p class="mb-0 ml-1">Classificação: </p>
                               &nbsp;&nbsp;
                          <b id="lblClassificacao" style="background-color: black; text-align: center; height: 26px; width: 175px; color: white;">---</b>
                           </div>
                        &nbsp;&nbsp;
                            &nbsp;&nbsp;
                           <div class="d-flex flex-row align-items-center">
                               <i class="icon-calendar icon-md text-secondary"></i>
                               <p class="mb-0 ml-1">Último Registo: </p>
                               &nbsp;&nbsp;
                        <b id="lblLastTimestamp" style="background-color: black; text-align: center; height: 26px; width: 175px; color: white;">---</b>

                           </div>
                    </div>
                </div>
                <div class="row">
                    <div class="card" style="width: 100%;">
                        <div class="card-body">
                            <h4 class="card-title" style="text-align: center;">Gráfico do Diâmetro</h4>
                            <div style="position: relative; height: 500px; width: 100%; background-color: white;">
                                <canvas id="myChart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-lg-8 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h6 style="text-align: center;">Alertas de Produção - Últimos 7 dias</h6>


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

                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-body">
                                <h6 style="text-align: center;">Médias Moveis</h6>

                                <table class="table table-striped" style="text-align: center;">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th><b>Valor</b></th>
                                            <th><b>Data/Hora</b></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="text-align: left;">Valor Médio: </td>
                                            <td id="lblValorMedio">0 </td>
                                            <td id="lblDtValorMedio">---</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">Valor Mínimo: </td>
                                            <td id="lblValorMin">0</td>
                                            <td id="lblDtValorMin">---</td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;">Valor Máximo: </td>
                                            <td id="lblValorMax">0</td>
                                            <td id="lblDtValorMax">---</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var timeFormat = 'HH:mm:ss';

    document.addEventListener('DOMContentLoaded', function () { //corre script no load

        $.noConflict();
        $('#tblAlertasProducao').DataTable({
            dom: 'lfrtpB',
            buttons: [
                { extend: 'excel', text: '<img src="images/excelIcon1.png"/>' },
                { extend: 'print', text: '<i class="icon-printer"></i>' }
            ],
            "order": [[0, "desc"]],
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

        GetGraph();
        UpdateAlertasProducao();

    }, false);

    $(".btn").on("click", function () {
        alert($(this).attr('id'));
    });

    setInterval(function () {
        try {
            $.ajax({
                type: "POST",
                url: "/index.aspx/GetUserControlInfos",
                data: '{id: "' + document.getElementById('lblId').innerHTML + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                    //Faz um for por cada campo
                    $.each($.parseJSON(response.d), function (i, item) {

                        let lblValue = document.getElementById('lblValue');
                        let lblClassificacao = document.getElementById('lblClassificacao');

                        document.getElementById('lblLastTimestamp').innerHTML = item.Timestamp;

                        if (!item.Error) {
                            lblValue.innerHTML = item.Diametro.toFixed(3);

                            switch (item.Classificacao) {
                                case 1:
                                    lblClassificacao.innerText = "Conforme";
                                    lblClassificacao.style.color = "LimeGreen";
                                    lblValue.style.color = "LimeGreen";
                                    break;
                                case 2:
                                    lblClassificacao.innerText = "Classe 2";
                                    lblClassificacao.style.color = "Yellow";
                                    lblValue.style.color = "Yellow";
                                    break;
                                case 3:
                                    lblClassificacao.innerText = "Classe 3";
                                    lblClassificacao.style.color = "OrangeRed";
                                    lblValue.style.color = "OrangeRed";
                                    break;
                                case 4:
                                    lblClassificacao.innerText = "Não Conforme";
                                    lblClassificacao.style.color = "Red";
                                    lblValue.style.color = "Red";
                                    break;
                                default:
                                    lblClassificacao.innerText = "Sem Especif.";
                                    lblClassificacao.style.color = "White";
                                    lblValue.style.color = "White";
                                    break;
                            }

                            //atualiza o grafico
                            UpdateChart(item.Diametro, item.Timestamp);


                        } else {
                            lblValue.innerHTML = "---";
                            lblValue.style.color = "White";
                            lblClassificacao.innerText = "Error";
                            lblClassificacao.style.color = "White";

                        }
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
    }, 500);


    function UpdateAlertasProducao() {
        try {
            $.ajax({
                type: "POST",
                url: "/index.aspx/GetAlertasProducao",
                data: '{id: "' + document.getElementById('lblId').innerHTML + '"}',
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
            alert('Update alertas producao: ' + e);
        }
    }

    setInterval(function () {
        try {
          
            //a cada 5 sec verifica a ocorrencia de novos alertas de producao
            $.ajax({
                type: "POST",
                url: "/index.aspx/CountAlertasProducao",
                data: '{id: "' + document.getElementById('lblId').innerHTML + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                    let numRegistos = parseInt(response.d);
                    let table = $('#tblAlertasProducao').DataTable();

                    // alert("nr registos: " + numRegistos + " count: " + table.rows().count());



                    if (numRegistos <= 0)
                        table.clear().draw();//limpar a tabela atual
                    else if (numRegistos != table.rows().count())
                        UpdateAlertasProducao(); //diferencas no nr de registos! vamos atualizar
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
    }, 5000);


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


    function formatDateTime(date) {
        return moment(date, 'dd/MM/yyyy HH:mm:ss').format(timeFormat);
    }

    let instNumOfRows = 0;

    let ctx = document.getElementById('myChart').getContext('2d');
    let myChart = new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Diâmetro',
                fill: false,
                borderColor: 'black',
                lineTension: 0.25,
                borderColor: 'rgba(0, 0, 0, 0.5)',
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
                    //   distribution: 'series',
                    time: {
                        parser: timeFormat,
                        round: 'second',
                        displayFormats: {
                            second: this.timeFormat
                        }
                    },
                    scaleLabel: {
                        display: false,
                        labelString: 'Periodo (sec)'
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
        try {

            let points = [];

            $.ajax({
                type: "POST",
                url: "/index.aspx/GetLastChartInfo",
                data: '{id: "' + document.getElementById('lblId').innerHTML + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                    instNumOfRows = 0;

                    //Faz um for por cada campo
                    $.each($.parseJSON(response.d), function (i, item) {
                        points.push({ x: formatDateTime(item.Timestamp), y: item.Diametro });
                        instNumOfRows++;
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

        } catch (e) {
            alert(e);
        }

    }


    let lastTS;


    function UpdateChart(Diametro, Timestamp) {

        //atualiza o grafico
        if (lastTS != Timestamp)
            try {
                if (myChart.data.datasets[0].data.length > instNumOfRows) {
                    myChart.data.datasets[0].data.shift();
                }

                myChart.data.datasets[0].data.push({ x: formatDateTime(Timestamp), y: Diametro });

                //medias min e maxs

                let total = 0;
                let minValue = 99999;
                let minDt;
                let maxValue = 0;
                let maxDt;

                for (var i = 0; i < myChart.data.datasets[0].data.length; i++) {
                    total += myChart.data.datasets[0].data[i].y;

                    //minimo
                    if (myChart.data.datasets[0].data[i].y < minValue) {
                        minValue = myChart.data.datasets[0].data[i].y;
                        minDt = myChart.data.datasets[0].data[i].x;
                    }

                    //maximo
                    if (myChart.data.datasets[0].data[i].y > maxValue) {
                        maxValue = myChart.data.datasets[0].data[i].y;
                        maxDt = myChart.data.datasets[0].data[i].x;
                    }

                }


                // document.getElementById('lblValorMedio').innerText = ((parseFloat(document.getElementById('lblValorMedio').innerText.replace(',', '.')) + Diametro) / averageRowCount).toFixed(3);
                document.getElementById('lblValorMedio').innerText = (total / myChart.data.datasets[0].data.length).toFixed(3);
                document.getElementById('lblDtValorMedio').innerText = formatDateTime(Timestamp);

                document.getElementById('lblValorMin').innerText = minValue.toFixed(3);
                document.getElementById('lblDtValorMin').innerText = minDt;

                document.getElementById('lblValorMax').innerText = maxValue.toFixed(3);
                document.getElementById('lblDtValorMax').innerText = maxDt;


                myChart.update();

            } catch (e) {

            }
            finally {
                lastTS = Timestamp;
            }
    }


</script>
