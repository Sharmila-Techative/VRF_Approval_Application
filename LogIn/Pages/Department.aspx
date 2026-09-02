<%@ Page Title="Department" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Department.aspx.cs" Inherits="LogIn.Pages.Department" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style>
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
        <%--<asp:Image ID="imgProfile" runat="server" CssClass="imgProfile" />--%>
        <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>
    </div>
    <div style="margin-top: 50px; margin-left: 10px;">
        <div class="master-page">
            <div class="heading">
                <h2>Department Management</h2>
                <hr />
            </div>

            <div class="master-form">
                <!-- Department ID -->
                <div class="master-group">
                    <label class="label">Department ID<span class="span1">*</span></label>
                    <asp:TextBox ID="txtDeptID" runat="server" CssClass="master-panel" placeholder="Enter Department ID"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDeptID" runat="server" ControlToValidate="txtDeptID"
                        ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>

                <!-- Department Name -->
                <div class="master-group">
                    <label class="label">Department Name<span class="span1">*</span></label>
                    <asp:TextBox ID="txtDeptName" runat="server" CssClass="master-panel" placeholder="Enter Department Name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDeptName" runat="server" ControlToValidate="txtDeptName"
                        ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>

                <!-- Active/Inactive -->
                <div class="master-group">
                    <label class="label">Active</label>
                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                </div>

                <!-- Buttons -->
                <br />
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="link-button" OnClick="btnAdd_Click">ADD</asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="link-button" OnClick="btnClear_Click" CausesValidation="false">CLEAR</asp:LinkButton>
                </div>

                <hr />
                <h3>Existing Departments</h3>

                <asp:GridView ID="gvDepartments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                    DataKeyNames="DepartmentID" OnRowDeleting="gvDepartments_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="DepartmentID" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                    AutoPostBack="true" OnCheckedChanged="chkActive_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
