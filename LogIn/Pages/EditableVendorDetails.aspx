<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditableVendorDetails.aspx.cs" Inherits="LogIn.Pages.EditableVendorDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div>
        <asp:GridView ID="gvUserDetails" AutoGenerateColumns="false" runat="server" DataKeyNames="GSTnO" CssClass="gvListDetails"  OnRowCommand="gvUserDetails_RowCommand">
            <Columns>
                <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                <asp:BoundField DataField="LName" HeaderText="Legal Name" />
                <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                <asp:TemplateField HeaderText="View">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View1.png"
                            CommandName="View" CommandArgument="<%# Container.DataItemIndex %>"  ToolTip="View Details" Width="36px" Height="36px"
                            Style="color: #B2BEB5; padding: 5px; border-radius: 3px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Approve">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnApprove" runat="server" ImageUrl="~/images/Approval.jpg"
                            CommandName="Approve" ToolTip="Approve" Width="36px" Height="36px"
                            Style="padding: 5px; border-radius: 3px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reject">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnReject" runat="server" ImageUrl="~/images/Reject.jpg"
                            CommandName="Reject" ToolTip="Reject" Width="36px" Height="36px"
                            Style="color:black; padding: 5px; border-radius: 3px;" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>--%>
    <style>
        .ajax__tab_active,
        .ajax__tab_header .ajax__tab_active a,
        .ajax__tab_header .ajax__tab_active span {
            background-color: #007bff !important;
            color: white !important;
            font-weight: bold;
        }

        /* Optional: make inactive tabs lighter */
        .ajax__tab_header .ajax__tab_tab {
            background-color: #f1f1f1;
            color: black;
        }

        .tabs .ajax__tab {
            background-color: #f1f1f1;
            border: 2px solid #ddd;
            padding: 10px;
        }

        .tabs .ajax__tab__selected {
            background-color: #007bff;
            color: white;
            font-weight: bold;
        }

        .custom-popup {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }

        .custom-popup-content {
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            width: 400px; /* Set a fixed width */
            max-width: 90%;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
        }

        .gvListDetails td,
        .gvListDetails th {
            text-align: center !important;
            vertical-align: middle !important;
        }
        /* Popup header */
        .custom-popup-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 1px solid #ddd;
            padding-bottom: 10px;
            align-items: center;
        }

        /* Form group styling for aligning labels and inputs in a straight line */
        .custom-popup-body .form-group {
            display: flex;
            justify-content: flex-start;
            align-items: center;
            margin-bottom: 15px;
        }

            .custom-popup-body .form-group label {
                width: 30%; /* Ensure labels have a fixed width */
                text-align: right; /* Align the text to the right */
                margin-right: 10px; /* Add some space between the label and input */
            }

            .custom-popup-body .form-group input {
                width: 65%; /* Make the input field take the remaining space */
                border: none;
                padding: 0;
                color: #555;
                background-color: transparent; /* Ensure the input doesn't have background color */
                text-align: left; /* Align the input text to the left */
            }


        /* Ensure the text is centered within the form */
        .custom-popup-body {
            padding-top: 15px;
            padding-bottom: 15px;
        }

        /* Close button hover effect */
        .close-popup {
            font-size: 24px;
            cursor: pointer;
            color: #000;
        }

            .close-popup:hover {
                color: red;
            }

        .suggestion-box div {
            padding: 8px;
            cursor: pointer;
        }

            .suggestion-box div:hover {
                background-color: #eee;
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
    <script>
       <%-- function Sys.Application.add_init(function () {
            Methods.set_path('<%= ResolveUrl("VendorForm.aspx") %>');
        }--%>

        function closePopup1() {
            const popup = document.getElementById('BankDetailspopup1');
            if (popup) {
                popup.style.display = 'none'; // Hide the popup
            } else {
                console.error('Popup element not found.');
            }
        }
        // Function to show the popup
        function showPopup1() {
            document.getElementById("BankDetailspopup1").style.display = "block";
        }
        function showPopup() {
            document.getElementById("BankDetailspopup").style.display = "block";
        }

        // Function to close the popup
        function closePopup() {
            document.getElementById("BankDetailspopup").style.display = "none";
        }
        function BankDetailspopup() {
            console.log("BankDetailspopup function called");
            const popup = document.getElementById('BankDetailspopup');
            if (popup) {
                popup.style.display = 'flex';
            } else {
                console.error('Popup element not found.');
            }
        }
        function closePopup() {
            const popup = document.getElementById('BankDetailspopup');
            if (popup) {
                popup.style.display = 'none'; // Hide the popup
            } else {
                console.error('Popup element not found.');
            }
        }


        function getSuggestions(query) {
            if (query.length === 0) {
                console.log("HI");
                document.getElementById("suggestionBox").style.display = "none";
                return;
            }

            PageMethods.GetSuggestions(query, function (result) {
                console.log("Hello");
                const box = document.getElementById("suggestionBox");
                box.innerHTML = "";

                if (result.length > 0) {
                    result.forEach(function (item) {
                        let div = document.createElement("div");
                        div.innerText = item;
                        div.onclick = function () {
                           <%-- document.getElementById("<%= txtSearch.ClientID %>").value = item;--%>
                            box.style.display = "none";
                        };
                        box.appendChild(div);
                    });

                    <%--const txt = document.getElementById("<%= txtSearch.ClientID %>");--%>
                    box.style.top = (txt.offsetTop + txt.offsetHeight) + "px";
                    box.style.left = txt.offsetLeft + "px";
                    box.style.width = txt.offsetWidth + "px";
                    box.style.display = "block";
                } else {
                    box.style.display = "none";
                }
            });
        }


        function confirmApproveAll() {
            var result = confirm("Are you sure you want to approve all selected vendors?");
            if (result) {
                // User clicked OK → allow postback
                return true;
            } else {
                // User clicked Cancel → cancel postback
                return false;
            }
        }

        function confirmApprove() {
            var result = confirm("Are you sure you want to approve this vendor?");
            if (result) {
                // User clicked OK → allow postback
                return true;
            } else {
                // User clicked Cancel → cancel postback
                return false;
            }
        }


    </script>
    <div class="User">
        <%--<asp:Image ID="imgProfile" runat="server" CssClass="imgProfile" />--%>
        <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>
    </div>
    <div style="margin-top: 50px; margin-left: 10px;">
        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="tabs" Width="100%">

            <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Rejected Details">
                <ContentTemplate>
                    <asp:GridView ID="GridView4" AutoGenerateColumns="false" runat="server" DataKeyNames="GSTnO" CssClass="gvListDetails" OnRowCommand="gvUserDetails_RowCommand1" Style="margin-top: 45px; margin-left: 30px;">
                        <Columns>
                            <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                            <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                            <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                            <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                            <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                            <asp:BoundField DataField="RejectedDate" HeaderText="Rejected Date" />
                            <asp:BoundField DataField="RejectedReason" HeaderText="Rejected Reason" />
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View1.png"
                                        CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Details" Width="36px" Height="36px"
                                        Style="color: #B2BEB5; padding: 5px; border-radius: 3px;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate>
            </ajaxToolkit:TabPanel>

        </ajaxToolkit:TabContainer>
    </div>
</asp:Content>
