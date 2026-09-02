<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="LogIn.Pages.LoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <body class="background-image">

        <div class="container">

            <div class="login-wrapper">
                <div class="logo-panel">
                    <img src="~/Images/Logo.png" alt="Company Logo" class="logo-image" runat="server" />
                </div>
            </div>
            <div class="user-page , login">
                <h2>Login</h2>
                <hr />
                <%-- <div class="master-form">

                    <div class="master-group  input-panel ">
                        <label class="label">User Name / E-mail <span class="span1">*</span> </label>
                        <asp:TextBox ID="UsernameTextBox" CssClass="master-panel" placeholder="Username" runat="server" />
                        <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator7" runat="server" ControlToValidate="UsernameTextBox" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator><br />
                    </div>
                    <br />

                    <div class="master-group">
                        <label class="label">Password <span class="span1">*</span></label>
                        <div class="password-container input-panel">
                            <asp:TextBox ID="PasswordTextBox" CssClass="master-panel" TextMode="Password" placeholder="Password" runat="server" />
                            <span class="password-toggle-icon" data-target="#PasswordTextBox">
                                <i class="fas fa-eye-slash"></i>
                            </span>
                        </div>
                        <asp:RequiredFieldValidator CssClass="errormsg" ID="RequiredFieldValidator8" runat="server" ControlToValidate="PasswordTextBox" ForeColor="Red" Display="Dynamic" ErrorMessage="Field Required."></asp:RequiredFieldValidator><br />
                    </div>
                    <br />

                    <div>
                        <asp:LinkButton ID="LoginValidate" class="link-button" type="button" OnClick="LoginValidate_Click" runat="server" Visible="false">LOGIN</asp:LinkButton>
                        <asp:LinkButton ID="ResetPass" class="link-button" type="button" OnClick="ResetPass_Click" runat="server">Reset Passwoed</asp:LinkButton>
                    </div>
                    <br />

                    <asp:Label CssClass="errormsg" ID="ErrorMessageLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </div>--%>

                <div class="master-form">

                    <!-- LOGIN PANEL -->
                    <asp:Panel ID="pnlLogin" runat="server">

                        <div class="master-group input-panel">
                            <label class="label">User Name / E-mail <span class="span1">*</span></label>
                            <asp:TextBox ID="UsernameTextBox" CssClass="master-panel"
                                runat="server" placeholder="Username"></asp:TextBox>
                        </div>

                        <br />

                        <div class="master-group">
                            <label class="label">Password <span class="span1">*</span></label>
                            <div class="password-container input-panel">
                                <asp:TextBox ID="PasswordTextBox" CssClass="master-panel" TextMode="Password" placeholder="Password" runat="server" />
                                <span class="password-toggle-icon" data-target="#PasswordTextBox">
                                    <i class="fas fa-eye-slash"></i>
                                </span>

                            </div>
                        </div>

                    </asp:Panel>


                    <!-- RESET PASSWORD PANEL -->
                    <asp:Panel ID="pnlResetPassword" runat="server" Visible="false">

                        <div class="master-group">
                            <label>Old Password</label>
                            <div class="password-container input-panel">
                                <asp:TextBox ID="txtOldPassword"
                                    runat="server"
                                    CssClass="master-panel"
                                    TextMode="Password"
                                    autocomplete="off">
                                </asp:TextBox>
                                <span class="password-toggle-icon" data-target="#txtOldPassword">
                                    <i class="fas fa-eye-slash"></i>
                                </span>
                            </div>
                        </div>

                        <br />

                        <div class="master-group">
                            <label>New Password</label>
                            <div class="password-container input-panel">
                                <asp:TextBox ID="txtNewPassword"
                                    runat="server"
                                    CssClass="master-panel"
                                    TextMode="Password"
                                    autocomplete="off">
                                </asp:TextBox>
                                <span class="password-toggle-icon" data-target="#txtNewPassword">
                                    <i class="fas fa-eye-slash"></i>
                                </span>
                            </div>
                        </div>

                        <br />

                        <div class="master-group">
                            <label>Confirm Password</label>
                            <div class="password-container input-panel">
                                <asp:TextBox ID="txtConfirmPassword"
                                    runat="server"
                                    CssClass="master-panel"
                                    TextMode="Password"
                                    autocomplete="off">
                                </asp:TextBox>
                                <span class="password-toggle-icon" data-target="#txtConfirmPassword">
                                    <i class="fas fa-eye-slash"></i>
                                </span>
                            </div>
                            <asp:CompareValidator CssClass="errormsg" ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPassword" ForeColor="Red" Display="Dynamic" ErrorMessage="Password does not match."></asp:CompareValidator>

                        </div>

                    </asp:Panel>

                    <br />

                    <div class="button-group">
                        <asp:LinkButton ID="LoginValidate"
                            runat="server"
                            CssClass="btn btn-primary"
                            OnClick="LoginValidate_Click">
                            LOGIN
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnUpdatePassword"
                            runat="server"
                            CssClass="btn btn-primary text-nowrap"
                            Visible="false"
                            OnClick="btnUpdatePassword_Click">
                            UPDATE PASSWORD
                        </asp:LinkButton>

                        <asp:LinkButton ID="ResetPass"
                            runat="server"
                            CssClass="btn btn-primary text-nowrap"
                            OnClick="ResetPass_Click"
                            CausesValidation="false">
                            RESET PASSWORD
                        </asp:LinkButton>
                    </div>

                    <br />

                    <asp:Label ID="ErrorMessageLabel"
                        runat="server"
                        CssClass="errormsg"
                        Visible="false">
                    </asp:Label>

                </div>
            </div>
        </div>
    </body>
    <script type="text/javascript">
        document.querySelectorAll('.password-toggle-icon').forEach(icon => {
            icon.addEventListener('click', function () {
                const input = this.previousElementSibling;
                if (input.type === 'password') {
                    input.type = 'text';
                    this.innerHTML = '<i class="fas fa-eye"></i>';
                } else {
                    input.type = 'password';
                    this.innerHTML = '<i class="fas fa-eye-slash"></i>';
                }
            });
        });

        document.addEventListener("keypress", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                document.getElementById("<%= LoginValidate.ClientID %>").click();
            }
        });


    </script>
    <style>
        .logo-image {
            width: 200px;
            height: 80px; /* Set exact height */
            object-fit: contain; /* Keeps logo proportions */
        }


        .logo-panel {
            flex: 1;
            text-align: center;
            padding-right: 20px;
            border-right: 1px solid #ccc;
        }

        .logo-image {
            width: 290px;
            height: auto;
            margin: 10px 0;
            margin-left: 100px;
            margin-top: 200px;
        }

        .button-group {
            display: flex;
            gap: 10px; /* space between buttons */
            align-items: center;
        }

        .link-button {
            display: inline-block;
        }
        /*.user-page {
            flex: 1.2;
            padding-left: 30px;
        }*/

        .user-page {
            flex: none;
            width: 380px;
            padding-left: 30px; /* same as before */
        }

        .password-container {
            position: relative;
            width: 100%;
        }

            .password-container .master-panel {
                width: 100%;
                box-sizing: border-box;
                padding-right: 35px; /* room for eye icon */
            }

        .password-toggle-icon {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
        }


        .master-panel {
            width: 320px; /* choose your desired width */
            box-sizing: border-box;
        }

        .link-button {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 10px 30px;
            border-radius: 6px;
            cursor: pointer;
            text-decoration: none;
            font-weight: bold;
        }

            .link-button:hover {
                background-color: #0056b3;
            }
    </style>
</asp:Content>
