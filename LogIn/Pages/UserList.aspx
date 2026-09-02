<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="LogIn.Pages.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />


    <div class="card text-center">
        <div class="card-header d-flex align-items-center justify-content-between">
            <h3>User List</h3>

            <div class="d-flex align-items-center">
                <div class="search-bar">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control search-input" placeholder="Search Users..." onkeyup="searchUser()" />
                </div>

                <div class="new ml-2">
                    <asp:LinkButton Style="color: green;" runat="server" ID="createnew" CssClass="btn-action" OnClick="createnew_Click" CommandArgument='<%# Eval("User_Mail_Id") %>'>
                    <span><i title="Create New" class='bx bxs-message-alt-add'></i></span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>

        <asp:GridView ID="gvUserDetails" AutoGenerateColumns="false" runat="server" CssClass="gvListDetails">

            <Columns>
                <asp:BoundField DataField="User_Name" HeaderText="User Name" />
                <asp:BoundField DataField="User_Mail_Id" HeaderText="Mail ID" />

                <asp:BoundField DataField="Mobile_No" HeaderText="Mobile Number" />
                <asp:BoundField DataField="Active" HeaderText="Active" />
                <asp:BoundField DataField="Department" HeaderText="Department" />
                <asp:BoundField DataField="Level" HeaderText="Level" />
                <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" />


                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton Style="color: green;" runat="server" ID="edit" CssClass="btn-action" OnClick="edit_Click" CommandArgument='<%# Eval("User_Mail_Id") %>'>
              <span><i title="Edit" class='bx bxs-edit'></i></span>
                        </asp:LinkButton>

                        <asp:LinkButton Style="color: red;" runat="server" ID="delete" CssClass="btn-action" OnClick="delete_Click"
                            OnClientClick="return confirm('Are you sure you want to delete this User?');" CommandArgument='<%# Eval("User_Mail_Id") %>'>
              <span><i  title="Delete" class='bx bx-trash'></i></i></span>
                        </asp:LinkButton>


                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>

    <script>
        function searchUser() {
            // Get the search input value
            var input = document.getElementById('<%= txtSearch.ClientID %>').value.toLowerCase();

            // Get the GridView table and its rows
            var gridView = document.getElementById('<%= gvUserDetails.ClientID %>');
            var rows = gridView.getElementsByTagName("tr");

            // Loop through the rows (start from 1 to skip the header row)
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                var cells = row.getElementsByTagName("td");
                var match = false;

                // Check each cell in the row for a match with the search query
                for (var j = 0; j < cells.length; j++) {
                    var cellText = cells[j].innerText || cells[j].textContent;
                    if (cellText.toLowerCase().indexOf(input) > -1) {
                        match = true;
                        break;
                    }
                }

                // If a match is found, display the row; otherwise, hide it
                row.style.display = match ? "" : "none";
            }
        }
    </script>


</asp:Content>
