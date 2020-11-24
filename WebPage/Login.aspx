<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebPage.Login" Title="Iniciar Sessão" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <!-- plugins:css -->
    <link rel="stylesheet" href="/vendors/simple-line-icons/css/simple-line-icons.css">
    <link rel="stylesheet" href="/vendors/flag-icon-css/css/flag-icon.min.css">
    <link rel="stylesheet" href="/vendors/css/vendor.bundle.base.css">
    <!-- endinject -->
    <!-- Plugin css for this page -->
    <!-- End plugin css for this page -->
    <!-- inject:css -->
    <!-- endinject -->
    <!-- Layout styles -->
    <link rel="stylesheet" href="/css/style.css" />
    <!-- End layout styles -->
    <link rel="shortcut icon" href="/images/favicon.png" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-scroller">
            <div class="container-fluid page-body-wrapper full-page-wrapper">
                <div class="content-wrapper d-flex align-items-center auth">
                    <div class="row flex-grow">
                        <div class="col-lg-4 mx-auto">
                            <div class="auth-form-light text-left p-5">
                                <div class="brand-logo">
                                    <img src="/images/logo-light.svg">
                                </div>
                                <h4>Olá! Vamos iniciar sessão?</h4>
                                <h6 class="font-weight-light">Inserir os dados de utilizador</h6>
                                <form class="pt-3">
                                    <div class="form-group">
                                        <input type="text" class="form-control form-control-lg" id="txtUsername" runat="server" placeholder="Username">
                                    </div>
                                    <div class="form-group">
                                        <input type="password" class="form-control form-control-lg" id="txtPassword" runat="server" placeholder="Password">
                                    </div>
                                    <div class="mt-3">
                                        <a class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" id="btnIniciarSessao" runat="server" onserverclick="btnIniciarSessao_ServerClick">Iniciar Sessão</a>
                                    </div>

                                    <center id="lblError" runat="server"><br />  <mark class="bg-danger text-white"><strong>Oh não!</strong> Alguma coisa correu mal e não conseguimos iniciar sessão!</mark></center>
                                </form>


                            </div>

                        </div>
                    </div>
                </div>
                <!-- content-wrapper ends -->
            </div>
            <!-- page-body-wrapper ends -->
        </div>

        <script type="text/javascript">
            // Get the input field
            var input = document.getElementById('<%=txtPassword.ClientID %>');

            // Execute a function when the user releases a key on the keyboard
            input.addEventListener("keyup", function (event) {
                // Number 13 is the "Enter" key on the keyboard
                if (event.keyCode === 13) {
                    // Cancel the default action, if needed
                    event.preventDefault();

 
                    // Trigger the button element with a click
                    document.getElementById('<%=btnIniciarSessao.ClientID %>').click();

                        }
                    });


        </script>

        <!-- container-scroller -->
        <!-- plugins:js -->
        <script src="/vendors/js/vendor.bundle.base.js"></script>
        <!-- endinject -->
        <!-- Plugin js for this page -->
        <!-- End plugin js for this page -->
        <!-- inject:js -->
        <script src="/js/off-canvas.js"></script>
        <script src="/js/misc.js"></script>
        <!-- endinject -->
    </form>
</body>
</html>
