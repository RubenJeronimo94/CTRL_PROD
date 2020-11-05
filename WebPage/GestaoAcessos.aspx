<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestaoAcessos.aspx.cs" Inherits="WebPage.GestaoAcessos" MasterPageFile="/MasterPage.Master" Title="Gestão de Acessos" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">


    <div class="col-12 grid-margin stretch-card">
        <div class="card">
            <div class="card-body">
                <h4 class="card-title">Gestão de Acessos</h4>
                <table id="tblAcessos" class="display" style="width: 100%; font-size: 14px;">
                    <colgroup>
                        <col style="width: 50px;" />
                        <col style="width: auto;" />
                        <col style="width: auto;" />
                        <col style="width: auto;" />
                        <col style="width: 80px;" />
                        <col style="width: 80px;" />
                        <col style="width: 160px;" />
                        <col style="width: 30px;" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nome</th>
                            <th>Username</th>
                            <th>Password</th>
                            <th>Nivel</th>
                            <th>Avatar</th>
                            <th>Último Acesso</th>
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
                    <i class="icon-user-follow"></i>&nbsp Adicionar novo utilizador
                </button>
            </div>
        </div>
    </div>




    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () { //corre script no load

            try {
                $.noConflict();
                $('#tblAcessos').DataTable({
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
                        "zeroRecords": "Não existem utilizadores registados!",
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


        function CallUpdate(id) {
            try {
                LoadingVisibility(true);

                let nome = document.getElementById("txtNome" + id).value;
                let username = document.getElementById("txtUsername" + id).value;
                let password = document.getElementById("txtPassword" + id).value;
                let nivel = document.getElementById("cmbNivel" + id).value;
                let avatar = document.getElementById("cmbAvatar" + id).value;

                let sql = "UPDATE Utilizadores SET Nome = '" + nome + "', Username = '" + username + "', Password = '" + password + "', Nivel = '" + nivel + "', Avatar = '" + avatar + "' WHERE ID = '" + id + "'";

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
                    if (!synchronusPageMethod(PageMethods.Update, "DELETE FROM Utilizadores WHERE ID = '" + id + "'"))
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

                    if (!synchronusPageMethod(PageMethods.Update, "INSERT INTO Utilizadores (Nome, Username, Password, Nivel, Avatar) VALUES ('" + nome + "', '" + nome + "', '', 0, 1)"))
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
                    url: "/GestaoAcessos.aspx/GetTbl",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        let table = $('#tblAcessos').DataTable();

                        //limpar a tabela atual
                        table.clear().draw();

                        //id/nome/email/ativo

                        //Faz um for por cada campo
                        $.each($.parseJSON(response.d), function (i, item) {
                            table.row.add([item.Id,
                            "<input type=\"text\" id=\'txtNome" + item.Id + "' value='" + item.Nome + "' style=\"width: 100%;\" onchange=\"CallUpdate(" + item.Id + "); return false;\"><p style='display:none;'>" + item.Nome + "</p>",
                            "<input type=\"text\" id=\'txtUsername" + item.Id + "' value='" + item.Username + "' style=\"width: 100%;\" onchange=\"CallUpdate(" + item.Id + "); return false;\"><p style='display:none;'>" + item.Username + "</p>",
                            "<input type=\"text\" id=\'txtPassword" + item.Id + "' value='" + item.Password + "' style=\"width: 100%;\" onchange=\"CallUpdate(" + item.Id + "); return false;\"><p style='display:none;'>" + item.Password + "</p>",
                            "<select id=\'cmbNivel" + item.Id + "' onchange=\"CallUpdate(" + item.Id + "); return false;\">" +
                            "<option value=\"0\" " + (item.Nivel == 0 ? "selected" : "") + "> </option>" +
                            "<option value=\"1\" " + (item.Nivel == 1 ? "selected" : "") + "> Utilizador</option>" +
                            "<option value=\"2\" " + (item.Nivel == 2 ? "selected" : "") + "> Administrador</option>" +
                            "</select>",
                            "<select id=\'cmbAvatar" + item.Id + "' onchange=\"CallUpdate(" + item.Id + "); return false;\">" +
                            "<option value=\"1\" " + (item.Avatar == 1 ? "selected" : "") + "> Avatar 1 </option>" +
                            "<option value=\"2\" " + (item.Avatar == 2 ? "selected" : "") + "> Avatar 2</option>" +
                            "<option value=\"3\" " + (item.Avatar == 3 ? "selected" : "") + "> Avatar 3</option>" +
                            "<option value=\"4\" " + (item.Avatar == 4 ? "selected" : "") + "> Avatar 4</option>" +
                            "</select>",
                            item.UltimoAcesso,
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
