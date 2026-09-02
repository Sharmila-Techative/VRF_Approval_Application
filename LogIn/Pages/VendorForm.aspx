<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendorForm.aspx.cs" Inherits="LogIn.Pages.VendorForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Bootstrap 5 JS (bundle includes Popper) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

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

        .RedBtn {
            background-color: #e63946; /* modern red */
            color: #fff;
            border: none;
            padding: 8px 20px;
            font-size: 14px;
            font-weight: 600;
            border-radius: 5px;
            cursor: pointer;
            transition: 0.3s ease;
            text-align-last: center;
        }

            .RedBtn:hover {
                background-color: #c1121f; /* dark red hover */
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
                width: 80%; /* Ensure labels have a fixed width */
                text-align: right; /* Align the text to the right */
                margin-right: 180px; /* Add some space between the label and input */
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

        .modal {
            position: fixed !important;
            z-index: 2000 !important;
        }

        .modal-backdrop {
            z-index: 1050 !important;
        }
        /* Overlay background */
        .custom-popup {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.55);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
        }

        /* Popup container */
        .modern-popup {
            background: #ffffff;
            border-radius: 12px;
            width: 420px;
            max-width: 95%;
            padding: 25px 30px;
            box-shadow: 0 6px 25px rgba(0, 0, 0, 0.25);
            animation: popupFadeIn 0.3s ease;
        }

        /* Header */
        .custom-popup-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2px solid #e0e0e0;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }

        .popup-title {
            font-size: 1.3rem;
            font-weight: 600;
            color: #333;
            margin: 0;
        }

        .close-popup {
            font-size: 22px;
            cursor: pointer;
            color: #666;
        }

            .close-popup:hover {
                color: #000;
            }

        /* Body */
        .popup-textbox {
            width: 100%;
            height: 70px;
            border: 1px solid #ccc;
            border-radius: 6px;
            padding: 8px 10px;
            resize: none;
            font-size: 14px;
            margin-bottom: 20px;
        }

        .popup-label {
            font-weight: 500;
            color: #333;
            display: block;
            margin-bottom: 6px;
        }

        .popup-dropdown {
            width: 100%;
            height: 35px;
            border: 1px solid #ccc;
            border-radius: 6px;
            padding-left: 10px;
            font-size: 14px;
            margin-right: 70px;
        }

        /* Footer Button */
        .popup-actions {
            text-align: center;
            margin-top: 25px;
        }

        .popup-btn {
            background-color: #e63946;
            color: #fff;
            border: none;
            border-radius: 6px;
            padding: 8px 20px;
            font-size: 14px;
            cursor: pointer;
            transition: background 0.3s ease;
        }

            .popup-btn:hover {
                background-color: #c81e2f;
            }

        /* Animation */
        @keyframes popupFadeIn {
            from {
                opacity: 0;
                transform: scale(0.9);
            }

            to {
                opacity: 1;
                transform: scale(1);
            }
        }
        /* Prevent text wrapping */
        .nowrap-text {
            white-space: nowrap;
        }

        .left-align {
            text-align: left;
            display: block;
            width: 100%;
            margin-bottom: 6px;
            margin-right: 30px;
        }

        .user-name-label {
            font-size: 1.5em;
            font-weight: bold;
            color: #333;
            top: 15px;
            right: 20px;
            position: fixed;
        }

        .user-name-label {
            font-size: 1.5em;
            font-weight: bold;
            color: #333;
            top: 15px;
            right: 20px;
            position: fixed;
        }

        .container {
            display: flex;
            justify-content: flex-start !important;
            align-items: flex-start;
            margin: 20px;
        }

        #fullScreenLoader {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 99999;
            display: none;
        }

        .loader-content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
        }
    </style>

    <!-- Fixed User Header Bar -->
    <div class="User">
        <%--<asp:Image ID="imgProfile" runat="server" CssClass="imgProfile" />--%>
        <asp:Label ID="lblUserName" runat="server" CssClass="user-name-label"></asp:Label>
    </div>

    <div style="margin-top: 50px;">

        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="tabs" Width="100%" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">

            <!-- Approval Waiting Status Tab -->
            <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Approval Waiting Status">
                <ContentTemplate>
                    <asp:GridView ID="GridView4" AutoGenerateColumns="false" runat="server" DataKeyNames="GSTnO" CssClass="gvListDetails" OnRowCommand="gvUserDetails_RowCommand1" Style="margin-top: 45px; margin-left: 30px;">
                        <Columns>
                            <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                            <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                            <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                            <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                            <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                            <asp:BoundField DataField="ApprovalWaiting" HeaderText="Waiting For Approval" />
                            <asp:BoundField DataField="Level" HeaderText="DepartmentLevel" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!-- Draft Status-->
            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Draft Status">
                <ContentTemplate>
                    <asp:GridView ID="GridView3" AutoGenerateColumns="false" runat="server" DataKeyNames="GSTnO" CssClass="gvListDetails" OnRowCommand="gvUserDetails_RowCommand1" Style="margin-top: 45px; margin-left: 30px;">
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Select">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="toggleSelectAll(this)" />
                                <span style="margin-left: 5px;">Select</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="rowCheckbox" />
                            </ItemTemplate>--%>
                            <%--</asp:TemplateField>--%>
                            <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                            <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                            <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                            <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                            <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View1.png"
                                        CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Details" Width="36px" Height="36px"
                                        Style="color: #B2BEB5; padding: 5px; border-radius: 3px;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview">
                                <ItemTemplate>
                                    <asp:Button ID="btnPreview" runat="server"
                                        Text="Preview"
                                        CommandName="Preview"
                                        CommandArgument="<%# Container.DataItemIndex %>"
                                        ToolTip="Preview Details"
                                        Width="80px" Height="36px"
                                        Style="background-color: #007BFF; color: white; border: none; border-radius: 4px; padding: 5px; cursor: pointer; font-weight: bold;" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>


                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!-- Pending Status-->
            <ajaxToolkit:TabPanel ID="TabPanelPending" runat="server" HeaderText="Pending Status">
                <ContentTemplate>


                    <asp:GridView ID="gvUserDetails" AutoGenerateColumns="false" runat="server" DataKeyNames="GSTnO" CssClass="gvListDetails" OnRowCommand="gvUserDetails_RowCommand" Style="margin-top: 45px; margin-left: 30px;">
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Select">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="toggleSelectAll(this)" />
                                <span style="margin-left: 5px;">Select</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="rowCheckbox" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                            <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                            <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                            <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                            <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View1.png"
                                        CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Details" Width="36px" Height="36px"
                                        Style="color: #B2BEB5; padding: 5px; border-radius: 3px;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approve">
                                <ItemTemplate>

                                    <asp:ImageButton ID="btnApprove" runat="server" ImageUrl="~/images/Approval.jpg"
                                        CommandName="Approve" ToolTip="Approve" Width="36px" Height="36px"
                                        OnClientClick='<%# "openApproveModal(\"" + Eval("GstNo") + "\"); return false;" %>'
                                        Style="padding: 5px; border-radius: 3px;" />
                                    <%-- OnClientClick="return confirmApprove();" OnClick="ApproveVendor1"--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reject">
                                <ItemTemplate>

                                    <asp:ImageButton ID="btnReject" runat="server" ImageUrl="~/images/Reject.jpg"
                                        CommandName="Reject" ToolTip="Reject" Width="36px" Height="36px" OnClientClick='<%# "BankDetailspopup1(\"" + Eval("GstNo") + "\"); return false;" %>'
                                        Style="color: black; padding: 5px; border-radius: 3px;" />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview">
                                <ItemTemplate>
                                    <asp:Button ID="btnPreview" runat="server"
                                        Text="Preview" CommandName="Preview" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Preview Details" Width="80px" Height="36px"
                                        Style="background-color: #007BFF; color: white; border: none; border-radius: 4px; padding: 5px; cursor: pointer; font-weight: bold;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Panel ID="pnlActions" runat="server" Visible="false" Style="margin-top: 20px; margin-left: 30px;">
                        <%--<asp:Button ID="btnAcceptAll" runat="server" Text="Approve Selected" CssClass="btn btn-success" OnClientClick="return confirmApprove();" OnClick="btnAcceptAll_Click" />
                    <asp:Button ID="btnRejectAll" runat="server" Text="Reject Selected" CssClass="btn btn-danger" Style="margin-left: 10px;"
                        OnClientClick="BankDetailspopup(); return false;" />--%>
                    </asp:Panel>
                    <%--<asp:GridView ID="GridView1" AutoGenerateColumns="false" runat="server" Style="margin-top: 45px; margin-left: 30px;" CssClass="gvListDetails">
                        <Columns>


                            <asp:BoundField DataField="Series" HeaderText="Series" />
                            <asp:BoundField DataField="TName" HeaderText="Trade Name" />

                            <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                        </Columns>
                    </asp:GridView>
                    <div style="margin-top: 20px; margin-left: 30px;">
                        <asp:Button
                            ID="btnPerformOperation"
                            runat="server"
                            Text="Push To SAP"
                            OnClick="btnPerformOperation_Click" />
                    </div>--%>
                </div>
                <div id="BankDetailspopup" class="custom-popup" style="display: none;">
                    <div class="custom-popup-content">
                        <!-- Popup Header -->
                        <div class="custom-popup-header">
                            <h5>Reason</h5>
                            <span class="close-popup" onclick="closePopup()">&times;</span>
                        </div>

                        <!-- Popup Body -->
                        <div class="custom-popup-body">
                            <div class="form-group">
                                <!-- ASP.NET TextBox -->
                                <asp:TextBox ID="popuptext" runat="server" CssClass="form-control" placeholder="Enter your reason here">                                </asp:TextBox>

                                <br />

                                <!-- Submit Button -->
                                <center>
                                    <asp:Button ID="SubmitButton" runat="server" CssClass="RedBtn" OnClick="btnRejectAll_Click" Text="OK" BackColor="Red" Width="50px" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>

                    <div id="BankDetailspopup1" class="custom-popup" style="display: none;">
                        <div class="custom-popup-content modern-popup">
                            <!-- Header -->
                            <div class="custom-popup-header">
                                <h4 class="popup-title">Reason</h4>
                                <span class="close-popup" onclick="closePopup1()">&times;</span>
                            </div>

                            <!-- Body -->
                            <div class="custom-popup-body">
                                <!-- Reason TextBox -->
                                <asp:TextBox
                                    ID="popuptext1"
                                    runat="server"
                                    CssClass="popup-textbox"
                                    placeholder="Enter your reason here">
                                </asp:TextBox>

                                <!-- GST Recreate Dropdown -->
                                <div class="form-group">
                                    <%--<label for="ddlGstRecreate" class="popup-label">Is this GST allowed to be recreated?</label>--%>
                                    <label for="ddlGstRecreate" class="popup-label nowrap-text left-align">
                                        Is this GST allowed to be recreated?
                                    </label>
                                    <asp:DropDownList
                                        ID="ddlGstRecreate"
                                        runat="server"
                                        CssClass="popup-dropdown">
                                        <asp:ListItem Text="Select" Value="" />
                                        <asp:ListItem Text="Yes" Value="Yes" />
                                        <asp:ListItem Text="No" Value="No" />
                                    </asp:DropDownList>
                                </div>

                                <!-- Action Button -->
                                <div class="popup-actions">
                                    <asp:Button
                                        ID="SubmitButton1"
                                        runat="server"
                                        CssClass="popup-btn"
                                        OnClick="btnReject" OnClientClick="showLoader(this); return false;"
                                        Text="OK" />
                                </div>
                            </div>
                        </div>
                    </div>


                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!-- Completed Status-->
            <ajaxToolkit:TabPanel ID="TabPanelCompleted" runat="server" HeaderText="Completed Status">
                <ContentTemplate>
                    <div style="margin-top: 50px; margin-left: 50px;">
                        <asp:GridView ID="gvCompleted" AutoGenerateColumns="false" runat="server" CssClass="gvListDetails" OnRowCommand="gvCompleted_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                                <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                                <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                                <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                                <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                                <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" />
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/View1.png"
                                            CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="View Details" Width="36px" Height="36px"
                                            Style="color: #B2BEB5; padding: 5px; border-radius: 3px;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Preview">
                                    <ItemTemplate>
                                        <asp:Button ID="btnPreview" runat="server"
                                            Text="Preview"
                                            CommandName="Preview"
                                            CommandArgument="<%# Container.DataItemIndex %>"
                                            ToolTip="Preview Details"
                                            Width="80px" Height="36px"
                                            Style="background-color: #007BFF; color: white; border: none; border-radius: 4px; padding: 5px; cursor: pointer; font-weight: bold;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>


                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!-- Rejected Status-->
            <ajaxToolkit:TabPanel ID="TabPanelRejected" runat="server" HeaderText="Rejected Status">
                <ContentTemplate>
                    <div style="margin-top: 50px; margin-left: 50px;">
                        <asp:GridView ID="gvRejected" AutoGenerateColumns="false" runat="server" CssClass="gvListDetails" OnRowCommand="gvRejected_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="TName" HeaderText="Trade Name" />
                                <asp:BoundField DataField="Bstate" HeaderText="Business State" />
                                <asp:BoundField DataField="NatureOfBusinessActivity" HeaderText="Nature of Business Activity" />
                                <asp:BoundField DataField="GstNo" HeaderText="GST Number" />
                                <asp:BoundField DataField="DateOfEstablishment" HeaderText="Applied Date of the Vendor" />
                                <asp:BoundField DataField="RejectedDate" HeaderText="Rejected Date" />
                                <asp:BoundField DataField="RejectedReason" HeaderText="Rejected Reason" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <%-- <ajaxToolkit:TabPanel ID="TabSAPPush" runat="server" HeaderText="Push To Sap">
            <ContentTemplate>
                <div style="margin-top: 50px; margin-left: 50px;">
                    <asp:HiddenField ID="hiddenGSTNo" runat="server" />
                    
                    <div style="margin-top: 20px; margin-left: 50px;">
                        <asp:CheckBox ID="chkVI" runat="server" Text="VI" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkVT" runat="server" Text="VT" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkVC" runat="server" Text="VC" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkIHB" runat="server" Text="IHB" />
                    </div>

                    <asp:GridView ID="GridView1"
                        runat="server"
                        AutoGenerateColumns="true"
                        CssClass="gvListDetails"
                        Style="margin-top: 45px; margin-left: 30px;">
                    </asp:GridView>

                    <div style="margin-top: 20px; margin-left: 30px;">
                        <asp:Button
                            ID="btnPerformOperation"
                            runat="server"
                            Text="Push To SAP"
                            OnClick="btnPerformOperation_Click" />
                    </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>--%>

            <!-- Push To SAP-->
            <ajaxToolkit:TabPanel ID="TabSAPPush" runat="server" HeaderText="Push To Sap">
                <ContentTemplate>
                    <div style="margin-top: 20px; margin-left: 80px;">
                        <input type="text" id="txtSearch"
                            placeholder="Search..."
                            onkeyup="filterGrid()"
                            style="margin-bottom: 10px; width: 200px; height: 28px;" />
                        <asp:HiddenField ID="hdnSearch" runat="server" />
                        <asp:CheckBox ID="chkVI" runat="server" Text="VI" Style="margin-right: 20px; margin-left: 20px" />
                        <asp:CheckBox ID="chkVT" runat="server" Text="VT" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkVC" runat="server" Text="VC" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkIHB" runat="server" Text="IHB" />
                        <asp:DropDownList ID="ddlGroup" runat="server" Width="220px" Style="margin-left: 40px;"></asp:DropDownList>
                    </div>


                    <!-- Four Checkboxes in Horizontal Row -->
                    <%--<div style="margin-top: 20px; margin-left: 90px;">
                    <asp:CheckBox ID="chkVI" runat="server" Text="VI" Style="margin-right: 20px;" />
                    <asp:CheckBox ID="chkVT" runat="server" Text="VT" Style="margin-right: 20px;" />
                    <asp:CheckBox ID="chkVC" runat="server" Text="VC" Style="margin-right: 20px;" />
                    <asp:CheckBox ID="chkIHB" runat="server" Text="IHB" />
                </div>--%>
                    <%--<div style="margin-top: 20px; margin-left: 90px;">
                        <asp:CheckBox ID="chkVI" runat="server" Text="VI" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkVT" runat="server" Text="VT" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkVC" runat="server" Text="VC" Style="margin-right: 20px;" />
                        <asp:CheckBox ID="chkIHB" runat="server" Text="IHB" />
                        <asp:DropDownList ID="ddlGroup" runat="server" Width="220px" Style="margin-left: 80px;"></asp:DropDownList>
                    </div>--%>


                    <div style="margin-top: 50px; margin-left: 50px;">

                        <asp:HiddenField ID="hiddenGSTNo" runat="server" />

                        <!-- GridView With Checkbox + Textbox + Auto fields -->
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" CssClass="gvListDetails" Style="margin-top: 45px; margin-left: 30px;">

                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckedChanged" onclick="saveSearchValue()" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Name">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtVendorName" runat="server" Placeholder="Vendor Name" onkeypress="return allowOnlyAlphaNumeric(event)" oninput="limitVendorName(this)">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>


                        <div style="margin-top: 20px; margin-left: 30px;">
                            <asp:Button ID="btnPerformOperation" runat="server" Text="Push To SAP" OnClick="btnPerformOperation_Click" />
                        </div>
                    </div>

                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!-- Posting Completed to SAP-->
            <ajaxToolkit:TabPanel ID="TabSAPPosted" runat="server" HeaderText="Created Vendor in SAP">
                <ContentTemplate>
                    <div style="margin-top: 50px; margin-left: 50px;">
                        <asp:GridView ID="GridView2"
                            runat="server"
                            AutoGenerateColumns="true"
                            CssClass="gvListDetails"
                            Style="margin-top: 45px; margin-left: 30px;">
                        </asp:GridView>
                    </div>

                </ContentTemplate>
            </ajaxToolkit:TabPanel>

        </ajaxToolkit:TabContainer>

        <!-- Vendor View Modal -->
        <div id="viewModal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title">Vendor Details</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body" style="height: 80vh;">
                        <iframe id="iframeView" src="" width="100%" height="100%" frameborder="0"></iframe>
                    </div>
                </div>
            </div>
        </div>

        <!-- Approval Remarks Modal -->
        <div class="modal fade" id="approveModal" tabindex="-1" aria-labelledby="approveModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title" id="approveModalLabel">Approval Remarks</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hdnGSTNo" runat="server" />
                        <div class="mb-3">
                            <label for="txtRemarks" class="form-label">Enter Remarks:</label>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            <%-- <asp:RequiredFieldValidator
                                ID="rfvRemarks"
                                runat="server"
                                ControlToValidate="txtRemarks"
                                ErrorMessage="Remarks is required."
                                CssClass="text-danger"
                                Display="Dynamic">
    </asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmitApproval" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="ApproveVendor1" OnClientClick="return validateRemarks();" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>

        <!--Loader-->
        <div id="fullScreenLoader" style="display: none;">
            <div class="loader-content">
                <div class="spinner-border text-light" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <div class="mt-2 text-white">Please wait...</div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateRemarks() {
            var remarks = document.getElementById('<%= txtRemarks.ClientID %>').value.trim();

            if (remarks === "") {
                alert("Please enter remarks before submitting.");
                return false; // stop postback
            }
            document.getElementById('fullScreenLoader').style.display = 'block';


            btn.disabled = true;
            return true; // allow postback
        }
        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }


        // ✅ Prevent special characters (allow only A–Z, a–z, 0–9, space)
        function allowOnlyAlphaNumeric(event) {
            const key = event.key;
            const regex = /^[A-Za-z0-9\s]$/;
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
            return true;
        }
        document.addEventListener("DOMContentLoaded", function () {
            const checkboxes = [
                document.getElementById('<%= chkVI.ClientID %>'),
                document.getElementById('<%= chkVT.ClientID %>'),
                document.getElementById('<%= chkVC.ClientID %>'),
                document.getElementById('<%= chkIHB.ClientID %>')
            ];

            checkboxes.forEach(chk => {
                if (chk) {
                    chk.addEventListener("change", function () {
                        // Uncheck all other checkboxes
                        checkboxes.forEach(c => { if (c !== chk) c.checked = false; });

                        // ✅ Clear all Vendor Name textboxes
                        document.querySelectorAll("input[id*='txtVendorName']").forEach(txt => {
                            txt.value = "";
                        });

                        // ✅ Uncheck all GridView "Select" checkboxes
                        document.querySelectorAll("input[id*='chkSelect']").forEach(cb => {
                            cb.checked = false;
                        });
                    });
                }
            });
        });
        // ✅ Limit total length based on selected checkbox
        function limitVendorName(txt) {
            const maxLength = 15;
            let prefix = "";

            // find selected checkbox
            const chkVI = document.getElementById('<%= chkVI.ClientID %>');
            const chkVT = document.getElementById('<%= chkVT.ClientID %>');
            const chkVC = document.getElementById('<%= chkVC.ClientID %>');
            const chkIHB = document.getElementById('<%= chkIHB.ClientID %>');

            if (chkVI && chkVI.checked) prefix = "VI-";
            else if (chkVT && chkVT.checked) prefix = "VT-";
            else if (chkVC && chkVC.checked) prefix = "VC-";
            else if (chkIHB && chkIHB.checked) prefix = "IHB-";

            const allowed = maxLength - prefix.length;
            if (txt.value.length > allowed) {
                txt.value = txt.value.substring(0, allowed);
            }
        }

        // ✅ Clear textbox when any checkbox is toggled
        document.addEventListener("DOMContentLoaded", function () {
            const checkboxes = [
                document.getElementById('<%= chkVI.ClientID %>'),
                document.getElementById('<%= chkVT.ClientID %>'),
                document.getElementById('<%= chkVC.ClientID %>'),
                document.getElementById('<%= chkIHB.ClientID %>')
            ];
            checkboxes.forEach(chk => {
                if (chk) {
                    chk.addEventListener("change", function () {
                        checkboxes.forEach(c => { if (c !== chk) c.checked = false; });
                        document.querySelectorAll("input[id*='txtVendorName']").forEach(txt => {
                            txt.value = "";
                        });
                    });
                }
            });
        });
        document.addEventListener("DOMContentLoaded", function () {
            // Get all checkboxes by class
            const checkboxes = document.querySelectorAll(".chkType");

            checkboxes.forEach(chk => {
                chk.addEventListener("change", function () {
                    if (this.checked) {
                        // Uncheck all others
                        checkboxes.forEach(c => {
                            if (c !== this) c.checked = false;
                        });
                    }

                    // ✅ Clear all Vendor Name textboxes (if present)
                    const textboxes = document.querySelectorAll("input[id*='txtVendorName']");
                    textboxes.forEach(txt => txt.value = "");
                });
            });
        });
        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        <%-- function Sys.Application.add_init(function () {
            Methods.set_path('<%= ResolveUrl("VendorForm.aspx") %>');
        }--%>
        function filterGrid() {
            var input = document.getElementById("txtSearch").value.toLowerCase();
            var grid = document.getElementById("<%= GridView1.ClientID %>");
            var rows = grid.getElementsByTagName("tr");

            for (var i = 1; i < rows.length; i++) {   // Skip header row
                var cells = rows[i].getElementsByTagName("td");
                var match = false;

                for (var j = 0; j < cells.length; j++) {
                    if (cells[j].innerText.toLowerCase().includes(input)) {
                        match = true;
                        break;
                    }
                }

                rows[i].style.display = match ? "" : "none";
            }
        }
        function BankDetailspopup1(GstNo) {
            document.getElementById('<%= hiddenGSTNo.ClientID %>').value = GstNo;
            const popup = document.getElementById('BankDetailspopup1');
            if (popup) {
                popup.style.display = 'flex'; // Show the popup
            } else {
                console.error('Popup element not found.');
            }
        }

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
        function toggleSelectAll(headerCheckbox) {
            // Get the GridView ID rendered by ASP.NET
            var grid = document.getElementById('<%= gvUserDetails.ClientID %>');

            // Loop through all rows (skip header row)
            for (var i = 1; i < grid.rows.length; i++) {
                var row = grid.rows[i];
                // Loop through cells in the row
                for (var j = 0; j < row.cells.length; j++) {
                    var input = row.cells[j].querySelector('input[type="checkbox"]');
                    if (input && input.id.includes("chkSelect")) {
                        input.checked = headerCheckbox.checked;
                        break; // found checkbox in this row, move to next row
                    }
                }
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

        function allowOnlyAlphaNumeric(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (
                (charCode >= 48 && charCode <= 57) || // 0-9
                (charCode >= 65 && charCode <= 90) || // A-Z
                (charCode >= 97 && charCode <= 122) || // a-z
                charCode === 32 // space
            ) {
                return true;
            }
            return false;
        }
        function limitVendorName(txt) {
            var maxLength = 15;
            var prefix = "";

            // 1. Find which checkbox is checked to determine the prefix
            // We search for the checkboxes within the specific container
            var chkVI = document.getElementById('<%= chkVI.ClientID %>');
            var chkVT = document.getElementById('<%= chkVT.ClientID %>');
            var chkVC = document.getElementById('<%= chkVC.ClientID %>');
            var chkIHB = document.getElementById('<%= chkIHB.ClientID %>');

            if (chkVI && chkVI.checked) prefix = "VI-";
            else if (chkVT && chkVT.checked) prefix = "VT-";
            else if (chkVC && chkVC.checked) prefix = "VC-";
            else if (chkIHB && chkIHB.checked) prefix = "IHB-";

            var prefixLength = prefix.length;
            var allowedInputLength = maxLength - prefixLength;

            // 2. If the user has typed more than allowed, truncate it
            if (txt.value.length > allowedInputLength) {
                txt.value = txt.value.substring(0, allowedInputLength);
                alert("Total length (including " + prefix + ") cannot exceed 15 characters.");
            }
        }

        // Keep your existing alphanumeric filter but integrate the length check
        function validateInput(event, txt) {
            // First, allow only alphanumeric
            var keyCode = event.keyCode || event.which;
            var regex = /^[A-Za-z0-9]+$/;
            var isValid = regex.test(String.fromCharCode(keyCode));

            if (!isValid) return false;

            // The length check is better handled on 'onkeyup' or 'oninput' 
            // to catch paste actions, but for 'onkeypress':
            // (Logic for character counting here)
            return true;
        }
        function openViewPopup(gstNo) {
            // Set iframe source to load View.aspx dynamically
            document.getElementById('iframeView').src = '/Pages/View.aspx?gstNumber=' + gstNo;

            // Show the modal (Bootstrap 5 way)
            var myModal = new bootstrap.Modal(document.getElementById('viewModal'));
            myModal.show();
        }
        function openApproveModal(gstNo) {
            document.getElementById('<%= hdnGSTNo.ClientID %>').value = gstNo;
            document.getElementById('<%= txtRemarks.ClientID %>').value = '';

            var myModal = new bootstrap.Modal(document.getElementById('approveModal'));
            myModal.show();
        }



        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }


        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
        function saveSearchValue() {
            var txt = document.getElementById('txtSearch');
            var hidden = document.getElementById('<%= hdnSearch.ClientID %>');
            if (txt && hidden) hidden.value = txt.value;
        }

        // Filter Grid rows based on search text
        function filterGrid() {
            var input = document.getElementById("txtSearch").value.toLowerCase();
            var grid = document.getElementById("<%= GridView1.ClientID %>");
            if (!grid) return;

            var rows = grid.getElementsByTagName("tr");

            for (var i = 1; i < rows.length; i++) { // Skip header row
                var cells = rows[i].getElementsByTagName("td");
                var match = false;

                for (var j = 0; j < cells.length; j++) {
                    if (cells[j].innerText.toLowerCase().includes(input)) {
                        match = true;
                        break;
                    }
                }

                rows[i].style.display = match ? "" : "none";
            }
        }

        // Restore the filter after postback
        function restoreFilter() {
            var hidden = document.getElementById('<%= hdnSearch.ClientID %>');
            if (hidden && hidden.value) {
                document.getElementById('txtSearch').value = hidden.value;
                // Delay filter application until DOM is ready
                setTimeout(filterGrid, 100);
            }
        }

        // Ensure filter is reapplied after each load
        window.addEventListener('load', restoreFilter);

        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }


        function BankDetailspopup1(gstNo) {
            document.getElementById('<%= hiddenGSTNo.ClientID %>').value = gstNo;
            var popup = document.getElementById('BankDetailspopup1');
            if (popup) {
                popup.style.display = 'flex';
            } else {
                console.error('Popup element not found.');
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            // 1️⃣ Get reference to TabContainer
            var tabContainer = $find("<%= TabContainer1.ClientID %>");

            if (tabContainer) {
                // 2️⃣ Check if we have a stored tab index
                var savedTabIndex = sessionStorage.getItem("activeTabIndex");
                if (savedTabIndex !== null) {
                    tabContainer.set_activeTabIndex(parseInt(savedTabIndex));
                }

                // 3️⃣ Listen for tab change
                tabContainer.add_activeTabChanged(function (sender, args) {
                    var activeIndex = sender.get_activeTabIndex();
                    sessionStorage.setItem("activeTabIndex", activeIndex);
                });
            }
        });

        // --- Prevent double POST on refresh (optional but recommended) ---
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        // Prevent form resubmission on refresh (F5 / Reload)
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        // Prevent form resubmission on refresh (F5 / Reload)
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }

        //Loader
        function showLoader(btn) {

            // Prevent multiple clicks
            btn.disabled = true;

            // Show full screen loader
            document.getElementById('fullScreenLoader').style.display = 'block';

            // Continue postback
            __doPostBack(btn.name, '');

            return false;
        }
    </script>

</asp:Content>
