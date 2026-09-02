<%@ Page Title="Approver Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Approver.aspx.cs" Inherits="LogIn.Pages.Approver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style>
        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial, sans-serif;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }

            .styled-grid th, .styled-grid td {
                padding: 12px 15px;
                text-align: left;
                border-bottom: 1px solid #ddd;
                text-align: center !important;
            }

            .styled-grid th {
                background-color: #3498db;
                color: #fff;
                text-transform: uppercase;
                letter-spacing: 0.05em;
                text-align: center !important;
            }

            .styled-grid tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            .styled-grid tr:hover {
                background-color: #e0f7fa;
            }

        .action-btn {
            border: none;
            background: none;
            cursor: pointer;
            padding: 5px 10px;
            border-radius: 4px;
            text-decoration: none;
            font-size: 0.9em;
            display: inline-flex;
            align-items: center;
        }

        .edit-btn {
            color: #2ecc71;
        }

        .delete-btn {
            color: #e74c3c;
            margin-left: 5px;
        }

        .action-btn i {
            margin-right: 5px;
        }

        .edit-btn:hover {
            background-color: #2ecc71;
            color: #fff;
        }

        .delete-btn:hover {
            background-color: #e74c3c;
            color: #fff;
        }

        .styled-grid th {
        }

        .user-name-label {
            font-size: 1.5em;
            font-weight: bold;
            color: #333;
            top: 15px;
            right: 20px;
            position: fixed;
        }
    </style>
    <div class="User">
        <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>
    </div>
    <div style="margin-top: 50px; margin-left: 10px;">
        <div class="container mt-4">
            <h3 class="mb-4">Approver Master</h3>
            <div class="form-container" style="max-width: 600px; margin-bottom: 20px;">
                <asp:HiddenField ID="hdnID" runat="server" />

                <div class="form-group mb-3">
                    <label for="ddlDepartment">Approver Department</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>

                <div class="form-group mb-3">
                    <label for="txtCount">Department Approver Level</label>
                    <div class="input-group">
                        <button type="button" class="btn btn-outline-secondary" onclick="decrementCount1()">−</button>
                        <asp:TextBox ID="txtLevel" runat="server" CssClass="form-control text-center" Text="1"></asp:TextBox>
                        <button type="button" class="btn btn-outline-secondary" onclick="incrementCount1()">+</button>
                    </div>
                </div>

                <div class="form-group mb-3">
                    <label for="txtCount">Department Approver Count</label>
                    <div class="input-group">
                        <button type="button" class="btn btn-outline-secondary" onclick="decrementCount()">−</button>
                        <asp:TextBox ID="txtCount" runat="server" CssClass="form-control text-center" Text="1"></asp:TextBox>
                        <button type="button" class="btn btn-outline-secondary" onclick="incrementCount()">+</button>
                    </div>
                </div>

                <div class="form-group text-center">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary mb-2" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success mb-2" Visible="false" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                </div>

                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            </div>
            <div style="margin-left: 15px;">
                <asp:GridView ID="gvApprover" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" OnRowCommand="gvApprover_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="ApproverDepartment" HeaderText="Department" />
                        <asp:BoundField DataField="Level" HeaderText="Level" />
                        <asp:BoundField DataField="DepartmentApproverCount" HeaderText="Approve Count" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRow" CommandArgument='<%# Eval("ID") %>' CssClass="action-btn edit-btn">
                    <i class="bx bxs-edit"></i> Edit
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' CssClass="action-btn delete-btn" OnClientClick="return confirm('Are you sure?');">
                    <i class="bx bx-trash"></i> Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>

        <script>
            function incrementCount() {
                var txt = document.getElementById('<%= txtCount.ClientID %>');
                var val = parseInt(txt.value || "0");
                txt.value = val + 1;
            }

            function decrementCount() {
                var txt = document.getElementById('<%= txtCount.ClientID %>');
                var val = parseInt(txt.value || "0");
                if (val > 1) txt.value = val - 1;
            }
            function incrementCount1() {
                var txt = document.getElementById('<%= txtLevel.ClientID %>');
                var val = parseInt(txt.value || "0");
                txt.value = val + 1;
            }

            function decrementCount1() {
                var txt = document.getElementById('<%= txtLevel.ClientID %>');
                var val = parseInt(txt.value || "0");
                if (val > 1) txt.value = val - 1;
            }
        </script>
    </div>
</asp:Content>
