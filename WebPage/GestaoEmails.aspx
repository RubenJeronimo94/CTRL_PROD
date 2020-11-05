<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestaoEmails.aspx.cs" Inherits="WebPage.GestaoEmails" MasterPageFile="/MasterPage.Master" Title="Gestão de Emails" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <div class="col-12 grid-margin stretch-card">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">Gestão de Emails</h4>
                <table id="tblEmails" class="display" style="width: 100%; font-size: 14px;">
                    <colgroup>
                        <col style="width: 50px;" />
                        <col style="width: auto;" />
                        <col style="width: auto;" />
                        <col style="width: 30px;" />
                        <col style="width: 160px;" />
                        <col style="width: 30px;" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nome</th>
                            <th>Email</th>
                            <th>Ativo</th>
                            <th>Última Alteração</th>
                            <th>Ação</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%--  <tr>
                            <td>1</td>
                            <td>
                                <input type="text" value="Rúben Jerónimo" style="width: 100%;" id="txtNome0" onchange="CallUpdate(0);"></td>
                            <td>
                                <input type="text" value="ruben.jeronimo@hotmail.com" style="width: 100%;" id="txtMail0"></td>
                            <td>
                                <center>
                                <button  type="button" onclick="javascript:InvertColor('btnAtivo0');" class="btn btn-success btn-rounded btn-icon" id="btnAtivo0">
                                    <i class="icon-check"></i>
                                </button>
                                    </center>
                            </td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>
                                <input type="text" value="Rúben Jerónimo" style="width: 100%;" id="txtNome1"></td>
                            <td>
                                <input type="text" value="ruben.jeronimo@hotmail.com" style="width: 100%;" id="txtMail1"></td>
                            <td>
                                <center>
                             <button type="button" onclick="javascript:InvertColor('btnAtivo1');" class="btn btn-inverse-dark btn-rounded btn-icon" id="btnAtivo1">
                                    <i class="icon-check"></i>
                                </button>
                                    </center>
                            </td>
                        </tr>--%>
                    </tbody>
                </table>

                <button type="button" class="btn btn-primary btn-lg btn-block" onclick="AddNew();">
                    <i class="icon-plus"></i>&nbsp Adicionar novo registo
                </button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () { //corre script no load

            try {
                $.noConflict();
                $('#tblEmails').DataTable({
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
                        "zeroRecords": "Não existem emails registados!",
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

                UpdateTable();

            } catch (e) {
                alert(e);
            }
            finally {
                LoadingVisibility(false);
            }

        }, false);

        function InvertColor(btn) {

            let button = document.getElementById(btn);

            if (button.classList.contains('btn-inverse-dark')) {
                button.classList.remove('btn-inverse-dark');
                button.classList.add('btn-success');
            }
            else {
                button.classList.remove('btn-success');
                button.classList.add('btn-inverse-dark');
            }
        }

        function CallUpdate(id) {
            try {
                LoadingVisibility(true);

                let nome = document.getElementById("txtNome" + id).value;
                let mail = document.getElementById("txtEmail" + id).value;
                let ativo = document.getElementById("btnAtivo" + id).classList.contains('btn-success') ? "1" : "0";

                let sql = "UPDATE Emails SET Nome = '" + nome + "', Email = '" + mail + "', Ativo = '" + ativo + "', DtAlteracao = getdate() WHERE ID = '" + id + "'";

                if (!synchronusPageMethod(PageMethods.Update, sql))
                    throw "Existiu um erro ao processar o pedido!";

            } catch (e) {
                alert(e);
            }
            finally {
                UpdateTable();
                LoadingVisibility(false);
            }
        }

        function CallDelete(id) {
            try {
                LoadingVisibility(true);

                if (confirm("Deseja realmente eliminar o registo da base de dados?"))
                    if (!synchronusPageMethod(PageMethods.Update, "DELETE FROM Emails WHERE ID = '" + id + "'"))
                        throw "Existiu um erro ao processar o pedido!";

            } catch (e) {
                alert(e);
            }
            finally {
                UpdateTable();
                LoadingVisibility(false);
            }
        }

        function AddNew() {
            try {
                let nome = prompt("Inserir o nome:", "");

                if (nome != null) {
                    LoadingVisibility(true);

                    if (!synchronusPageMethod(PageMethods.Update, "INSERT INTO Emails (Nome, Email, Ativo) VALUES ('" + nome + "', '', 0)"))
                        throw "Existiu um erro ao processar o pedido!";
                }

            } catch (e) {
                alert(e);
            }
            finally {
                UpdateTable();
                LoadingVisibility(false);
            }
        }

        function UpdateTable() {
            try {

                $.ajax({
                    type: "POST",
                    url: "/GestaoEmails.aspx/GetTbl",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        let table = $('#tblEmails').DataTable();

                        //limpar a tabela atual
                        table.clear().draw();

                        //id/nome/email/ativo

                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {
                            table.row.add([item.Id,
                            "<input type=\"text\" id=\'txtNome" + item.Id + "' value='" + item.Nome + "' style=\"width: 100%;\" onchange=\"CallUpdate(" + item.Id + "); return false;\"><p style='display:none;'>" + item.Nome + "</p>",
                            "<input type=\"email\" id=\'txtEmail" + item.Id + "' value='" + item.Email + "' style=\"width: 100%;\" onchange=\"CallUpdate(" + item.Id + "); return false;\"><p style='display:none;'>" + item.Email + "</p>",
                            "<center><button type=\"button\" id='btnAtivo" + item.Id + "' onclick=\"javascript:InvertColor('btnAtivo" + item.Id + "'); CallUpdate(" + item.Id + "); return false;\" class=\"btn " + (item.Ativo ? "btn-success" : "btn-inverse-dark") + " btn-rounded btn-icon\"><i class=\"icon-check\"></i></button></center><p style='display:none;'>" + (item.Ativo ? "Sim" : "Não") + "</p>",
                                item.DtAlteracao,
                                "<center><button type=\"button\" onclick=\"javascript: CallDelete(" + item.Id + ");\" class=\"btn btn-danger btn-icon\"><i class=\"icon-trash\"></i></button></center>"
                            ]);
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
                alert(e);
            }
        }

    </script>

</asp:Content>
